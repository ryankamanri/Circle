using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kamanri.Database.Models.Relation;
using Kamanri.Extensions;
using Kamanri.Http;
using Microsoft.Extensions.Logging;
using WebViewServer.Models;
using WebViewServer.Models.Post;
using WebViewServer.Models.User;

namespace WebViewServer.Services
{
	public class PostService
	{

		private Api _api;
		private readonly TagService _tagService;
		private readonly ILogger<PostService> _logger;


		public PostService(Api api, TagService tagService, ILoggerFactory loggerFactory)
		{
			_api = api;
			_tagService = tagService;
			_logger = loggerFactory.CreateLogger<PostService>();
		}

		#region Get

		public async Task<IList<Post>> GetAllPost()
		{
			return await _api.Get<IList<Post>>("/Post/GetAllPost");
		}

		public async Task<Post> GetPost(string postID)
		{
			_logger.LogDebug($"Post ID = {postID}");
			return await _api.Get<Post>($"/Post/GetPost?postID={postID}");
		}

		public async Task<PostInfo> GetPostInfo(Post post)
		{
			return await _api.Post<PostInfo>("/Post/GetPostInfo", new Form()
			{
				{"Post", post}
			});
		}

		#endregion

		#region Select


		public async Task<UserInfo> SelectAuthorInfo(Post post)
		{

			return await _api.Post<UserInfo>("/Post/SelectAuthorInfo", new Form()
			{
				{"Post", post}
			});
		}

		public async Task<IList<Tag>> SelectTags(Post post)
		{
			return await _api.Post<IList<Tag>>("/Post/SelectTags", new Form()
			{
				{"Post", post}
			});
		}

		public async Task<Form> SelectLikeCollectCommentCount(Post post)
		{
			return await _api.Post<Form>("/Post/SelectLikeCollectCommentCount", new Form()
			{
				{"Post", post}
			});
		}

		public async Task<Form> SelectIsLikeOrCollect(Post post, User user)
		{
			return await _api.Post<Form>("/Post/SelectIsLikeOrCollect", new Form()
			{
				{"Post", post},
				{"User", user}
			});
		}

		public async Task<IList<Comment>> SelectComments(Post post)
		{
			return await _api.Post<IList<Comment>>("/Post/SelectComments", new Form()
			{
				{ "Post", post }
			});

        }

		public async Task<IList<Comment>> SelectReplyComments(Comment comment)
		{
			return await _api.Post<IList<Comment>>("/Post/SelectReplyComments", new Form()
			{
				{ "Comment", comment }
			});
		}

		public async Task<KeyValuePair<Comment, Form>> SelectAFormedCommentAndUser(Comment comment, User user)
		{
			return await _api.Post<KeyValuePair<Comment, Form>>("/Post/SelectAFormedCommentAndUser", new Form()
			{
				{"Comment", comment},
				{"User", user}
			});
		}

		public async Task<Key_ListValue_Pairs<KeyValuePair<Comment, Form>, KeyValuePair<Comment, Form>>> SelectFormedCommentsAndUser(Post post, User user)
		{
			return await _api.Post<Key_ListValue_Pairs<KeyValuePair<Comment, Form>, KeyValuePair<Comment, Form>>>("/Post/SelectFormedCommentsAndUser", new Form()
			{
				{ "Post", post },
				{ "User", user }
			});

		}


		#endregion

		#region Judge
		
		public struct CircleType
		{
			public bool Postgraduate { get; set; }
			public bool Employment { get; set; }
		}

		public async Task<CircleType> GetCircleType(Post post)
		{
			var tags = await SelectTags(post);
			var postgraduateTag = (await _tagService.TagIndex("考研")).FirstOrDefault();
			var employmentTag = (await _tagService.TagIndex("就业")).FirstOrDefault();
			var result = new CircleType()
			{
				Employment = false,
				Postgraduate = false
			};
			foreach (var tag in tags)
			{
				if ((await _tagService.FindAncestorTag(tag)).ID == postgraduateTag.ID)
					result.Postgraduate = true;
				if ((await _tagService.FindAncestorTag(tag)).ID == employmentTag.ID)
					result.Employment = true;
			}

			return result;
		}

		#endregion

		/// <summary>
		/// 保存帖子
		/// </summary>
		/// <param name="author"></param>
		/// <param name="title"></param>
		/// <param name="focus"></param>
		/// <param name="content"></param>
		/// <param name="tagIDs"></param>
		/// <returns></returns>
		public async Task<bool> InsertPost(User author, string title, string focus, string summary, string content, string tagIDs)
		{
			var TagIDs = tagIDs.ToObject<IList<long>>();
			return await _api.Post<bool>("/Post/InsertPost", new Form()
			{
				{"Author", author},
				{"Title", title},
				{"Focus", focus},
				{"Summary", summary},
				{"Content", content},
				{"TagIDs", TagIDs}
			});
		}
	}
}