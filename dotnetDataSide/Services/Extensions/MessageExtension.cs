using System;
using System.Net.WebSockets;
using System.Collections.Generic;
using Kamanri.WebSockets.Model;
using Kamanri.Extensions;
using dotnetDataSide.Models;

namespace dotnetDataSide.Services.Extensions
{
    public static class MessageExtension
    {
        public static IList<WebSocketMessage> ToWebSocketMessageList(this Message message, WebSocketMessageEvent wsmEvent)
        {
            var msgList = new List<WebSocketMessage>();
            if(message.ContentType == MessageContentType.Text)
            {
                msgList.Add(new WebSocketMessage(
                    wsmEvent, 
                    WebSocketMessageType.Text, 
                    message.ToJson()
                ));
            }
            else
            {
                var bytes = message.Content;
                message.Content = default;
                msgList.Add(new WebSocketMessage(
                    wsmEvent,
                    WebSocketMessageType.Text,
                    message.ToJson()
                ));
                msgList.Add(new WebSocketMessage(
                    wsmEvent, 
                    WebSocketMessageType.Binary, 
                    bytes
                ));
                message.Content = bytes;
            }
            return msgList;
        }

        public static Message ToMessage(this IList<WebSocketMessage> wsMessages)
        {
            var message = (wsMessages[0].Message as string).ToObject<Message>();
            if(wsMessages.Count == 1) return message;
            var bytesContent = (wsMessages[1].Message as byte[]);
            message.Content = bytesContent;
            return message;
        }
        
    }
}