using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using dotnet.Services;
using dotnet.Model;
namespace dotnet.Controllers
{
    [Controller]
    public class ViewsController : Controller
    {

        private ICookie _cookie;
        private SQL _sql;
        public ViewsController(SQL sql,ICookie cookie)
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
        [Route("Home")]
        [Authorize]
        public IActionResult Home()
        {
            return View("Home");
        }

        [HttpGet]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            return View("LogIn");
        }

        [HttpGet]
        [Route("LogInSubmit")]
        public IActionResult LogInSubmit()
        {

            string account = "974481066@qq.com";

            string password = "123456";

            IList<User> user = Model.User.GetList(_sql.Query($"select * from schema1.users where Account = '{account}'"));

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

            return RedirectToAction("Home");
        }


        [HttpGet]
        [Route("LogOutSubmit")]
        public IActionResult LogOutSubmit()
        {
            _cookie.DeleteCookie(HttpContext);
            return View("Index");
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