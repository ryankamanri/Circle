using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using dotnet.Model;
using dotnet.Services;


namespace dotnet.Controllers
{
    [Controller]
    [Route("map/")]
    public class MappingController : Controller
    {
        private DataBaseContext _dbc;
        private SQL _sql;

        private ICookie _cookie;
        public MappingController(SQL sql,DataBaseContext dbc,ICookie cookie)
        {
            _sql = sql;
            _dbc = dbc;
            _cookie = cookie;
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
            

            long userID = Model.User.FindID(_sql,account);

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

        [HttpGet]
        [Route("connectTest")]
        public async Task<string> ConnectTest()
        {
            string result = "";
            int userID = default;
            string tagContext = "第一个标签";

            result += $"建立本用户的帖子与'第一个标签'标签的关系\n";

            foreach(var c in _cookie.GetCookie(HttpContext).Claims)
            {
                if(c.Type == "ID") userID = Convert.ToInt32(c.Value); //从cookie中找到用户ID
            }

            User user = Model.User.Find(_sql,userID);//由用户ID在数据库中找到用户

            result += $"用户 : {user.ToString()}\n";

            long tagID = Tag.FindID(_sql,tagContext);//由标签内容找到标签ID

            if (tagID == long.MinValue) return result;//表示没找到对应的标签

            Tag tag = new Tag(tagID,tagContext);

            result += $"帖子 : {tag.ToString()}";

            await _dbc.Connect<User,Tag>(user,tag);//用户跟标签建立关系

            return result;
        }

        [HttpGet]
        [Route("operation")]
        public async Task Operation()
        {
            
        }
    }
}