
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Kamanri.WebSockets;

namespace Kamanri.Middlewares
{
    public class WebSocketMiddleware
    {
        public RequestDelegate _next;
        public IWebSocketSession _wsSession;
        public IWebSocketMessageService _wsmService;
        public WebSocketMiddleware(RequestDelegate next, IWebSocketMessageService wsmService, IWebSocketSession wsSession)
        {
            _next = next;
            _wsmService = wsmService;
            _wsSession = wsSession;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next.Invoke(httpContext);                  
        }
    }
}