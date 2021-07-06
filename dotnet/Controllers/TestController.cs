using System;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using dotnet.Model;
using dotnet.Model.Relation;
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
            // User user = new User();
            // user.Select(_sql,1);
            
            Tag tag = new Tag("dotnet");
            //_dbc.Insert<Tag>(tag);
            // _dbc.SelectID<Tag>(tag);
            // tag._Tag = ".NET";
            // _dbc.Update<Tag>(tag);
            // _dbc.Delete<Tag>(tag);

            

            //_dbc.SelectID<Post>(post);

            //List<Tag> tags = new List<Tag>(){new Tag("第一个标签"),new Tag("哲学")};

            //IList<Tag> tagsRes = _dbc.SelectIDs<Tag>(tags);

            // var users = _dbc.Mapping<Tag,User>(
            //     tag,new Model.User(),ID_IDList.OutPutType.Key);

            // foreach(var user in users)
            // {
            //     result += $"{JsonConvert.SerializeObject(user)}";
            // }

            Post post = new Post("无了无了");
            _dbc.SelectID(post);

            User user = new User(1);
            user = _dbc.Select(user);

            //_dbc.Connect(user,post,relation => {});

            // _dbc.ChangeRelation(user,post,relation =>
            // {
            //     relation.t1 = 1;
            // });
            dynamic relation = _dbc.SelectRelation(user,post);
            result += $"{JsonConvert.SerializeObject(relation)}";
            IList<Post> posts = _dbc.MappingSelect(user,new Post(),ID_IDList.OutPutType.Value,selections => 
            {
                selections.t1 = 1;
            });
            
            foreach(var postResult in posts)
            {
                result += $"{postResult.ToString()}\n";
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