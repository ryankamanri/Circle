using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using Kamanri.WebSockets.Model;

namespace Kamanri.WebSockets
{
    

    public interface IWebSocketMessageService
    {
        IWebSocketMessageService AddEventHandler(WebSocketMessageEvent messageEvent, Func<WebSocket, IList<WebSocketMessage>, Task<IList<WebSocketMessage>>> EventHandler);
        Task<IList<WebSocketMessage>> OnMessage(WebSocket webSocket, IList<WebSocketMessage> messages);
    }


    public sealed class WebSocketMessageService : IWebSocketMessageService
    {
        // Return A Task Without Do Anything.
        public static Task<IList<WebSocketMessage>> DefaultTask = Task.Run<IList<WebSocketMessage>>(() => new List<WebSocketMessage>());

        private ILogger<WebSocketMessageService> _logger;
        private Dictionary<int, Func<WebSocket, IList<WebSocketMessage>, Task<IList<WebSocketMessage>>>> eventHandlerCollection;

        public WebSocketMessageService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WebSocketMessageService>();
            eventHandlerCollection = new Dictionary<int, Func<WebSocket, IList<WebSocketMessage>, Task<IList<WebSocketMessage>>>>();
        }

        /// <summary>
        /// 向WebSocket消息添加处理事件
        /// </summary>
        /// <param name="messageEvent">待处理事件</param>
        /// <param name="EventHandler">处理方法</param>
        public IWebSocketMessageService AddEventHandler(WebSocketMessageEvent messageEvent, Func<WebSocket, IList<WebSocketMessage>, Task<IList<WebSocketMessage>>> EventHandler)
        {
            Func<WebSocket, IList<WebSocketMessage>, Task<IList<WebSocketMessage>>> e;
            if(eventHandlerCollection.TryGetValue(messageEvent.Code, out e))
            {
                _logger.LogError($"[{DateTime.Now}] : Add Event {messageEvent.Code} Failure : A Event Has Existed Or Have The Same Code With Your Event");
                return this;
            }
            eventHandlerCollection.Add(messageEvent.Code, EventHandler);
            return this;
        }



        public async Task<IList<WebSocketMessage>> OnMessage(WebSocket webSocket, IList<WebSocketMessage> messages)
        {

            var firstMessage = messages.FirstOrDefault();
            if (firstMessage == default) return default;
            _logger.LogInformation($"[{DateTime.Now}] : Received Message Event Code : {firstMessage.MessageEvent.Code}, Try To Find And Execute The Corresponding Handler");
            Func<WebSocket, IList<WebSocketMessage>, Task<IList<WebSocketMessage>>> EventHandler = default;
            if (!eventHandlerCollection.TryGetValue(firstMessage.MessageEvent.Code, out EventHandler))
            {
                _logger.LogError($"[{DateTime.Now}] : The Corresponding Handler Of Event {{ Code = {firstMessage.MessageEvent.Code} }} Is Not Found, It Will Cause Exception");
                throw new WebSocketException($"The Corresponding Handler Of Event {{ Code = {firstMessage.MessageEvent.Code} }} Is Not Found");
            }
            return await EventHandler(webSocket, messages);
            
        }
    }
}