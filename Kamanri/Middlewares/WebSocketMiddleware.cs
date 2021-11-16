
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Kamanri.WebSockets;

namespace Kamanri.Middlewares
{
    public class WebSocketMiddleware
    {
        public RequestDelegate _next;
        public IWebSocketClient _wsClient;
        public IWebSocketMessageService _wsmService;
        public WebSocketMiddleware(RequestDelegate next, IWebSocketMessageService wsmService, IWebSocketClient wsClient)
        {
            _next = next;
            _wsmService = wsmService;
            _wsClient = wsClient;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next.Invoke(httpContext);                  
        }
    }
}