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

                    // if (OnOpen != null)
                    //     OnOpen(webSocket, new EventArgs());

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
                        Close(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription + netErr);
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
            return ID;
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
            while(true) 
                await webSocket.OnReceiveMessageAsync(messages => _wsmService.OnMessage(webSocket, messages));

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
            await webSocketServerCollection[webSocketID].SendMessageAsync(sendMessages);
        }




        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            isUserClose = true;
            Close(WebSocketCloseStatus.NormalClosure, "用户手动关闭");
        }

        public void Close(WebSocketCloseStatus closeStatus, string statusDescription)
        {
            Task.Run(async () =>
            {
                try
                {
                    //关闭WebSocket（客户端发起）
                    await webSocket.CloseAsync(closeStatus, statusDescription, CancellationToken.None);
                }
                catch (Exception e)
                {
                    throw e;
                }

                webSocket.Abort();
                webSocket.Dispose();


            });
        }

    }

}