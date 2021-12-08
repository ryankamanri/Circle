using System.Net.WebSockets;
using Kamanri.WebSockets.Model;
using Kamanri.Extensions;

namespace dotnetPrivateChatApi.Services
{
    public class WSMsgEvt : WebSocketMessageEvent
    {
        public WSMsgEvt(int code) : base(code)
        {
            // new WebSocket().SendMessageAsync()
        }

        /// <summary>
        /// Code : 1001
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnSelf = new WebSocketMessageEvent(1001);

        

    }

}