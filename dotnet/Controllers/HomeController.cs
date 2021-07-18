using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnet.Model;
using dotnet.Middlewares;
using dotnet.Services;
using dotnet.Services.Database;
using dotnet.Services.Extensions;
using dotnet.Services.Cookie;

namespace dotnet.Controllers
{
    [Controller]
    [Authorize]
    [Route("Home")]
    public class HomeController : Controller
    {
        private User _user;
        private ICookie _cookie;
        private DataBaseContext _dbc;

        private UserService _userService;

        private TagService _tagService;


        public HomeController(DataBaseContext dbc, ICookie cookie, User user, UserService userService,TagService tagService)
        {
            _dbc = dbc;
            _cookie = cookie;
            _user = user;
            _userService = userService;
            _tagService = tagService;
        }


        #region Home

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// 添加个人标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTag")]
        public async Task<IActionResult> AddTag()
        {
            StringValues tagID = new StringValues(), tagRelation = new StringValues();
            if (!HttpContext.Request.Form.TryGetValue("tagID", out tagID)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("tagRelation", out tagRelation)) return new JsonResult("bad request");
            Tag tag = new Tag(Convert.ToInt64(tagID));

            await _userService.AppendTagRelation(_user,tag, tagRelation.ToString());

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
            StringValues tagID = new StringValues(), tagRelation = new StringValues();
            if (!HttpContext.Request.Form.TryGetValue("tagID", out tagID)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("tagRelation", out tagRelation)) return new JsonResult("bad request");
            Tag tag = new Tag(Convert.ToInt64(tagID));

            await _userService.RemoveTagRelation(_user,tag,tagRelation.ToString());

            return new JsonResult("remove tag succeed");
        }
        #endregion


        #region PrivateChat
        [HttpGet]
        [Route("PrivateChat")]
        public IActionResult PrivateChat()
        {
            return View("PrivateChat");
        }
        #endregion

        #region TagTree

        [HttpGet]
        [Route("TagTree")]
        public IActionResult TagTree()
        {
            return View("TagTree");
        }

        [HttpPost]
        [Route("FindChildTags")]
        public IActionResult FindChildTags()
        {
            StringValues tagID = new StringValues();
            if (!HttpContext.Request.Form.TryGetValue("tagID", out tagID)) return new JsonResult("bad request");
            
            ICollection<Tag> childTags = _tagService.FindChildTag(new Tag(Convert.ToInt64(tagID)));

            IList<string> resultJSON = new List<string>();

            foreach(var childTag in childTags)
            {
                resultJSON.Add(this.RenderViewAsync("Tag",childTag,true).Result);
            }

            return new JsonResult(JsonConvert.SerializeObject(resultJSON));
        }
            
        #endregion


        #region SearchResult

        [HttpGet]
        [Route("SearchResult")]
        public IActionResult SearchResult(string searchString)
        {
            return View("SearchResult",searchString);
        }
            
        #endregion

    }
}