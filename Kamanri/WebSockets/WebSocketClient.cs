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

namespace Kamanri.WebSockets
{

    public interface IWebSocketClient
    {
        Task AcceptWebSocketInjection(WebSocket webSocket);
        Task ServerSendMessage(long webSocketID, IList<WebSocketMessage> sendMessages);
        Task ClientSendMessage(IList<WebSocketMessage> sendMessages);
        long GetWebSocketAssignedID(WebSocket webSocket);
        Task ClientClose(long webSocketID, WebSocketCloseStatus closeStatus, string statusDescription);

    }

    public sealed class WebSocketClient : IWebSocketClient
    {

        ILogger<WebSocketClient> _logger;
        ClientWebSocket webSocket = null;

        IWebSocketMessageService _wsmService;

        IDictionary<long, WebSocket> webSocketServerCollection = default;
        Uri uri = null;
        bool isUserClose = false;//是否最后由用户手动关闭


        /// <summary>
        /// client/server端WebSocket构造函数
        /// </summary>
        /// <param name="webSocketUrl"></param>
        /// <param name="wsmService"></param>
        /// <param name="loggerFactory"></param>
        public WebSocketClient(IConfiguration config,IWebSocketMessageService wsmService, ILoggerFactory loggerFactory)
        {
            var webSocketUrl = config["WebSocket:URL"];
            _logger = loggerFactory.CreateLogger<WebSocketClient>();
            _wsmService = wsmService;
            webSocketServerCollection = new Dictionary<long, WebSocket>();
            if(webSocketUrl != default)
            {
                uri = new Uri(webSocketUrl);
                webSocket = new ClientWebSocket();
                Open();
            }
            
        }




        /// <summary>
        /// 打开链接
        /// </summary>
        private void Open()
        {
            Task.Run(async () =>
            {
                if (webSocket.State == WebSocketState.Connecting || webSocket.State == WebSocketState.Open)
                    return;

                string netErr = string.Empty;
                try
                {
                    //初始化链接
                    isUserClose = false;
                    webSocket = new ClientWebSocket();
                    await webSocket.ConnectAsync(uri, CancellationToken.None);

                    await webSocket.SendMessageAsync(new List<WebSocketMessage>()
                    {
                        new WebSocketMessage(WebSocketMessageEvent.OnConnect,
                        WebSocketMessageType.Text,
                        "Hello")
                    });

                    _logger.LogInformation($"[{DateTime.Now}] : Open the WebSocket Connection,Ready To Send Message");


                    while(true) 
                        await webSocket.OnReceiveMessageAsync(async messages => await _wsmService.OnMessage(webSocket, messages));

                }
                catch (Exception e)
                {
                    throw new WebSocketException($"Failed To Open The WebSocket Connection : {uri}", e);
                }
                finally
                {
                    if (!isUserClose)
                        await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription + netErr, CancellationToken.None);
                }
            });

        }

        /// <summary>
        /// 向WebSocket连接池中添加新的连接
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        private long InjectWebSocket(WebSocket webSocket)
        {
            if(webSocketServerCollection == default) webSocketServerCollection = new Dictionary<long, WebSocket>();
            long ID = RandomGenerator.GenerateID();
            this.webSocketServerCollection.Add(ID, webSocket);
            _logger.LogInformation($"[{DateTime.Now}] : WebSocket Client {{ id = {ID} }} Has Connected, WebSocket Collection Count : {webSocketServerCollection.Count}");
            return ID;
        }

        private void DisposeWebSocket(WebSocket webSocket)
        {
            
            var managedID_webSocket = (from id_socket in webSocketServerCollection
                where id_socket.Value == webSocket
                select id_socket).FirstOrDefault();
            webSocketServerCollection.Remove(managedID_webSocket);
            webSocket.Dispose();
            _logger.LogInformation($"[{DateTime.Now}] : WebSocket Client {{ id = {managedID_webSocket.Key } }} Has Disposed, WebSocket Collection Count : {webSocketServerCollection.Count}");
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
            long ID = InjectWebSocket(webSocket);
            await webSocket.OnReceiveMessageAsync(messages => 
            {
                var firstMessage = messages.FirstOrDefault();
                if(firstMessage == default || firstMessage.MessageEvent.Code != WebSocketMessageEvent.OnConnect.Code)
                    return _wsmService.OnMessage(webSocket, messages);
                messages.Insert(0, new WebSocketMessage(
                    WebSocketMessageEvent.OnConnect, 
                    WebSocketMessageType.Text, 
                    ID.ToString()
                ));
                return _wsmService.OnMessage(webSocket, messages);
            });
            while(webSocket.State == WebSocketState.Open) 
                await webSocket.OnReceiveMessageAsync(messages => _wsmService.OnMessage(webSocket, messages));
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
            if(!webSocketServerCollection.TryGetValue(webSocketID, out clientWebSocket))
                _logger.LogError($"[{DateTime.Now}] : Cannot Find Client WebSocket By WebSocket ID {webSocketID}, Execute Default Task");
            else
            {
                _logger.LogInformation($"[{DateTime.Now}] : Send To Client {webSocketID}");
                await clientWebSocket.SendMessageAsync(sendMessages);
            }
        }

        public long GetWebSocketAssignedID(WebSocket webSocket)
        {
            return (from ID_WebSocket in webSocketServerCollection
                where ID_WebSocket.Value == webSocket
                select ID_WebSocket.Key).FirstOrDefault();
        }

        public WebSocket GetWebSocket(long assignedID)
        {
            WebSocket websocket = default;
            if(!webSocketServerCollection.TryGetValue(assignedID, out websocket))
                _logger.LogWarning($"Cannot Get WebSocket By Assigned ID {assignedID}");
            return websocket;
        }


        public async Task ClientClose(long webSocketID, WebSocketCloseStatus closeStatus, string statusDescription)
        {
            WebSocket clientWebSocket = default;
            if (!webSocketServerCollection.TryGetValue(webSocketID, out clientWebSocket))
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