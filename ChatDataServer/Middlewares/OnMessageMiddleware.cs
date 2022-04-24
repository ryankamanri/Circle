using ChatDataServer.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ChatDataServer.Middlewares
{
	public class OnMessageMiddleware
	{
		public RequestDelegate _next;
		//启动OnMessageServive服务
		public OnMessageController OnMessageController { get; set; }
		public OnMessageMiddleware(RequestDelegate next, OnMessageController onMsController)
		{
			_next = next;
			OnMessageController = onMsController;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			await _next.Invoke(httpContext);
		}
	}
}
