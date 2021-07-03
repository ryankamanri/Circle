﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dotnet.Services.Extensions;

namespace dotnet.Services.Cookie
{
    public class Cookie : ICookie
    {


        private const string _cookieName  = "SelfLogIn";

        public void DeleteCookie(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete(_cookieName);
            httpContext.SignOutAsync();
        }

        public ClaimsPrincipal GetCookie(HttpContext httpContext)
        {
            string content = httpContext.Request.Cookies[_cookieName];
            AuthenticationTicket ticket = Scheme.Deserialize(Base64.DecodeBase64(content));
            return ticket.Principal;
        }

        public void SetCookie(HttpContext httpContext,ClaimsPrincipal claimsPrincipal)
        {
            AuthenticationScheme scheme = new AuthenticationScheme("name", "displayName", typeof(Scheme));
            var ticket = new AuthenticationTicket(claimsPrincipal, scheme.Name);
            httpContext.Response.Cookies.Append(_cookieName, Base64.EncodeBase64(Scheme.Serialize(ticket)));

            httpContext.SignInAsync(claimsPrincipal);
        }
    }
}
