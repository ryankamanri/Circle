using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Kamanri.Http;
using Microsoft.AspNetCore.Http;
using WebViewServer.Models.User;
using WebViewServer.Services.Cookie;

namespace WebViewServer.Middlewares
{
    public class UserMiddleware
    {
        private Api _api;

        private ICookie _cookie;

        private RequestDelegate _next;

        public UserMiddleware(Api api, ICookie cookie, RequestDelegate next)
        {
            _api = api;
            _cookie = cookie;
            _next = next;
        }
        public async Task Invoke(HttpContext context, User user, Dictionary<string, string> dict)
        {

            try
            {
                ClaimsPrincipal claimsPrincipal = context.User;
                foreach (var c in claimsPrincipal.Claims)
                {

                    if (c.Type.ToString() == "ID")
                        user.ID = Convert.ToInt64(c.Value);
                    if (c.Type.ToString() == "Account")
                        user.Account = c.Value;

                }
                dict["Circle"] = _cookie.GetCookie(context, "Circle");
                await _next.Invoke(context);
            }
            catch (HttpRequestException)
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("错误 : 与后端Api连接断开");
            }
        }
    }
}