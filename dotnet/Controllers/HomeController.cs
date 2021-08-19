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

        private UserService _userService;

        private TagService _tagService;

        private PostService _postService;


        public HomeController(ICookie cookie, User user, UserService userService,TagService tagService,PostService postService)
        {
            _cookie = cookie;
            _user = user;
            _userService = userService;
            _tagService = tagService;
            _postService = postService;
        }


        #region Home

        [HttpGet]
        public IActionResult Home()
        {
            return View("Home/Posts");
        }

        [HttpGet]
        [Route("Home/Posts")]
        public IActionResult Posts()
        {
            return View("Home/Posts");
        }

        [HttpGet]
        [Route("Home/Zone")]
        public IActionResult Zone()
        {
            return View("Home/Zone");
        }

        [HttpGet]
        [Route("Home/SendPost")]
        public IActionResult SendPost()
        {
            return View("Home/SendPost");
        }

        [HttpPost]
        [Route("SendPostSubmit")]
        public async Task<IActionResult> SendPostSubmit()
        {
            StringValues Title = new StringValues(), Focus = new StringValues(),Summary = new StringValues(), Content = new StringValues(),TagIDs = new StringValues();
            if (!HttpContext.Request.Form.TryGetValue("Title", out Title)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("Focus", out Focus)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("Summary", out Summary)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("Content", out Content)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("TagIDs", out TagIDs)) return new JsonResult("bad request");

            await _postService.InsertPost(_user,Title,Focus,Summary,Content,TagIDs);
            return new JsonResult("add succeed");
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
        public async Task<IActionResult> FindChildTags()
        {
            StringValues tagID = new StringValues();
            if (!HttpContext.Request.Form.TryGetValue("tagID", out tagID)) return new JsonResult("bad request");
            
            ICollection<Tag> childTags = await _tagService.FindChildTag(new Tag(Convert.ToInt64(tagID)));

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