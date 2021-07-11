using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using dotnet.Model;
using dotnet.Services.Database;
using dotnet.Services.Cookie;
namespace dotnet.Middlewares
{
    public class GetUserInfoMiddleware
    {

        private DataBaseContext _dbc;

        private ICookie _cookie;

        private RequestDelegate _next;

        public GetUserInfoMiddleware(DataBaseContext dbc,ICookie cookie,RequestDelegate next)
        {
            _dbc = dbc;
            _cookie = cookie;
            _next = next;
        }
        public async Task Invoke(HttpContext context,User user)
        {
            ClaimsPrincipal claimsPrincipal = _cookie.GetCookie(context);
            foreach(var c in claimsPrincipal.Claims)
            {
                if(c.Type.ToString() == "ID")
                {
                    User selectedUser = new Model.User(Convert.ToInt64(c.Value)); 
                    selectedUser = _dbc.Select<User>(selectedUser);
                    if(selectedUser == null) break;
                    user.ID = selectedUser.ID;
                    user.Account = selectedUser.Account;
                    user.Password = selectedUser.Password;
                    break;
                }
            }
            await _next.Invoke(context);
        }
    }
}