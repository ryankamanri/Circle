using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dotnet.Services.Extensions;

namespace dotnet.Services.Cookie
{

    public interface ICookie
    {
        void SetCookie(HttpContext httpContext,ClaimsPrincipal claimsPrincipal);

        void DeleteCookie(HttpContext httpContext);

        ClaimsPrincipal GetCookie(HttpContext httpContext);
    }
    
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
            AuthenticationTicket ticket;
            string content = httpContext.Request.Cookies[_cookieName];
            if(content == default || content == "") return new ClaimsPrincipal();
            ticket = Scheme.Deserialize(Base64.DecodeBase64(content));
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
