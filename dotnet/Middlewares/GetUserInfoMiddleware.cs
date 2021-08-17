using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using dotnet.Model;
using dotnet.Services;
using dotnet.Services.Database;
using dotnet.Services.Cookie;
namespace dotnet.Middlewares
{
    public class GetUserInfoMiddleware
    {

        private UserService _userService;

        private ICookie _cookie;

        private RequestDelegate _next;

        public GetUserInfoMiddleware(UserService userService,ICookie cookie,RequestDelegate next)
        {
            _userService = userService;
            _cookie = cookie;
            _next = next;
        }
        public async Task Invoke(HttpContext context,User user,Dictionary<string,string> dict)
        {
            ClaimsPrincipal claimsPrincipal = context.User;
            foreach(var c in claimsPrincipal.Claims)
            {

                if(c.Type.ToString() == "ID")
                {
                    var selectedUser = from userItem in _userService.Users
                    where userItem.ID == Convert.ToInt64(c.Value)
                    select userItem;
                    if(selectedUser == null) break;
                    var userEnumerator = selectedUser.GetEnumerator();
                    userEnumerator.MoveNext();
                    user.ID = userEnumerator.Current.ID;
                    user.Account = userEnumerator.Current.Account;
                    user.Password = userEnumerator.Current.Password;
                    break;
                    
                }
            }
            dict["Circle"] =  _cookie.GetCookie(context,"Circle");
            await _next.Invoke(context);
        }
    }
}