using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using dotnet.Model;
using dotnet.Services.Database;
using dotnet.Services.Cookie;

namespace dotnet.Controllers
{
    [Controller]
    public class HomeController : Controller
    {

        private ICookie _cookie;
        private SQL _sql;

        private User _user;
        public HomeController(SQL sql,ICookie cookie)
        {
            _sql = sql;
            _cookie = cookie;
            _user = new User();
        }

        [HttpGet]
        [Route("Home")]
        [Authorize]
        public IActionResult Home()
        {
            foreach(var c in _cookie.GetCookie(HttpContext).Claims)
            {
                if(c.Type.ToString() == "ID")
                    return View(_user.Select(_sql,Convert.ToInt64(c.Value)) );
            }

            return View(new User());
        }
    }
}