using System;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using dotnet.Model;
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



        public TestController(SQL sql,DataBaseContext dbc)
        {
            _sql = sql;
            _dbc = dbc;
        }

        [HttpGet]
        [Route("email")]
        public IActionResult SendEmail(string emailto,string title,string body)
        {
            EmailHelper.SendThread(emailto,title,body);
            return new JsonResult("OK");
        }

        [HttpGet]
        [Route("Hello")]
        public string Hello()
        {
            return "MySql Hello";
        }
        [HttpGet]
        [Route("sql")]
        public string SqlTest()
        {

            string result = "";
            Tag _tag = new Tag();

            IList<Tag> tags = _tag.GetList(_sql.Query("select * from tags"));
            
            foreach (var tag in tags)
            {
                result += $"{tag.ToString()}\n";
            }

            return result;
        }

        [HttpGet]
        [Route("dbc")]
        public string DBCTest()
        {
            string result = "";

            dynamic json = new ExpandoObject();
            json.value = 10; 

            result += $"{JsonConvert.SerializeObject(json)}";
            
            
            return result;
        }
    }
}