using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using dotnet.Model;
using dotnet.Services;


namespace dotnet.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        private ICookie _cookie;
        private SQL _sql;
        public AuthController(SQL sql,ICookie cookie)
        {
            _sql = sql;
            _cookie = cookie;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpGet]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        [Route("LogInSubmit")]
        public IActionResult LogInSubmit()
        {
            string account = "",password = "";
            StringValues userInfo = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("User[]",out userInfo) == false) return new JsonResult("Post error");

            account = userInfo[0];
            password = userInfo[1];

            IList<User> user = Model.User.GetList(_sql.Query($"select * from users where Account = '{account}'"));


            if(user.Count == 0)
                return new JsonResult("account not found");

            if(user[0].Password != password)
                return new JsonResult("password is error");

            var userClaims = new List<Claim>()
            {
                new Claim("ID",user[0].ID.ToString()),
                new Claim("Account",account)
            };

            var userIdentity = new ClaimsIdentity(userClaims,"user");

            var userIdentities = new List<ClaimsIdentity>()
            {
                userIdentity
            };

            var claimsPrincipal = new ClaimsPrincipal(userIdentities);

            _cookie.SetCookie(HttpContext,claimsPrincipal);

            return new JsonResult("login succeed");
        }


        [HttpPost]
        [Route("LogOutSubmit")]
        public IActionResult LogOutSubmit()
        {
            _cookie.DeleteCookie(HttpContext);
            return new JsonResult("logout succeed");
        }

        [HttpGet]
        [Route("GetCookie")]
        public string GetCookie()
        {
            string result = "";

            ClaimsPrincipal claimsPrincipal =  _cookie.GetCookie(HttpContext);

            foreach (var i in claimsPrincipal.Identities)
            {
                foreach (var c in i.Claims)
                {
                    result += $"{c.Type} : {c.Value}\n";
                }
            }

            return result;
        }


    }
}