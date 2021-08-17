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
using dotnet.Services.Http;
using dotnet.Services.Database;
using dotnet.Services.Self;


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

        private Api _api;



        public TestController(SQL sql,DataBaseContext dbc,TagService tagService,User user,Api api)
        {
            _sql = sql;
            _dbc = dbc;
            _tagService = tagService;
            _user = user;
            _api = api;
        }

        [HttpGet]
        [Route("email")]
        public IActionResult SendEmail(string emailto,string title,string body)
        {
            EmailHelper.SendThread(emailto,title,body);
            return new JsonResult("OK");
        }

        [HttpGet]
        [Route("getapi")]
        public async Task<string> GetApi()
        {
            return await _api.Get("/api/get");
        }

        [HttpGet]
        [Route("postapi")]
        public async Task<string> PostApi()
        {
            return await _api.Post("/api/post",
            new Dictionary<string,object>()
            {
                {"Type","1"}
            });
        }
        
    }
}