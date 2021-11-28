using System;
using System.Collections.Generic;
namespace Kamanri.WebSockets.Model
{
    /// <summary>
    /// WebSocket请求响应事件. 
    /// 若需要增加自定义响应事件, 请 
    /// 1. 派生该类. 
    /// 2. 向该类构造函数传入事件响应码参数(int). 
    /// 3. 以 'public static WebSocketMessageEvent {YourEventName} = new WebSocketMessageEvent({code})' 格式添加你的自定义事件, 建议自定义事件响应码 >= 1000 
    /// </summary>
    public class WebSocketMessageEvent : IEquatable<WebSocketMessageEvent>, IEqualityComparer<WebSocketMessageEvent>
    {
        public int Code {get; }
        public WebSocketMessageEvent(int code) => Code = code;

        public bool Equals(WebSocketMessageEvent obj) => Code == obj.Code;

        public bool Equals(WebSocketMessageEvent obj1, WebSocketMessageEvent obj2) => obj1.Code == obj2.Code;

        public int GetHashCode(WebSocketMessageEvent obj) => obj.Code;



        /// <summary>
        /// Code : 1
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnConnect = new WebSocketMessageEvent(1);

        /// <summary>
        /// Code : 2 
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnDisconnect = new WebSocketMessageEvent(2);
        /// <summary>
        /// Code : 100
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnClientConnect = new WebSocketMessageEvent(100);
        /// <summary>
        /// Code : 101
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnServerConnect = new WebSocketMessageEvent(101);
        /// <summary>
        /// Code : 102
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnDataSideConnect = new WebSocketMessageEvent(102);

        /// <summary>
        /// Code : 200
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnClientDisconnect = new WebSocketMessageEvent(200);
        /// <summary>
        /// Code : 201
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnServerDisconnect = new WebSocketMessageEvent(201);
        /// <summary>
        /// Code : 202
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnDataSideDisconnect = new WebSocketMessageEvent(202);

        /// <summary>
        /// Code : 300
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnClientMessage = new WebSocketMessageEvent(300);
        /// <summary>
        /// Code : 301
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnServerMessage = new WebSocketMessageEvent(301);
        /// <summary>
        /// Code : 302
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnDataSideMessage = new WebSocketMessageEvent(302);


        /// <summary>
        /// Code : 400
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnClientTempMessage = new WebSocketMessageEvent(400);
        /// <summary>
        /// Code : 401
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnServerTempMessage = new WebSocketMessageEvent(401);
        /// <summary>
        /// Code : 402
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnDataSideTempMessage = new WebSocketMessageEvent(402);

        /// <summary>
        /// Code : 500
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnClientPreviousMessage = new WebSocketMessageEvent(500);

        /// <summary>
        /// Code : 501
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnServerPreviousMessage = new WebSocketMessageEvent(501);
        /// <summary>
        /// Code : 502
        /// </summary>
        /// <returns></returns>
        public static WebSocketMessageEvent OnDataSidePreviousMessage = new WebSocketMessageEvent(502);
    }
}