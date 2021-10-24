using System.Collections.Generic;
using Kamanri.WebSockets.Model;
namespace Kamanri.Extensions
{
    public static class WebSocketMessageListExtension
    {
        public static void SetMessageEvent(this IList<WebSocketMessage> webSocketMessages, WebSocketMessageEvent wsmEvent)
        {
            foreach(var webSocketMessage in webSocketMessages)
            {
                webSocketMessage.MessageEvent = wsmEvent;
            }
        }
    }
}