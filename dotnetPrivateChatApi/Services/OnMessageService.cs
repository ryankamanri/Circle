using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Kamanri.WebSockets;
using Kamanri.WebSockets.Model;
using Kamanri.Extensions;
using dotnetPrivateChatApi.Model;
using dotnetPrivateChatApi.Services.Extensions;
namespace dotnetPrivateChatApi.Services
{
    public class OnMessageService
    {

        private ILogger<OnMessageService> _logger;
        private WebSocketMessageService _wsmService;

        private WebSocketClient _wsClient;

        private long clientID;


        public OnMessageService(WebSocketMessageService wsmService, WebSocketClient wsClient, ILoggerFactory loggerFactory)
        {
            _wsmService = wsmService;
            _wsClient = wsClient;
            _logger = loggerFactory.CreateLogger<OnMessageService>();
            _wsmService.AddEventHandler(WebSocketMessageEvent.OnDataSideConnect, OnDataSideConnect);
            _wsmService.AddEventHandler(WebSocketMessageEvent.OnClientConnect, OnClientConnect);
            _wsmService.AddEventHandler(WSMsgEvt.OnSelf, OnDataSideDisconnect);
        }

        /// <summary>
        /// 接受数据端连接事件, 消息协议
        /// {
        ///    0 : ID int 
        /// }, 响应码 : 102
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public Task<IList<WebSocketMessage>> OnDataSideConnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            var eventCode = messages[0].MessageEvent.Code;
            _logger.LogInformation(eventCode, $"Receive Assigned ID : {messages[0].Message}");
            clientID = Convert.ToInt64(messages[0].Message);

            // 假设过5s后要添加一个用户
            new Task(async () =>
            {
                // for (int i = 0; i < 3; i++)
                // {
                //     System.Threading.Thread.Sleep(3 * 1000);
                    await _wsClient.ClientSendMessage(new List<WebSocketMessage>()
                    {
                        new WebSocketMessage(
                            WebSocketMessageEvent.OnClientConnect,
                            System.Net.WebSockets.WebSocketMessageType.Text,
                            clientID.ToString()
                        ),
                        new WebSocketMessage(
                            WebSocketMessageEvent.OnClientConnect,
                            System.Net.WebSockets.WebSocketMessageType.Text,
                            new User(8, "974481066@qq.com","123456").ToJson()
                        )
                    });
                //     _logger.LogInformation(messages[0].MessageEvent.Code, $"Login A New User");
                // }

                for (int i = 0; i < 3; i++)
                {
                    System.Threading.Thread.Sleep(3 * 1000);
                    await _wsClient.ClientSendMessage(new List<WebSocketMessage>()
                    {
                        // new WebSocketMessage(
                        //     WebSocketMessageEvent.OnServerMessage,
                        //     System.Net.WebSockets.WebSocketMessageType.Text,
                        //     clientID.ToString()
                        // ),
                        // new WebSocketMessage(
                        //     WebSocketMessageEvent.OnServerMessage,
                        //     System.Net.WebSockets.WebSocketMessageType.Text,
                        //     new User(1,"97448","123456").ToJson()
                        // ),
                        new WebSocketMessage(
                            WebSocketMessageEvent.OnServerMessage, 
                            WebSocketMessageType.Text, 
                            new Message(2, 8, false, DateTime.Now,
                                MessageContentType.Text,
                                "Hello, World").ToJson()
                        )

                        // new WebSocketMessage(
                        //     WebSocketMessageEvent.OnServerMessage, 
                        //     WebSocketMessageType.Text, 
                        //     new Message(1, 1, 2, DateTime.Now,
                        //         MessageContentType.Jpg,
                        //         default).ToJson()
                        // ),
                        // new WebSocketMessage(
                        //     WebSocketMessageEvent.OnServerMessage, 
                        //     WebSocketMessageType.Binary, 
                        //     "Hello, World".ToByteArray()
                        // )
                    });
                    _logger.LogInformation(messages[0].MessageEvent.Code, $"Send A New Message");
                }


            }).Start();
            return WebSocketMessageService.DefaultTask;
        }

        public Task<IList<WebSocketMessage>> OnClientConnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            foreach(var msg in messages)
            {
                _logger.LogInformation(msg.MessageEvent.Code, $"Receive OnClientConnent From DataSide : {msg.Message}");
            }
            
            return Task.Run<IList<WebSocketMessage>>(() => new List<WebSocketMessage>()
            {
                new WebSocketMessage(WebSocketMessageEvent.OnServerConnect, 
                WebSocketMessageType.Text, 
                "Hello Client!"),
                new WebSocketMessage(WebSocketMessageEvent.OnServerConnect, 
                WebSocketMessageType.Text, 
                "I Am Server!")
            });
        }


        public Task<IList<WebSocketMessage>> OnDataSideDisconnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            return Task.Run<IList<WebSocketMessage>>(() => messages);
        }
    }
}