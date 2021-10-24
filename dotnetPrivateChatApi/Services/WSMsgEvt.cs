using Kamanri.WebSockets.Model;

namespace dotnetPrivateChatApi.Services
{
    public class WSMsgEvt : WebSocketMessageEvent
    {
        public WSMsgEvt(int code) : base(code){}

        /// <summary>
        /// Code : 1001
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnSelf = new WebSocketMessageEvent(1001);



    }

}