using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using Kamanri.WebSockets;
using Kamanri.WebSockets.Model;
using Kamanri.Extensions;
using Kamanri.Database;
using Kamanri.Utils;
using ChatDataServer.Models;
using ChatDataServer.Services.Extensions;

namespace ChatDataServer.Services
{
	public class OnMessageController
	{

		private ILogger<OnMessageController> _logger;
		private IWebSocketMessageService _wsmService;

		private UserService _userService;

		private MessageService _messageService;




		public OnMessageController(IWebSocketMessageService wsmService, ILoggerFactory loggerFactory, DatabaseContext dbc)
		{
			_wsmService = wsmService;
			_logger = loggerFactory.CreateLogger<OnMessageController>();
			_userService = new UserService(loggerFactory);
			_messageService = new MessageService(dbc, loggerFactory);

			_wsmService.AddEventHandler(WebSocketMessageEvent.OnConnect, OnConnect)
				.AddEventHandler(WebSocketMessageEvent.OnClientConnect, OnClientConnect)
				.AddEventHandler(WebSocketMessageEvent.OnServerConnect, OnServerConnect)
				.AddEventHandler(WebSocketMessageEvent.OnServerMessage, OnServerMessage)
				.AddEventHandler(WebSocketMessageEvent.OnClientDisconnect, OnClientDisconnect)
				.AddEventHandler(WebSocketMessageEvent.OnServerPreviousMessage, OnServerPreviousMessage)
				.AddEventHandler(WebSocketMessageEvent.OnDataSideConnect, OnDataSideConnect)
				.AddEventHandler(WebSocketMessageEvent.OnDataSideDisconnect, OnDataSideDisconnect)
				.AddEventHandler(WebSocketMessageEvent.OnDisconnect, OnDisconnect);

		}

		/// <summary>
		/// 接受连接事件, 消息协议
		/// {
		///	  0 : ID long, assigned ID AUTO ASSIGNED
		/// }, 响应码 : 1
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public Task<IList<WebSocketMessage>> OnConnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			return Task.Run<IList<WebSocketMessage>>(() =>
			{
				var clientAssignedID = Convert.ToInt64(messages[0].Message);
				_logger.LogInformation(messages[0].MessageEvent.Code, $"A Connect Request, Assigned ID : {clientAssignedID}");


				return new List<WebSocketMessage>()
				{
					new WebSocketMessage(WebSocketMessageEvent.OnDataSideConnect,
						WebSocketMessageType.Text,
						clientAssignedID.ToString())
				};
			});
		}



		/// <summary>
		/// 添加用户端事件, 消息协议
		/// {
		///	 0 : UserID long
		///	 1 : serviceID long
		/// }, 响应码 : 100
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public Task<IList<WebSocketMessage>> OnClientConnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			return Task.Run<IList<WebSocketMessage>>(() =>
			{
				var userID = Convert.ToInt64(messages[0].Message);
				var serviceID = Convert.ToInt64(messages[1].Message);
				var eventCode = messages[0].MessageEvent.Code;
				_userService.AppendOnlineUser(eventCode, userID, serviceID);
				_logger.LogInformation(eventCode, $"A User {userID} Connection Request, Client ID = {_userService.OnlineUserID_ServerID[userID]}");



				var tempMessages = _messageService.SelectTempMessagesOfAUser(eventCode, userID);
				var result = new List<WebSocketMessage>();
				result.Add(new WebSocketMessage()
				{
					MessageEvent = WebSocketMessageEvent.OnDataSideTempMessage,
					MessageType = WebSocketMessageType.Text,
					Message = userID.ToString()
				});
				result.AddRange(tempMessages.ToWebSocketMessageList(WebSocketMessageEvent.OnDataSideTempMessage));

				return result;
			});


		}

		/// <summary>
		/// 服务端连接事件, 消息协议
		/// {
		/// 
		/// }, 响应码 : 101
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public Task<IList<WebSocketMessage>> OnServerConnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			// Had Deprecated
			return WebSocketMessageService.DefaultTask;
		}

		/// <summary>
		/// 接受服务端的消息事件, 消息协议
		/// {
		///	 1 : Message Text Message
		///	 (2 : Message Binary byte[])
		///  }, 响应码 : 301
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnServerMessage(IWebSocketSession session, IList<WebSocketMessage> wsMessages)
		{
			var eventCode = wsMessages[0].MessageEvent.Code;
			var message = wsMessages.ToMessages()[0];
			_logger.LogInformation(eventCode, $"Receive A Message {message.ContentType} From User (ID = {message.SendUserID}) To User(ID = {message.ReceiveID})");
			var serverID = _userService.SelectServerIDByUserID(message.ReceiveID);

			// 统一改成服务器时间
			message.Time = DateTime.Now;
			if (serverID != default)//说明待接收消息的用户在线
			{
				//发送给该用户所连接的服务端
				// wsMessages.SetMessageEvent(WebSocketMessageEvent.OnDataSideMessage);
				// await _wsSession.ServerSendMessage(serverID, wsMessages);

				var sendMessages = new List<Message>() { message };
				var sendWSMessages = sendMessages.ToWebSocketMessageList(WebSocketMessageEvent.OnDataSideMessage);
				await session.ServerSendMessage(serverID, sendWSMessages);
			}
			else _messageService.AppendTemporaryMessage(eventCode, message);

			await _messageService.AppendConstantMessage(eventCode, message);
			return default;
		}

		/// <summary>
		/// 用户端端开事件, 消息协议
		/// {
		///	 
		///	 0 : UserID long
		///	 1 : serviceID int, server ID
		/// }, 响应码 : 100
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public Task<IList<WebSocketMessage>> OnClientDisconnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			var userID = Convert.ToInt64(messages[0].Message);
			var serviceID = Convert.ToInt64(messages[1].Message);
			var eventCode = messages[0].MessageEvent.Code;
			_userService.RemoveOnlineUser(eventCode, userID, serviceID);
			return WebSocketMessageService.DefaultTask;
		}

		/// <summary>
		/// 接受服务端发出的请求历史数据(来自客户端), 消息协议
		/// {
		///	 0 : clientUserID long, 客户端用户
		///	 1 : requestUserID long, 请求用户
		/// }, 响应码 : 501
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		public async Task<IList<WebSocketMessage>> OnServerPreviousMessage(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			var clientUserID = Convert.ToInt64(messages[0].Message);
			var requestUserID = Convert.ToInt64(messages[1].Message);
			var eventCode = messages[0].MessageEvent.Code;
			var previousMessages = await _messageService.SelectPreviousMessagesOfClientUser(eventCode, clientUserID, requestUserID);
			var resultMessages = previousMessages.ToList().ToWebSocketMessageList(WebSocketMessageEvent.OnDataSidePreviousMessage);
			resultMessages.Insert(0, new WebSocketMessage()
			{
				MessageEvent = WebSocketMessageEvent.OnDataSidePreviousMessage,
				MessageType = WebSocketMessageType.Text,
				Message = clientUserID.ToString()
			});
			return resultMessages;
		}
		public async Task<IList<WebSocketMessage>> OnDataSideConnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			_logger.LogInformation(messages[0].MessageEvent.Code, "A Connection Request");
			return await Task.Run(() => messages);
		}

		public async Task<IList<WebSocketMessage>> OnDataSideDisconnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			return await Task.Run(() => messages);
		}

		public Task<IList<WebSocketMessage>> OnDisconnect(IWebSocketSession session, IList<WebSocketMessage> messages)
		{
			var message = messages[0];
			_logger.LogInformation(message.MessageEvent.Code, $"Client {message.Message} Has Disconnected");
			return WebSocketMessageService.DefaultTask;
		}
	}
}