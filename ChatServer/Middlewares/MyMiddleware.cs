using ChatServer.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ChatServer.Middlewares
{
    public class MyMiddleware
    {
        public RequestDelegate _next;
        //启动OnMessageServive服务
        public OnMessageService OnMessageService { get; set; }
        public MyMiddleware(RequestDelegate next, OnMessageService onMsService)
        {
            _next = next;
            OnMessageService = onMsService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next.Invoke(httpContext);
        }
    }
}
