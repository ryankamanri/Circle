using System.Collections.Generic;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebViewServer.Models.User;
using WebViewServer.Models.Post;
using System;

namespace WebViewServer.Controllers
{
	[Controller]
	[Route("Api")]
	public class ApiController : Controller
	{
		private readonly User _user;

		private readonly Api _api;

		private readonly ILogger<ApiController> _logger;

		private readonly string BAD_REQUEST = "Bad Request";

		public ApiController(User user, Api api, ILoggerFactory loggerFactory)
		{
			_user = user;
			_api = api;
			_logger = loggerFactory.CreateLogger<ApiController>();
		}


		[HttpPost]
		[Route("User/GetSelfInfo")]
		public async Task<string> GetSelf()
		{
			var selfInfo = await _api.Post<UserInfo>("/User/GetUserInfo", new Form()
			{
				{ "User", _user }
			});

			return selfInfo.ToJson();
		}


		[HttpPost]
		[Route("User/GetUserInfo")]
		public async Task<string> GetUserInfo()
		{
			if (!HttpContext.Request.Form.ContainsKey("User")) return BAD_REQUEST;
			var user = HttpContext.Request.Form["User"].ToObject<User>();
			var userInfo = await _api.Post<UserInfo>("/User/GetUserInfo", new Form()
			{
				{ "User", user }
			});
			return userInfo.ToJson();
		}


		[HttpPost]
		[Route("User/SelectUserInfoInitiative")]
		public async Task<string> SelectUserInfoInitiative()
		{
			if (!HttpContext.Request.Form.ContainsKey("Selections")) return BAD_REQUEST;
			var selections = HttpContext.Request.Form["Selections"].ToObject<Form>();
			var users = await _api.Post<List<User>>("/User/SelectUserInitiative", new Form()
			{
				{ "User", _user },
				{ "Selections", selections }
			});

			var userInfoList = new List<UserInfo>();
			foreach (var user in users)
			{
				var userInfo = await _api.Post<UserInfo>("/User/GetUserInfo", new Form()
				{
					{ "User", user }
				});
				userInfoList.Add(userInfo);
			}

			_logger.LogDebug($"Focus User Count : {userInfoList.Count}");
			return userInfoList.ToJson();
		}

		[HttpPost]
		[Route("SendAComment")]
		public async Task<string> SendAComment()
		{
			if (!HttpContext.Request.Form.TryGetValue("postID", out var postID)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("comment", out var comment)) return "bad request".ToJson();
			_logger.LogDebug($"Comment String: {comment.ToString()}");
			return (await _api.Post("/Post/InsertAComment", new Form()
			{
				{ "User", _user },
				{ "Post", new Post(Convert.ToInt64(postID)) },
				{ "CommentText", comment.ToString() }
			})).ToJson();
		}

		[HttpPost]
		[Route("SendACommentReply")]
		public async Task<string> SendACommentReply()
		{
			if (!HttpContext.Request.Form.TryGetValue("postID", out var postID)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("replyCommentID", out var replyCommentID)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("comment", out var comment)) return "bad request".ToJson();

			return (await _api.Post("/Post/InsertACommentReply", new Form()
			{
				{ "User", _user },
				{ "Post", new Post(Convert.ToInt64(postID)) },
				{ "ReplyComment", new Comment(Convert.ToInt64(replyCommentID)) },
				{ "CommentText", comment.ToString() }
			})).ToJson();
		}
	}
}