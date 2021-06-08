using System;
using Microsoft.AspNetCore.Mvc;
using dotnet.Model;
using dotnet.Services;
using System.Collections.Generic;
using System.Linq;

namespace dotnet.Controllers
{
    [Controller]
    [Route("map/")]
    public class MappingController : Controller
    {
        private DataBaseContext _dbc;
        private SQL _sql;
        public MappingController(SQL sql,DataBaseContext dbc)
        {
            _sql = sql;
            _dbc = dbc;
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            string result = "";


             return result;
        }


        [HttpGet]
        [Route("search")]
        public string Search()
        {
            string result = "";

            result += "匹配同时拥有第一个标签,考研的标签的所有用户\n";

            string tag1 = "第一个标签";
            string tag2 = "考研";
            List<string> tags = new List<string>(){tag1,tag2};

            List<long> tagIDs = Tag.FindIDs(_sql,tags);

            foreach(var ID in tagIDs)
            {
                result += $"标签的ID是{ID}\n";
            }
            

            //IEnumerable<long> UserIDs =  _dbc.Users_SortedTags.FindAndIntersect(new List<long>(){3},ID_IDList.OutPutType.Key);
            IEnumerable<long> UserIDs =  _dbc.Users_SortedTags.FindAndIntersect(tagIDs,ID_IDList.OutPutType.Key);


            foreach(var ID in UserIDs)
            {
                result += $"用户的ID是{ID}\n";
            }
            IList<User> users =  Model.User.Finds(_sql,UserIDs);
            foreach(var user in users)
            {
                result += $"{user.ToString()}\n";
            }

            result += $"{"匹配第一个用户的所有帖子"}\n";

            string account = "974481066@qq.com";
            string password = "123456";

            long userID = Model.User.FindID(_sql,account,password);

            result += $"{userID}\n";

            List<long> PostIDs = _dbc.SortedUsers_Posts.Find(userID,ID_IDList.OutPutType.Value);

            foreach(var tagID in PostIDs)
            {
                result += $"帖子的ID是{tagID}\n";
            }

            IList<Post> posts = Model.Post.Finds(_sql,PostIDs);

            foreach(var post in posts)
            {
                result += $"{post._Post}\n";
            }
            return result;
        }
    }
}