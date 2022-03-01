using Kamanri.Utils;
using Microsoft.AspNetCore.Http;

namespace WebViewServer.Services.Cookie
{

	public interface ICookie
	{
		void AppendCookie(HttpContext httpContext, string key, string value);

		void DeleteCookie(HttpContext httpContext, string key);

		string GetCookie(HttpContext httpContext, string key);
	}

	public class Cookie : ICookie
	{

		public void AppendCookie(HttpContext httpContext, string key, string value)
		{
			httpContext.Response.Cookies.Append(Base64.EncodeBase64(key), Base64.EncodeBase64(value));
		}

		public void DeleteCookie(HttpContext httpContext, string key)
		{
			httpContext.Response.Cookies.Delete(Base64.EncodeBase64(key));
		}

		public string GetCookie(HttpContext httpContext, string key)
		{
			string unDecodedCookie = httpContext.Request.Cookies[Base64.EncodeBase64(key)];
			if (unDecodedCookie == default) return default;
			return Base64.DecodeBase64(unDecodedCookie);
		}
	}
}
