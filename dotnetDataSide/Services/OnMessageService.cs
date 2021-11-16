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
using dotnetDataSide.Models;
using dotnetDataSide.Services.Extensions;

namespace dotnetDataSide.Services
{
    public class OnMessageService
    {

        private ILogger<OnMessageService> _logger;
        private IWebSocketMessageService _wsmService;

        private UserService _userService;

        private MessageService _messageService;

        private IWebSocketClient _wsClient;



        public OnMessageService(IWebSocketMessageService wsmService, IWebSocketClient wsClient, ILoggerFactory loggerFactory, DataBaseContext dbc)
        {
            _wsmService = wsmService;
            _wsClient = wsClient;
            _logger = loggerFactory.CreateLogger<OnMessageService>();
            _userService = new UserService(loggerFactory);
            _messageService = new MessageService(dbc, loggerFactory);

            _wsmService.AddEventHandler(WebSocketMessageEvent.OnConnect,OnConnect);
            _wsmService.AddEventHandler(WebSocketMessageEvent.OnClientConnect, OnClientConnect);
            _wsmService.AddEventHandler(WebSocketMessageEvent.OnServerConnect, OnServerConnect);
            _wsmService.AddEventHandler(WebSocketMessageEvent.OnServerMessage, OnServerMessage);
            _wsmService.AddEventHandler(WebSocketMessageEvent.OnDataSideConnect, OnDataSideConnect);
            _wsmService.AddEventHandler(WebSocketMessageEvent.OnDataSideDisconnect, OnDataSideDisconnect);
            
        }

        /// <summary>
        /// 接受连接事件, 消息协议
        /// {
        ///      0 : ID int
        /// }, 响应码 : 1
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public Task<IList<WebSocketMessage>> OnConnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            return Task.Run<IList<WebSocketMessage>>(() =>
            {
                var ID = Convert.ToInt64(messages[0].Message);
                _logger.LogInformation(messages[0].MessageEvent.Code, $"A Connect Request, Assigned ID : {ID}");

                
                return new List<WebSocketMessage>()
                {
                    new WebSocketMessage(WebSocketMessageEvent.OnDataSideConnect,
                        System.Net.WebSockets.WebSocketMessageType.Text,
                        messages[0].Message)
                };
            });
            
        }


        
        /// <summary>
        /// 添加用户端事件, 消息协议
        /// {
        ///     0 : ID int, 
        ///     1 : User User   
        /// }, 响应码 : 100
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public async Task<IList<WebSocketMessage>> OnClientConnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {

            var ID = Convert.ToInt64(messages[0].Message);
            var user = messages[1].Message.ToString().ToObject<User>();
            var eventCode = messages[0].MessageEvent.Code;
            _logger.LogInformation(eventCode, $"A User {user.ToString()} Connection Request, Client ID = {ID}");
            _userService.AppendOnlineUser(eventCode, user, ID);


            var tempMessages = _messageService.SelectTempMessagesOfAUser(eventCode, user);
            var result = new List<WebSocketMessage>();
            foreach(var tempMessage in tempMessages)
            {
                result.AddRange(tempMessage.ToWebSocketMessageList(WebSocketMessageEvent.OnClientConnect));
            }

            return await Task.Run<IList<WebSocketMessage>>(() => result);
            
        }

        public Task<IList<WebSocketMessage>> OnServerConnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            return WebSocketMessageService.DefaultTask;
        }

        /// <summary>
        /// 接受服务端的消息事件, 消息协议
        /// {
        ///     1 : Message Text Message
        ///     (2 : Message Binary byte[])
        ///  }, 响应码 : 301
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public async Task<IList<WebSocketMessage>> OnServerMessage(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            var eventCode = messages[0].MessageEvent.Code;
            // var count = messages.Count;
            // var messageInWSMsgs = messages.Take(count == 1 ? 1 : 2).ToList();
            var message = messages.ToMessage();
            _logger.LogInformation(eventCode, $"Receive A Message {message.ContentType} From User (ID = {message.SendUserID}) To User(ID = {message.ReceiveID})");

            var serverID = _userService.SelectServerIDByUserID(message.ReceiveID);
            if(serverID != default)//说明待接收消息的用户在线
            {
                //发送给该用户所连接的客户端
                messages.SetMessageEvent(WebSocketMessageEvent.OnClientMessage);
                await _wsClient.ServerSendMessage(serverID, messages);
            }
            await _messageService.AppendMessage(eventCode, message);
            return default;
        }

        
        public async Task<IList<WebSocketMessage>> OnDataSideConnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            _logger.LogInformation(messages[0].MessageEvent.Code, "A Connection Request");
            return await Task.Run<IList<WebSocketMessage>>(() => messages);
        }

        public async Task<IList<WebSocketMessage>> OnDataSideDisconnect(WebSocket webSocket, IList<WebSocketMessage> messages)
        {
            return await Task.Run<IList<WebSocketMessage>>(() => messages);
        }
    }
}