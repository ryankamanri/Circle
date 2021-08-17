using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dotnetApi.Services.Self;

namespace dotnetApi.Services.Cookie
{

    public interface ICookie
    {
        void SignIn(HttpContext httpContext,ClaimsPrincipal claimsPrincipal);

        void SignOut(HttpContext httpContext);

        ClaimsPrincipal GetAuth(HttpContext httpContext);

        void AppendCookie(HttpContext httpContext,string key,string value);

        void DeleteCookie(HttpContext httpContext,string key);

        string GetCookie(HttpContext httpContext,string key);
    }
    
    public class Cookie : ICookie
    {


        private string cookieName { get; } = "SelfLogIn";


        public void SignIn(HttpContext httpContext,ClaimsPrincipal claimsPrincipal)
        {
            AuthenticationScheme scheme = new AuthenticationScheme("name", "displayName", typeof(Scheme));
            var ticket = new AuthenticationTicket(claimsPrincipal, scheme.Name);
            httpContext.Response.Cookies.Append(cookieName, Base64.EncodeBase64(Scheme.Serialize(ticket)));

            httpContext.SignInAsync(claimsPrincipal);
        }
        public void SignOut(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete(cookieName);
            httpContext.SignOutAsync();
        }

        public ClaimsPrincipal GetAuth(HttpContext httpContext)
        {
            AuthenticationTicket ticket;
            string content = httpContext.Request.Cookies[cookieName];
            if(content == default || content == "") return new ClaimsPrincipal();
            ticket = Scheme.Deserialize(Base64.DecodeBase64(content));
            return ticket.Principal;
        }

        

        public void AppendCookie(HttpContext httpContext,string key,string value)
        {
            httpContext.Response.Cookies.Append(Base64.EncodeBase64(key), Base64.EncodeBase64(value));
        }

        public void DeleteCookie(HttpContext httpContext,string key)
        {
            httpContext.Response.Cookies.Delete(Base64.EncodeBase64(key));
        }

        public string GetCookie(HttpContext httpContext,string key)
        {
            string unDecodedCookie =  httpContext.Request.Cookies[Base64.EncodeBase64(key)];
            if(unDecodedCookie == default) return default;
             return Base64.DecodeBase64(unDecodedCookie);
        }
    }
}
