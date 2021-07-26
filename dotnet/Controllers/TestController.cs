using System;
using System.Dynamic;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using dotnet.Model;
using dotnet.Model.Relation;
using dotnet.Services;
using dotnet.Services.Database;
using dotnet.Services.Extensions;


namespace dotnet.Controllers
{
    [Route("test/")]
    [Controller]
    public class TestController : Controller
    {

        private SQL _sql;
        private DataBaseContext _dbc;

        private TagService _tagService;

        private User _user;



        public TestController(SQL sql,DataBaseContext dbc,TagService tagService,User user)
        {
            _sql = sql;
            _dbc = dbc;
            _tagService = tagService;
            _user = user;
        }

        [HttpGet]
        [Route("email")]
        public IActionResult SendEmail(string emailto,string title,string body)
        {
            EmailHelper.SendThread(emailto,title,body);
            return new JsonResult("OK");
        }

        
    }
}