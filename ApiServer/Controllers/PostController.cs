using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Kamanri.Extensions;
using ApiServer.Services;
using ApiServer.Models.User;
using ApiServer.Models.Post;
using System;
using Kamanri.Http;
using Microsoft.Extensions.Logging;

namespace ApiServer.Controllers
{
	[ApiController]
	[Route("Post/")]
	public class PostController : ControllerBase
	{


		private PostService _postService;

		private readonly ILogger<PostController> _logger;
		public PostController(PostService postService, ILoggerFactory loggerFactory)
		{
			_postService = postService;
			_logger = loggerFactory.CreateLogger<PostController>();
		}

		#region Get

		[HttpGet]
		[Route("GetAllPost")]
		public async Task<string> GetAllPost()
		{
			return (await _postService.GetAllPost()).ToJson();
		}

		[HttpGet]
		[Route("GetPost")]
		public async Task<string> GetPost(string postID)
		{
			return (await _postService.GetPost(postID)).ToJson();
		}


		[HttpPost]
		[Route("GetPostInfo")]
		public async Task<string> GetPostInfo()
		{
			Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
			return (await _postService.GetPostInfo(post)).ToJson();
		}

		#endregion

		#region Select

		[HttpPost]
		[Route("SelectAuthorInfo")]
		public async Task<string> SelectAuthorInfo()
		{
			Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
			return (await _postService.SelectAuthorInfo(post)).ToJson();
		}


		[HttpPost]
		[Route("SelectTags")]
		public async Task<string> SelectTags()
		{
			Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
			return (await _postService.SelectTags(post)).ToJson();
		}

		[HttpPost]
		[Route("SelectLikeCollectCommentCount")]
		public async Task<string> SelectLikeCollectCommentCount()
		{
			Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
			return (await _postService.SelectLikeCollectCommentCount(post)).ToJson();
		}

		[HttpPost]
		[Route("SelectIsLikeOrCollect")]
		public async Task<string> SelectIsLikeOrCollect()
		{
			Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
			var user = HttpContext.Request.Form["User"].ToObject<User>();
			return (await _postService.SelectIsLikeOrCollect(post, user)).ToJson();
		}

		[HttpPost]
		[Route("SelectComments")]
		public async Task<string> SelectComments()
		{
			Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
			return (await _postService.SelectComments(post)).ToJson();
		}

		[HttpPost]
		[Route("SelectAFormedCommentAndUser")]
		public async Task<string> SelectAFormedCommentAndUser()
		{
			var comment = HttpContext.Request.Form["Comment"].ToObject<Comment>();
			var user = HttpContext.Request.Form["User"].ToObject<User>();
			return (await _postService.SelectAFormedCommentAndUser(comment, user)).ToJson();
		}

		[HttpPost]
		[Route("SelectFormedCommentsAndUser")]
		public async Task<string> SelectFormedCommentsAndUser()
		{
			var post = HttpContext.Request.Form["Post"].ToObject<Post>();
			var user = HttpContext.Request.Form["User"].ToObject<User>();
			return (await _postService.SelectFormedCommentsAndUser(post, user)).ToJson();
		}

		#endregion

		#region Change



		[HttpPost]
		[Route("InsertPost")]
		public async Task<string> InsertPost()
		{

			if (!HttpContext.Request.Form.TryGetValue("Author", out var Author) ||
			!HttpContext.Request.Form.TryGetValue("Title", out var Title) ||
			!HttpContext.Request.Form.TryGetValue("Focus", out var Focus) ||
			!HttpContext.Request.Form.TryGetValue("Summary", out var Summary) ||
			!HttpContext.Request.Form.TryGetValue("Content", out var Content) ||
			!HttpContext.Request.Form.TryGetValue("TagIDs", out var TagIDs))
				return false.ToJson();


			return (await _postService.InsertPost(
				Author.ToObject<User>(),
				Title.ToObject<string>(),
				Focus.ToObject<string>(),
				Summary.ToObject<string>(),
				Content.ToObject<string>(),
				TagIDs.ToObject<IList<long>>())).ToJson();
		}

		[HttpPost]
		[Route("InsertAComment")]
		public async Task<string> InsertAComment()
		{
			if (!HttpContext.Request.Form.TryGetValue("User", out var User)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("Post", out var Post)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("CommentText", out var CommentText)) return "bad request".ToJson();
			_logger.LogDebug($"Comment String: {CommentText.ToString()}");
			return (await _postService.InsertAComment(
				User.ToObject<User>(),
				Post.ToObject<Post>(),
				CommentText.ToObject<string>()
			)).ToJson();
		}

		[HttpPost]
		[Route("InsertACommentReply")]
		public async Task<string> InsertACommentReply()
		{
			if (!HttpContext.Request.Form.TryGetValue("User", out var User)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("Post", out var Post)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("ReplyComment", out var ReplyComment)) return "bad request".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("CommentText", out var CommentText)) return "bad request".ToJson();

			return (await _postService.InsertACommentReply(
			User.ToObject<User>(),
			Post.ToObject<Post>(), 
			ReplyComment.ToObject<Comment>(),
			CommentText.ToObject<string>()
			)).ToJson();
		}

		#endregion
	}
}