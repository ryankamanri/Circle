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

            
            Tag tag37 = new Tag(37);
            Tag tag17 = new Tag(17);

            _dbc.Select(tag37);
            _dbc.Select(tag17);
            _dbc.ChangeRelation(tag37,tag17,relation => 
            {
                relation.relation = 1;
            });

            dynamic relation = _dbc.SelectRelation(tag37,tag17);

            result += $"{JsonConvert.SerializeObject(relation)}";
            

            

            //_dbc.Connect(user,post,relation => {});

            // _dbc.ChangeRelation(user,post,relation =>
            // {
            //     relation.t1 = 1;
            // });
            // dynamic relation = _dbc.SelectRelation(user,post);
            // result += $"{JsonConvert.SerializeObject(relation)}";
            // IList<Post> posts = _dbc.MappingSelect(user,new Post(),ID_IDList.OutPutType.Value,selections => 
            // {
            //     selections.t1 = 1;
            // });
            
            // foreach(var postResult in posts)
            // {
            //     result += $"{postResult.ToString()}\n";
            // }

            return result;
        }

        [HttpGet]
        [Route("dbc")]
        public async Task<string> DBCTest()
        {
            string result = "";

            // Tag 线性代数 = new Tag("线性代数");
            // Tag 概率论 = new Tag("概率论");
            // Tag 计算机 = new Tag("计算机");
            // _dbc.SelectID(线性代数);
            // _dbc.SelectID(概率论);
            // _dbc.SelectID(计算机);
            // double similarity = _tagService.CalculateSimilarity(线性代数,线性代数);
            // result += $"{similarity}\n";
            // similarity = _tagService.CalculateSimilarity(概率论,计算机);
            // result += $"{similarity}\n";

            //await _dbc.Connect(_user,_dbc.Select(new Tag(3)),relation => relation.Type = new List<string>(){"Self"});
            //await _dbc.Connect(_user,_dbc.Select(new Tag(4)),relation => relation.Type = new List<string>(){"Interested"});
            IEnumerable<int> list = new List<int>();

            list.Intersect(list);
            
            return result;
        }
    }
}