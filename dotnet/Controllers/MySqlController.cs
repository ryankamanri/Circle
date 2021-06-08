using System;
using Microsoft.AspNetCore.Mvc;
using dotnet.Model;
using dotnet.Services;
using System.Collections.Generic;

namespace dotnet.Controllers
{
    [Route("MySql/")]
    [Controller]
    public class MySqlController : Controller
    {

        private SQL _sql;
        private DataBaseContext _dbc;
        public MySqlController(SQL sql,DataBaseContext dbc)
        {
            _sql = sql;
            _dbc = dbc;
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

            IList<Tag> tags = Model.Tag.GetList(_sql.Query("select * from tags"));
            
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

           
            

            return result;
        }
    }
}