using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebViewServer.Models;
using WebViewServer.Models.User;
using WebViewServer.Services;
using WebViewServer.Services.Cookie;
using WebViewServer.Services.Extensions;

namespace WebViewServer.Controllers
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


        public HomeController(ICookie cookie, User user, UserService userService, TagService tagService, PostService postService)
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
            if (!HttpContext.Request.Form.TryGetValue("Title", out var Title)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("Focus", out var Focus)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("Summary", out var Summary)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("Content", out var Content)) return new JsonResult("bad request");
            if (!HttpContext.Request.Form.TryGetValue("TagIDs", out var TagIDs)) return new JsonResult("bad request");

            await _postService.InsertPost(_user, Title, Focus, Summary, Content, TagIDs);
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
            if (!HttpContext.Request.Form.TryGetValue("tagID", out var tagID)) return new JsonResult("bad request");

            ICollection<Tag> childTags = await _tagService.FindChildTag(new Tag(Convert.ToInt64(tagID)));

            IList<string> resultJSON = new List<string>();

            foreach (var childTag in childTags)
            {
                resultJSON.Add(this.RenderViewAsync("Tag", childTag, true).Result);
            }

            return new JsonResult(resultJSON.ToJson());
        }

        #endregion


        #region SearchResult

        [HttpGet]
        [Route("SearchResult")]
        public IActionResult SearchResult(string searchString)
        {
            return View("SearchResult", searchString);
        }

        #endregion

    }
}