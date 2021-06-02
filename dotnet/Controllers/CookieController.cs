using System;
using Microsoft.AspNetCore.Mvc;

namespace dotnet
{
    [Route("C/")]
    [Controller]
    public class CookieController : Controller
    {
        [HttpGet]
        [Route("Hello")]
        public string Hello()
        {
            return "Hello";
        }
    }
}