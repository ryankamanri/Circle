using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Kamanri.Extensions;
using Kamanri.Self;
using Kamanri.WebSockets.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;

namespace Kamanri.WebSockets
{

	public interface IWebSocketSession
	{
		Task AcceptWebSocketInjection(WebSocket webSocket);
		Task ServerSendMessage(long webSocketID, IList<WebSocketMessage> sendMessages);
		Task ClientSendMessage(IList<WebSocketMessage> sendMessages);
		Task ClientClose(long webSocketID, WebSocketCloseStatus closeStatus, string statusDescription);

	}

	public sealed class WebSocketSession : IWebSocketSession
	{
		private readonly ILogger<WebSocketSession> _logger;
		private ClientWebSocket webSocket = null;

		private readonly WebSocketMessageService _wsmService;

		private IDictionary<long, WebSocket> webSocketServerCollection = default;
		private readonly Uri uri = null;


		/// <summary>
		/// client/server端WebSocket构造函数
		/// </summary>
		/// <param name="webSocketUrl"></param>
		/// <param name="wsmService"></param>
		/// <param name="loggerFactory"></param>
		public WebSocketSession(IConfiguration config, IWebSocketMessageService wsmService, ILoggerFactory loggerFactory)
		{
			var webSocketUrl = config["WebSocket:URL"];
			_logger = loggerFactory.CreateLogger<WebSocketSession>();
			_wsmService = wsmService as WebSocketMessageService;
			webSocketServerCollection = new Dictionary<long, WebSocket>();
			if (webSocketUrl != default)
			{
				uri = new Uri(webSocketUrl);
				BuildWebSocketSession();
			}

		}




        /// <summary>
        /// 打开与服务器的网络套接字会话
        /// </summary>
        private void BuildWebSocketSession()
        {
            try
            {
                Task.Run(async () =>
				{
					webSocket = new ClientWebSocket();

					while (!await TryConnectAsync(webSocket, uri, CancellationToken.None))
					{
						_logger.LogWarning($"[{DateTime.Now}] : Cannot Establish The Connection, Retry 3 Seconds Later...");
						webSocket = new ClientWebSocket();
						Thread.Sleep(3000);
					}

					await webSocket.SendMessageAsync(new List<WebSocketMessage>()
						{
							new WebSocketMessage(WebSocketMessageEvent.OnConnect,
							WebSocketMessageType.Text,
							"Hello")
						});

					_logger.LogInformation($"[{DateTime.Now}] : Open the WebSocket Connection,Ready To Send Message");

					try
					{
						while (true)
							await webSocket.OnReceiveMessageAsync(async messages => await _wsmService.OnMessage(this, messages));

					}
					catch (Exception e)
					{
						_logger.LogError(e, e.Message);
						_logger.LogWarning($"[{DateTime.Now}] : The WebSocket Session Encounter The Exception, Start To Open A New Session");
						await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription + e, CancellationToken.None);
						BuildWebSocketSession();
					}

				});
            }
            catch (Exception e)
            {
                throw new WebSocketException($"Failed To Open The WebSocket Connection : {uri}", e);
            }


        }

		private async Task<bool> TryConnectAsync(ClientWebSocket webSocket, Uri uri, CancellationToken cancellationToken)
		{
			try
			{
				await webSocket.ConnectAsync(uri, CancellationToken.None);
				return true;
			}catch (System.Net.WebSockets.WebSocketException)
			{
				return false;
			}
			
		}

		/// <summary>
		/// 向WebSocket连接池中添加新的连接
		/// </summary>
		/// <param name="webSocket"></param>
		/// <returns></returns>
		private long InjectWebSocket(WebSocket webSocket)
		{
			if (webSocketServerCollection == default) webSocketServerCollection = new Dictionary<long, WebSocket>();
			long ID = RandomGenerator.GenerateID();
			this.webSocketServerCollection.Add(ID, webSocket);
			_logger.LogDebug($"[{DateTime.Now}] : WebSocket Client {{ id = {ID} }} Has Connected, WebSocket Collection Count : {webSocketServerCollection.Count}");
			return ID;
		}

		private void DisposeWebSocket(WebSocket webSocket)
		{

			var managedID_webSocket = (from id_socket in webSocketServerCollection
									   where id_socket.Value == webSocket
									   select id_socket).FirstOrDefault();
			webSocketServerCollection.Remove(managedID_webSocket);
			webSocket.Dispose();
			_logger.LogDebug($"[{DateTime.Now}] : WebSocket Client {{ id = {managedID_webSocket.Key } }} Has Disposed, WebSocket Collection Count : {webSocketServerCollection.Count}");
		}

		/// <summary>
		/// Server端接受WebSocket连接注入
		/// </summary>
		/// <param name="webSocket"></param>
		/// <param name="SetWebSocketID"></param>
		/// <returns></returns>
		public async Task AcceptWebSocketInjection(WebSocket webSocket)
		{
			//注册WebSocket
			long ID = InjectWebSocket(webSocket); // 该WebSocket唯一ID
			await webSocket.OnReceiveMessageAsync(messages =>
			{
				var firstMessage = messages.FirstOrDefault();
				if (firstMessage == default || firstMessage.MessageEvent.Code != WebSocketMessageEvent.OnConnect.Code)
					return _wsmService.OnMessage(this, messages);
				messages.Insert(0, new WebSocketMessage(
					WebSocketMessageEvent.OnConnect,
					WebSocketMessageType.Text,
					ID.ToString()
				));
				return _wsmService.OnMessage(this, messages);
			});
			while (webSocket.State == WebSocketState.Open)
				await webSocket.OnReceiveMessageAsync(messages =>
				{
					var firstMessage = messages.FirstOrDefault();
					if (firstMessage.MessageEvent.Code == WebSocketMessageEvent.OnDisconnect.Code)
						messages.Insert(0, new WebSocketMessage(
							WebSocketMessageEvent.OnDisconnect,
							WebSocketMessageType.Text,
							ID.ToString()
						));
					return _wsmService.OnMessage(this, messages);
				});
			// webSocket遭到中断, 关闭连接 (status 'Aborted')

			DisposeWebSocket(webSocket);

		}

		/// <summary>
		/// 作为客户端向服务端发送一次WebSocket请求
		/// </summary>
		/// <param name="sendMessages"></param>
		/// <returns></returns>
		public async Task ClientSendMessage(IList<WebSocketMessage> sendMessages)
		{
			await webSocket.SendMessageAsync(sendMessages);
		}


		/// <summary>
		/// 作为服务端向某一特定客户端发送一次WebSocket请求
		/// </summary>
		/// <param name="webSocketID"></param>
		/// <param name="sendMessages"></param>
		/// <returns></returns>
		public async Task ServerSendMessage(long webSocketID, IList<WebSocketMessage> sendMessages)
		{
			WebSocket clientWebSocket = default;
			if (!webSocketServerCollection.TryGetValue(webSocketID, out clientWebSocket))
				_logger.LogError($"[{DateTime.Now}] : Cannot Find Client WebSocket By WebSocket ID {webSocketID}, Execute Default Task");
			else
			{
				_logger.LogDebug($"[{DateTime.Now}] : Send To Client {webSocketID}");
				await clientWebSocket.SendMessageAsync(sendMessages);
			}
		}



		public async Task ClientClose(long webSocketID, WebSocketCloseStatus closeStatus, string statusDescription)
		{
			if (!webSocketServerCollection.TryGetValue(webSocketID, out WebSocket clientWebSocket))
			{
				_logger.LogError($"[{DateTime.Now}] : Cannot Find Client WebSocket By WebSocket ID {webSocketID}");
				return;
			}
			try
			{
				await clientWebSocket.CloseAsync(closeStatus, statusDescription, CancellationToken.None);
				clientWebSocket.Dispose();
			}
			catch (Exception e)
			{
				webSocket.Abort();
				webSocket.Dispose();
				_logger.LogError(e, $"Closed WebSocket {webSocketID} Abnormally");
			}
		}
	}

}