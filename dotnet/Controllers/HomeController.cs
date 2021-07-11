using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using dotnet.Model;
using dotnet.Middlewares;
using dotnet.Services.Database;
using dotnet.Services.Cookie;

namespace dotnet.Controllers
{
    [Controller]
    [Route("Home")]
    public class HomeController : Controller
    {
        private User _user;
        private ICookie _cookie;
        private DataBaseContext _dbc;


        public HomeController(DataBaseContext dbc,ICookie cookie,User user)
        {
            _dbc = dbc;
            _cookie = cookie;
            _user = user;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Home()
        {
            return View(_user);
        }

        /// <summary>
        /// 添加个人标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTag")]
        public async Task<IActionResult> AddTag()
        {
            StringValues tagID = new StringValues(),tagRelation = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("tagID",out tagID) == false) return new JsonResult("bad request");
            if(HttpContext.Request.Form.TryGetValue("tagRelation",out tagRelation) == false) return new JsonResult("bad request");
            Tag tag = new Tag(Convert.ToInt64(tagID));
            dynamic relation = _dbc.SelectRelation<User,Tag>(_user,tag);

            if (relation == null)
            {   //两个实体之间的关系不存在,则新建关系
                await _dbc.Connect<User, Tag>(_user, tag, relation => relation.Type = new List<string>() { tagRelation.ToString() });
                return new JsonResult("add tag succeed");
            }
            await _dbc.ChangeRelation<User, Tag>(_user, tag, async relation =>
               {
                   bool isTypeExists = false;
                   foreach (var properties in relation)
                   {
                       //两个实体之间存在名为Type的关系
                       if (properties.Key.ToString() == "Type")
                       {
                           if (!relation.Type.Contains(tagRelation.ToString()))
                           {
                               relation.Type.Add(tagRelation.ToString());
                               isTypeExists = true;
                               return;
                           }
                       }
                   }
                   //两个实体之间不存在名为Type的关系
                   if (isTypeExists == false) relation.Type = new List<string>() { tagRelation.ToString() };
               });

            return new JsonResult("add tag succeed");
        }

        /// <summary>
        /// 删除个人标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveTag")]
        public async Task<IActionResult> RemoveTag()
        {
            StringValues tagID = new StringValues(),tagRelation = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("tagID",out tagID) == false) return new JsonResult("bad request");
            if(HttpContext.Request.Form.TryGetValue("tagRelation",out tagRelation) == false) return new JsonResult("bad request");
            Tag tag = new Tag(Convert.ToInt64(tagID));
            //dynamic relation = _dbc.SelectRelation<User,Tag>(_user,tag);

            await _dbc.ChangeRelation<User,Tag>(_user,tag,async relation => 
            {
                relation.Type.Remove(tagRelation.ToString());
                if(relation.Type.Count == 0) 
                    await _dbc.Disconnect<User,Tag>(_user,tag);
            });
            
            return new JsonResult("remove tag succeed");
        }
    }
}