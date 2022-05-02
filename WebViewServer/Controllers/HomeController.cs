using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kamanri.Http;
using Kamanri.Extensions;
using Microsoft.Extensions.Logging;
using WebViewServer.Models;
using WebViewServer.Models.Post;
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

		private Api _api;
		private User _user;

		private ViewModelService _vmService;
		private ICookie _cookie;

		private UserService _userService;

		private TagService _tagService;

		private PostService _postService;

		private UploadService _uploadService;

		private ILogger<HomeController> _logger;


		public HomeController(ICookie cookie, User user, Api api, ViewModelService vmService, UserService userService, TagService tagService, PostService postService, UploadService uploadService, ILoggerFactory loggerFactory)
		{
			_cookie = cookie;
			_user = user;
			_api = api;
			_vmService = vmService;
			_userService = userService;
			_tagService = tagService;
			_postService = postService;
			_uploadService = uploadService;
			_logger = loggerFactory.CreateLogger<HomeController>();
		}

		[HttpGet]
		[Route("Template")]
		public IActionResult Template()
		{
			return View("_Template");
		}

		#region Home

		[HttpGet]
		public IActionResult Home()
		{
			return View("_Template");
		}

		[HttpGet]
		[Route("Home/Template")]
		public IActionResult HomeTemplate()
		{
			return View("Home/Template");
		}

		[HttpGet]
		[Route("Home/Posts")]
		public IActionResult Posts()
		{
			return View("Home/Posts");
		}
		
		[HttpPost]
		[Route("Home/PostsModel")]
		public async Task<string> PostsModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetHomePostsViewModels(new PostService.CircleType()
			               {
				               Postgraduate = true,
				               Employment = true
			               }))
			{
				modelList.Add(await model);
			}

			return modelList.ToJson();
		}
		
		[HttpPost]
		[Route("Home/PostsExtraModel")]
		public async Task<string> PostsExtraModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetExtraPostsViewModels(new PostService.CircleType()
			               {
				               Postgraduate = true,
				               Employment = true
			               }))
			{
				modelList.Add(await model);
			}
			return modelList.ToJson();
		}

		[HttpPost]
		[Route("Home/PostgraduatePostsModel")]
		public async Task<string> PostgraduatePostsModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetHomePostsViewModels(new PostService.CircleType()
			               {
				               Postgraduate = true,
				               Employment = false
			               }))
			{
				modelList.Add(await model);
			}

			return modelList.ToJson();
		}
		
		[HttpPost]
		[Route("Home/EmploymentPostsModel")]
		public async Task<string> EmploymentPostsModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetHomePostsViewModels(new PostService.CircleType()
			               {
				               Employment = true,
				               Postgraduate = false
			               }))
			{
				modelList.Add(await model);
			}

			return modelList.ToJson();
		}

		[HttpPost]
		[Route("Home/PostgraduatePostsExtraModel")]
		public async Task<string> PostgraduatePostsExtraModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetExtraPostsViewModels(new PostService.CircleType()
			               {
				               Postgraduate = true,
				               Employment = false
			               }))
			{
				modelList.Add(await model);
			}
			return modelList.ToJson();
		}
		
		[HttpPost]
		[Route("Home/EmploymentPostsExtraModel")]
		public async Task<string> EmploymentPostsExtraModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetExtraPostsViewModels(new PostService.CircleType()
			               {
				               Employment = true,
				               Postgraduate = false
			               }))
			{
				modelList.Add(await model);
			}
			return modelList.ToJson();
		}

		#region Zone

		[HttpGet]
		[Route("Home/Zone")]
		public IActionResult Zone()
		{
			return View("Home/Zone");
		}

		[HttpPost]
		[Route("Home/ZoneModel")]
		public async Task<string> ZoneModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetZonePostsViewModels())
			{
				modelList.Add(await model);
			}
			return modelList.ToJson();
		}


		#endregion


		[HttpGet]
		[Route("Home/Notice")]
		public IActionResult Notice()
		{
			return View("Home/Notice");
		}

		[HttpPost]
		[Route("Home/NoticeModel")]
		public async Task<string> NoticeModel()
		{
			return (await _vmService.GetNoticeViewModels()).ToJson(dateTimeFormatString: "yyyy-MM-dd HH:mm");
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

		#region PostItem

		[HttpGet]
		[Route("PostItem")]
		public IActionResult PostItem()
		{
			return View("PostItem");
		}

		[HttpGet]
		[Route("PostItemModel")]
		public async Task<string> PostItemModel(string postID)
		{
			var post = await _postService.GetPost(postID);
			return (await _vmService.GetPostViewModel(post)).ToJson();

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
		
		#endregion

		

		#region UserPage

		[HttpGet]
		[Route("Userpage")]
		public async Task<IActionResult> UserPage()
		{
			var userInfo = await _userService.GetUserInfo(_user);
			return View("UserPage",userInfo);
		}
		
		[HttpPost]
		[Route("UserPagePostModel")]
		public async Task<string> UserPagePostModel()
		{
			var modelList = new List<Form>();
			await foreach (var model in _vmService.GetUserPagePostsViewModels())
			{
				modelList.Add(await model);
			}
			return modelList.ToJson();
		}

		[HttpPost]
		[Route("SelectPostModel")]
		public async Task<string> SelectPostModel()
		{
			if (!HttpContext.Request.Form.TryGetValue("Type[]", out var type))
			{
				return new Form()
				{
					{"Status","Failure"},
					{"Info","Bad Request"}
				}.ToJson();
			}
			
			

			var posts = (await _userService.SelectPost(_user, 
				selection => selection.Type = type.ToList()));
			var result = new List<Form>();

			foreach (var post in posts)
			{
				result.Add(await _vmService.GetPostViewModel(post));
			}

			return result.ToJson();
		}

		[HttpPost]
		[Route("SelectCommentModel")]
		public async Task<string> SelectCommentModel()
		{
			if (!HttpContext.Request.Form.TryGetValue("Type[]", out var type))
			{
				return new Form()
				{
					{"Status","Failure"},
					{"Info","Bad Request"}
				}.ToJson();
			}
			var comments = (await _userService.SelectComment(_user, 
				selection => selection.Type = type.ToList()));
			var result = new List<KeyValuePair<Comment, Form>>();

			foreach (var comment in comments)
			{
				result.Add(await _vmService.GetCommentViewModel(comment));
			}

			return result.ToJson();
		}
		#endregion

		

		#region Setting

		

		
		[HttpGet]
		[Route("Setting")]
		public IActionResult Setting()
		{
			return View("Setting");
		}

		#region AccountInfo

		

		

		[HttpGet]
		[Route("AccountInfo")]
		public async Task<IActionResult> AccountInfo()
		{
			var userInfo = await _userService.GetUserInfo(_user);
			return View("AccountInfo", userInfo);
		}

		[HttpPost]
		[Route("AccountInfoSubmit")]
		public async Task<string> AccountInfoSubmit()
		{
			if (!HttpContext.Request.Form.TryGetValue("NickName", out var nickName)
				|| (!HttpContext.Request.Form.TryGetValue("RealName", out var realName))
				|| (!HttpContext.Request.Form.TryGetValue("University", out var university))
				|| (!HttpContext.Request.Form.TryGetValue("School", out var school))
				|| (!HttpContext.Request.Form.TryGetValue("Speciality", out var speciality))
				|| (!HttpContext.Request.Form.TryGetValue("SchoolYear", out var schoolYear))
				|| (!HttpContext.Request.Form.TryGetValue("Introduction", out var introduction)))
			{
				return new Form()
				{
					{"Status","Failure"},
					{"Info","Bad Request"}
				}.ToJson();
			}

			var imageUrl = (await _userService.GetUserInfo(_user)).HeadImage;

			if (HttpContext.Request.Form.Files.Count != 0)
			{
				var imageFile = HttpContext.Request.Form.Files[0];
				var uploadInfo = (await _uploadService.UploadFile(
					imageFile, 
					UploadService.UploadFileType.IMAGE, 
					UploadService.ImageType.HEAD_IMAGE));

				if (uploadInfo["Status"] as string != "Success")
				{
					return uploadInfo.ToJson();
				}

				imageUrl = uploadInfo["Info"] as string;
				_logger.LogDebug($"New Image Url: {imageUrl}");
				
			}
				
			_logger.LogDebug($"Image Url: {imageUrl}");
			
			var userInfo = new UserInfo(_user.ID, nickName, realName, university, school, speciality, Convert.ToDateTime(schoolYear),
				introduction, imageUrl);
			var successInfo = await _userService.InsertOrUpdateUserInfo(userInfo);
			return new Form()
			{
				{ "Status", "Success" },
				{ "Info", successInfo }
			}.ToJson();
		}
		
		#endregion

		[HttpGet]
		[Route("Password")]
		public IActionResult Password()
		{
			return View("Password");
		}
		
		#endregion

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

		


		#region SearchResult

		[HttpGet]
		[Route("SearchResult")]
		public IActionResult SearchResult(string searchString)
		{
			return View("SearchResult", searchString);
		}

		[HttpGet]
		[Route("SearchResultModel")]
		public async Task<string> SearchResultModel(string searchString)
		{
			if (searchString == null) return "NO Search String".ToJson();
			return (await _vmService.GetSearchResultViewModel(searchString)).ToJson();
		}

		#endregion

		#region Match
		
		[HttpGet]
		[Route("Match")]
		public IActionResult Match()
		{
			return View("Match");
		}

		[HttpGet]
		[Route("MatchModel")]
		public async Task<string> MatchModel()
		{
			return (await _vmService.GetMatchModels()).ToJson();
		}

		#endregion

	}
}