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
        public MySqlController(SQL sql)
        {
            _sql = sql;
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

            IList<User> users = Model.User.GetList(_sql.Query("select * from users"));
            
            foreach (var user in users)
            {
                result += $"{user.ToString()}\n";
            }

            return result;
        }
    }
}