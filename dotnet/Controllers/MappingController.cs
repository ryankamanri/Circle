using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using dotnet.Model;
using dotnet.Model.Relation;
using dotnet.Services.Database;
using dotnet.Services.Cookie;



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
        public async Task<string> Test()
        {
            string result = "";

            //创建用户
            //Model.User userTest = new User();
            //保存到数据库
            //await userTest.Save(_sql);

            //查找并修改用户数据
             //Model.User userTest = Model.User.Find(_sql,Model.User.FindID(_sql,"2168359585@qq.com"));
             //userTest.Password = "456789";
             //userTest.Modify(_sql);

            //查找并删除用户
            // Model.User userTest = Model.User.Find(_sql,Model.User.FindID(_sql,"2168359585@qq.com"));
            // userTest.Remove(_sql);

            //用户与id为1帖子建立关系
            //await _dbc.Connect<Model.User,Tag>(userTest,Tag.Find(_sql,1));

            //用户与id为1帖子解除关系

            //await _dbc.DisConnect<Model.User,Tag>(userTest,Tag.Find(_sql,1));

             return result;
        }


        [HttpGet]
        [Route("sc")]
        public string Sc()
        {

            Tag _tag = new Tag();
            User _user = new User();
            string result = "";


            Tag tag1 = new Tag("第一个标签");
            Tag tag2 = new Tag("考研");
            Tag tag3 = new Tag("哲学");

            result += $"匹配拥有'{tag1}','{tag2}','{tag3}'的标签的用户\n";

            
            List<Tag> tags = new List<Tag>(){tag1,tag2,tag3};

            List<long> tagIDs = _tag.SelectIDs(_sql,tags);

            foreach(var ID in tagIDs)
            {
                result += $"标签的ID是{ID}\n";
            }
            

            //IEnumerable<long> UserIDs =  _dbc.Users_SortedTags.FindAndIntersect(tagIDs,ID_IDList.OutPutType.Key);

            Key_ListValue_Pairs<long,long> userIDs_tagsIDs = _dbc.Users_SortedTags.FindUnionStatistics(tagIDs,ID_IDList.OutPutType.Key);
            //KeyValuePair<long,IList<long>> kp = new KeyValuePair<long, IList<long>>();
            foreach(var userID_tagsID in userIDs_tagsIDs)
            {
                var userID = userID_tagsID.Key;
                IList<long> tagsID = userID_tagsID.Value;
                IList<long> allTagsID = _dbc.SortedUsers_Tags.Find(userID,ID_IDList.OutPutType.Value);
                result += $"\n用户的ID是{userID}\n";
                User user = _user.Select(_sql,userID);
                result += $"用户的信息是{user.ToString()}\n";

                foreach(var tagID in allTagsID)
                {
                    _tag.Select(_sql,tagID);

                    result += $"用户的标签有{_tag.ToString()}\n";
                }

                foreach(var tagID in tagsID)
                {
                    _tag.Select(_sql,tagID);

                    result += $"与该用户相同的标签有{_tag.ToString()}\n";
                }


            }


            // IList<User> users =  Model.User.Finds(_sql,UserIDs);
            // foreach(var user in users)
            // {
            //     result += $"{user.ToString()}\n";
            // }

            // result += $"{"匹配第一个用户的所有帖子"}\n";

            // string account = "974481066@qq.com";
            

            // long userID = Model.User.FindID(_sql,account);

            // result += $"{userID}\n";

            // List<long> PostIDs = _dbc.SortedUsers_Posts.Find(userID,ID_IDList.OutPutType.Value);

            // foreach(var tagID in PostIDs)
            // {
            //     result += $"帖子的ID是{tagID}\n";
            // }

            // IList<Post> posts = Model.Post.Finds(_sql,PostIDs);

            // foreach(var post in posts)
            // {
            //     result += $"{post._Post}\n";
            // }
            return result;
        }

        [HttpGet]
        [Route("connectTest")]
        public async Task<string> ConnectTest()
        {
            string result = "";
            User _user = new User();
            int userID = default;
            Tag tag = new Tag("第一个标签");

            result += $"建立本用户的帖子与'第一个标签'标签的关系\n";

            foreach(var c in _cookie.GetCookie(HttpContext).Claims)
            {
                if(c.Type == "ID") userID = Convert.ToInt32(c.Value); //从cookie中找到用户ID
            }

            _user.Select(_sql,userID);//由用户ID在数据库中找到用户

            result += $"用户 : {_user.ToString()}\n";

            long tagID = tag.SelectID(_sql,tag);//由标签内容找到标签ID

            if (tagID == long.MinValue) return result;//表示没找到对应的标签

            // Tag tag = new Tag(tagID,tagContext);

            result += $"帖子 : {tag.ToString()}";

            await _dbc.Connect<User,Tag>(_user,tag);//用户跟标签建立关系

            return result;
        }

        [HttpPost]
        [Route("Search")]
        public IActionResult Search()
        {
            string search = "";
            string result = "{\"result\" : [";
            StringValues searchInfo = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("search",out searchInfo) == false) return new JsonResult("bad request");

            search = searchInfo;
            if(!_dbc.TagIndex.ContainsKey(search)) 
            {
                result += "]}";
                return new JsonResult(result);
            }
            var tagList = _dbc.TagIndex[search];
            for(int i = 0;i < tagList.Count - 1;i++)
            {
                result += $"{tagList[i].ToString()}";
                result += ",";
            }
            result += $"{tagList[tagList.Count - 1].ToString()}";
            result += "]}";
            return new JsonResult(result);
            
        }
    }
}