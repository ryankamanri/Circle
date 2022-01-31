using Microsoft.AspNetCore.Http;
using MLServer.Services;
using System.Threading.Tasks;

namespace MLServer.Middlewares
{
	public class MLServiceMiddleware
	{
		public RequestDelegate _next;
		//启动OnMessageServive服务
		public IMLService _mLService;
		public MLServiceMiddleware(RequestDelegate next, IMLService mLService)
		{
			_next = next;
			_mLService = mLService;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			await _next.Invoke(httpContext);
		}
	}
}
