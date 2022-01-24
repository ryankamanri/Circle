using System;
using System.Net.WebSockets;
using System.Collections.Generic;
using Kamanri.WebSockets.Model;
using Kamanri.Extensions;
using ChatServer.Models;

namespace ChatServer.Services.Extensions
{
    public static class MessageExtension
    {
        public static IList<WebSocketMessage> ToWebSocketMessageList(this IList<Message> messages, WebSocketMessageEvent wsmEvent)
        {
            var msgList = new List<WebSocketMessage>();
            foreach (var message in messages)
            {
                if (message.ContentType == MessageContentType.Text)
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
            }

            return msgList;
        }

        public static IList<Message> ToMessages(this IList<WebSocketMessage> wsMessages, int offset = 0, int length = -1)
        {
            var result = new List<Message>();
            var _offset = offset;
            // length = -1 ===>  _offset < wsMessages.Count
            // length != -1 ===> _offset < wsMessages.Count && _offset - offset < length
            while (_offset < wsMessages.Count && (_offset - offset < length || length == -1))
            {
                var message = (wsMessages[_offset].Message as string).ToObject<Message>();
                if (message.ContentType != MessageContentType.Text)
                {
                    var bytesContent = wsMessages[_offset + 1].Message as byte[];
                    message.Content = bytesContent;
                    result.Add(message);
                    _offset += 2;
                }
                else
                {
                    result.Add(message);
                    _offset++;
                }

            }

            return result;

        }
    }
}