using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebViewServer.Models.Post;
using WebViewServer.Models.User;
using WebViewServer.Services;
using WebViewServer.Services.Cookie;
using WebViewServer.Services.Extensions;

namespace WebViewServer.Controllers
{
	[Controller]
	[Authorize]
	[Route("Shared/")]
	public class SharedController : Controller
	{

		private User _user;

		private UserService _userService;

		private TagService _tagService;

		private PostService _postService;

		private ICookie _cookie;


		public SharedController(ICookie cookie, User user, UserService userService, TagService tagService, PostService postService)
		{
			_cookie = cookie;
			_user = user;
			_tagService = tagService;
			_userService = userService;
			_postService = postService;

		}

        #region Components

		#region Post
		
		[HttpGet]
        [Route("Components/Post/FirstLevelComment")]
        public IActionResult Components_Post_FirstLevelComment()
        {
			return View("Components/Post/FirstLevelComment");
        }

		[HttpGet]
        [Route("Components/Post/SecondLevelComment")]
        public IActionResult Components_Post_SecondLevelComment()
        {
			return View("Components/Post/SecondLevelComment");
        }

		[HttpGet]
        [Route("Components/Post/Post")]
        public IActionResult Components_Post()
        {
			return View("Components/Post/Post");
        }

		#endregion
        
		#region PrivateChat
		[HttpGet]
		[Route("Components/PrivateChat/OtherMessageItem")]
		public IActionResult Components_PrivateChat_OtherMessageItem()
		{
			return View("Components/PrivateChat/OtherMessageItem");
		}

		[HttpGet]
		[Route("Components/PrivateChat/SelfMessageItem")]
		public IActionResult Components_PrivateChat_SelfMessageItem()
		{
			return View("Components/PrivateChat/SelfMessageItem");
		}

		[HttpGet]
		[Route("Components/PrivateChat/TimeMessageItem")]
		public IActionResult Components_PrivateChat_TimeMessageItem()
		{
			return View("Components/PrivateChat/TimeMessageItem");
		}

		[HttpGet]
		[Route("Components/PrivateChat/UserInfo")]
		public IActionResult Components_PrivateChat_UserInfo()
		{
			return View("Components/PrivateChat/UserInfo");
		}
		#endregion

		[HttpGet]
        [Route("Components/SearchUserInfo")]
        public IActionResult Components_SearchUserInfo()
        {
			return View("Components/SearchUserInfo");
        }

		[HttpGet]
        [Route("Components/Tag")]
        public IActionResult Components_Tag()
        {
			return View("Components/Tag");
        }

        #endregion

        [HttpGet]
		[Route("SharedTagTree")]
		public IActionResult SharedTagTree()
		{
			return View("SharedTagTree");
		}

		/// <summary>
		/// 添加个人标签
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("AppendRelation")]
		public async Task<IActionResult> AppendRelation()
		{
			string entityType = HttpContext.Request.Form["entityType"];
			string ID = HttpContext.Request.Form["ID"];
			string relationName = HttpContext.Request.Form["relationName"];
			string relation = HttpContext.Request.Form["relation"];

			if (!await _userService.AppendRelation(_user, entityType, Convert.ToInt64(ID), relationName, relation)) return new JsonResult("entity is not exist");

			return new JsonResult("append succeed");
		}

		/// <summary>
		/// 删除个人标签
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("RemoveRelation")]
		public async Task<IActionResult> RemoveRelation()
		{
			string entityType = HttpContext.Request.Form["entityType"];
			string ID = HttpContext.Request.Form["ID"];
			string relationName = HttpContext.Request.Form["relationName"];
			string relation = HttpContext.Request.Form["relation"];

			if (!await _userService.RemoveRelation(_user, entityType, Convert.ToInt64(ID), relationName, relation)) return new JsonResult("entity is not exist");


			return new JsonResult("remove succeed");
		}

		/// <summary>
		/// 搜索标签
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("Search")]
		public async Task<IActionResult> Search()
		{
			string search = "";
			List<string> tagsString = new List<string>();

			if (HttpContext.Request.Form.TryGetValue("search", out var searchInfo) == false) return new JsonResult("bad request");

			search = searchInfo;

			var tagList = await _tagService.TagIndex(search);


			foreach (var tag in tagList)
			{
				tagsString.Add(await this.RenderViewAsync("Tag", tag, true));
			}
			var resultJSON = tagsString.ToJson();
			return new JsonResult(resultJSON);

		}

		[HttpPost]
		[Route("ShowPostInfo")]
		public async Task<IActionResult> ShowPostInfo()
		{
			if (!HttpContext.Request.Form.TryGetValue("postID", out var postID)) return new JsonResult("bad request");

			PostInfo postInfo = await _postService.GetPostInfo(new Post(Convert.ToInt64(postID.ToString())));

			return new JsonResult(postInfo.ToJson());
		}

		[HttpPost]
		[Route("SelectPostComments")]
		public async Task<string> SelectPostComments()
		{
			if (!HttpContext.Request.Form.TryGetValue("postID", out var postID)) return "bad request".ToJson();
			var commentList = await _postService.SelectComments(new Post(Convert.ToInt64(postID.ToString())));

			return commentList.ToJson();
		}
		[HttpPost]
		[Route("SelectFormedCommentsAndUser")]
		public async Task<string> SelectFormedCommentsAndUser()
		{
			if (!HttpContext.Request.Form.TryGetValue("postID", out var postID)) return "bad request".ToJson();
			var commentList = await _postService.SelectFormedCommentsAndUser(new Post(Convert.ToInt64(postID.ToString())), _user);

			return commentList.ToJson(dateTimeFormatString: "yyyy-MM-dd HH:mm");
		}

		
		
	}
}