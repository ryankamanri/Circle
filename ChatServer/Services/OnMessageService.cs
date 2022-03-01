using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using Kamanri.WebSockets;
using Kamanri.WebSockets.Model;
using Kamanri.Extensions;
using Kamanri.Utils;
using ChatServer.Services.Extensions;

namespace ChatServer.Services
{
	public class OnMessageService
	{

		private ILogger<OnMessageService> _logger;
		private IWebSocketMessageService _wsmService;


		private long assignedID;

		private UserService _userService;


		public OnMessageService(IWebSocketMessageService wsmService, ILoggerFactory loggerFactory)
		{
			_wsmService = wsmService;
			_logger = loggerFactory.CreateLogger<OnMessageService>();
			_userService = new UserService(loggerFactory);
			_wsmService.AddEventHandler(WebSocketMessageEvent.OnConnect, OnConnect)
				.AddEventHandler(WebSocketMessageEvent.OnDataSideConnect, OnDataSideConnect)
				.AddEventHandler(WebSocketMessageEvent.OnClientConnect, OnClientConnect)
				.AddEventHandler(WebSocketMessageEvent.OnDataSideTempMessage, OnDataSideTempMessage)
				.AddEventHandler(WebSocketMessageEvent.OnClientMessage, OnClientMessage)
				.AddEventHandler(WebSocketMessageEvent.OnDataSideMessage, OnDataSideMessage)
				.AddEventHandler(WebSocketMessageEvent.OnClientPreviousMessage, OnClientPreviousMessage)
				.AddEventHandler(WebSocketMessageEvent.OnDataSidePreviousMessage, OnDataSidePreviousMessage)
				.AddEventHandler(WebSocketMessageEvent.OnDisconnect, OnDisconnect);
		}

		public Task<IList<WebSocketMessage>> OnConnect(IWebSocketSession client, IList<WebSocketMessage> messages)
		{
			return Task.Run<IList<WebSocketMessage>>(() =>
			{
				var clientAssignedID = Convert.ToInt64(messages[0].Message);
				_logger.LogInformation(messages[0].MessageEvent.Code, $"A Connect Request, Assigned ID : {clientAssignedID}");


				return new List<WebSocketMessage>()
				{
					new WebSocketMessage(WebSocketMessageEvent.OnServerConnect,
						WebSocketMessageType.Text,
						clientAssignedID.ToString())
				};
			});
		}



		/// <summary>
		/// 接受数据端连接事件, 消息协议
		/// {
		///	0 : ID long, Assigned server ID
		/// }, 响应码 : 102
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public Task<IList<WebSocketMessage>> OnDataSideConnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			var eventCode = messages[0].MessageEvent.Code;
			assignedID = Convert.ToInt64(messages[0].Message);

			_logger.LogInformation(eventCode, $"Receive Assigned ID : {assignedID}");

			return WebSocketMessageService.DefaultTask;
		}

		/// <summary>
		/// 接受客户端连接事件, 消息协议
		/// {
		///	0 : assignedID long AUTO ASSIGNED
		///	1 : userID long
		/// }, 响应码 : 100
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnClientConnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{

			// 检查在线用户表中是否存在该用户,若存在把旧连接换成新连接,若不存在添加一个项,(并向数据端发送添加请求)
			var clientAssignedID = Convert.ToInt64(messages[0].Message);
			var userID = Convert.ToInt64(messages[1].Message);
			var eventID = messages[0].MessageEvent.Code;
			_logger.LogInformation(eventID, $"Bind User {{ID = {userID}}} To WebSocketID {clientAssignedID}");

			// 删除旧连接
			if (_userService.onlineUserID_WebSocketIDs.TryGetValue(userID, out var webSocketID))
			{
				await session.ClientClose(webSocketID, WebSocketCloseStatus.NormalClosure,
					"You Have Signed In To Another Platform");
				// 需要当响应OnDisconnect并发送消息到数据端后才能进行下一步
				_userService.onlineUserID_WebSocketIDsMutex[userID].Wait();
			}
			_userService.onlineUserID_WebSocketIDs[userID] = clientAssignedID;
			_userService.onlineUserID_WebSocketIDsMutex[userID] = new Mutex(true);

			await session.ClientSendMessage(
				new List<WebSocketMessage>()
				{
					new WebSocketMessage()
					{
						MessageEvent = WebSocketMessageEvent.OnClientConnect,
						MessageType = WebSocketMessageType.Text,
						Message = userID.ToString()
					},
					new WebSocketMessage()
					{
						MessageEvent = WebSocketMessageEvent.OnClientConnect,
						MessageType = WebSocketMessageType.Text,
						Message = assignedID.ToString()
					}
				});
			return default;
		}

		/// <summary>
		/// 接受数据端返回的临时会话消息, 消息协议
		/// {
		///	 0 : userID long
		///	 messages...
		/// }, 响应码 : 402
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnDataSideTempMessage(IWebSocketSession session, IList<WebSocketMessage> messages)
		{

			var eventID = messages[0].MessageEvent.Code;
			var userID = Convert.ToInt64(messages[0].Message);
			_logger.LogInformation(eventID, $"Receive {messages.Count} Temp Message From DataSide : {messages.ToJson()}");
			if (!_userService.onlineUserID_WebSocketIDs.TryGetValue(userID, out var webSocketID))
				_logger.LogError(eventID, $"Count Of UserService.onlineUserID_WebSockIDs Is {_userService.onlineUserID_WebSocketIDs.Count}");
			messages.RemoveAt(0);// 去掉第一个Message
			messages.SetMessageEvent(WebSocketMessageEvent.OnServerTempMessage);

			await session.ServerSendMessage(webSocketID, messages);
			return default;
		}
		/// <summary>
		/// 接受客户端的消息事件, 消息协议
		/// {
		///	 1 : Message Text Message
		///	 (2 : Message Binary byte[])
		///  }, 响应码 : 300
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnClientMessage(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			messages.SetMessageEvent(WebSocketMessageEvent.OnServerMessage);
			await session.ClientSendMessage(messages);
			return default;
		}

		/// <summary>
		/// code : 302
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="wsMessages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnDataSideMessage(IWebSocketSession session, IList<WebSocketMessage> wsMessages)
		{
			var message = wsMessages.ToMessages(0, 1)[0];
			var receiveWebSocketID = _userService.onlineUserID_WebSocketIDs[message.ReceiveID];
			wsMessages.SetMessageEvent(WebSocketMessageEvent.OnServerMessage);
			await session.ServerSendMessage(receiveWebSocketID, wsMessages);
			return default;
		}




		public Task<IList<WebSocketMessage>> OnDataSideDisconnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			return Task.Run(() => messages);
		}

		/// <summary>
		/// 接受客户端发出的请求历史数据(来自客户端), 消息协议
		/// {
		///	 0 : clientUserID long, 客户端用户
		///	 1 : requestUserID long, 请求用户
		/// }, 响应码 : 500
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnClientPreviousMessage(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			messages.SetMessageEvent(WebSocketMessageEvent.OnServerPreviousMessage);
			await session.ClientSendMessage(messages);
			return default;
		}
		/// <summary>
		/// 接受数据端发出的请求历史数据(来自客户端), 消息协议
		/// {
		///	 0 : clientUserID long, 客户端用户
		///	 1... :  Message
		/// }, 响应码 : 502
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="wsMessages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnDataSidePreviousMessage(IWebSocketSession session, IList<WebSocketMessage> wsMessages)
		{
			var eventID = wsMessages[0].MessageEvent.Code;
			var clientUserID = Convert.ToInt64(wsMessages[0].Message);
			_logger.LogInformation(wsMessages[0].MessageEvent.Code, $"Receive {wsMessages.Count} Previous Messages (In WebSocketMessage Form) From DataSide");
			var receiveWebSocketID = _userService.GetWebSocketIDByOnlineUserID(eventID, clientUserID);
			_logger.LogInformation(eventID, $"Find WebSocketID {receiveWebSocketID} By User ID {clientUserID}");
			wsMessages.SetMessageEvent(WebSocketMessageEvent.OnServerPreviousMessage);
			await session.ServerSendMessage(receiveWebSocketID, wsMessages);
			return default;
		}


		/// <summary>
		/// (客户端)断开连接事件
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnDisconnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			var message = messages[0];
			var eventID = message.MessageEvent.Code;
			var clientWebSocketID = Convert.ToInt64(message.Message as string);
			_logger.LogInformation(message.MessageEvent.Code, messages[1].Message.ToString());

			var userID = _userService.GetOnlineUserIDByWebSocketID(eventID, clientWebSocketID);
			await session.ClientSendMessage(new List<WebSocketMessage>()
			{
				new WebSocketMessage()
				{
					MessageEvent = WebSocketMessageEvent.OnClientDisconnect,
					MessageType = WebSocketMessageType.Text,
					Message = userID.ToString()
				},
				new WebSocketMessage()
				{
					MessageEvent = WebSocketMessageEvent.OnClientDisconnect,
					MessageType = WebSocketMessageType.Text,
					Message = assignedID.ToString()
				}
			});
			_userService.onlineUserID_WebSocketIDs.Remove(userID);
			Mutex mutex;
			if (_userService.onlineUserID_WebSocketIDsMutex.TryGetValue(userID, out mutex))
			{
				mutex.Signal();
			}
			return default;
		}
	}
}