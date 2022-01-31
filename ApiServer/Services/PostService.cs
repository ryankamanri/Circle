
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Kamanri.Database;
using Kamanri.Database.Models.Relation;
using Kamanri.Http;
using ApiServer.Models.User;
using ApiServer.Models;
using ApiServer.Models.Post;

namespace ApiServer.Services
{
	public class PostService
	{
		private DatabaseContext _dbc;

		private UserService _userService;

		private const string TYPE_POST = "Post";

		private const string TYPE_COMMENT = "Comment";
        public PostService(DatabaseContext dbc, UserService userService)
		{
			_dbc = dbc;
			_userService = userService;
		}

		#region Get

		public async Task<IList<Post>> GetAllPost()
		{
			return await _dbc.SelectAll<Post>();
		}

		public async Task<PostInfo> GetPostInfo(Post post)
		{
			return await _dbc.Select(new PostInfo(post.ID));
		}

		#endregion

		#region Select


		public async Task<UserInfo> SelectAuthorInfo(Post post)
		{
			// 这儿要修改成根据relation = {Type:["Owned"]}来寻找作者
			ICollection<User> authorCollection = (await _dbc.MappingSelect<Post, User>(
				post, 
				ID_IDList.OutPutType.Key, 
				selection => selection.Type = new List<string>() { "Owned" }
			));
			IEnumerator<User> userEnumerator = authorCollection.GetEnumerator();
			if (!userEnumerator.MoveNext()) return new UserInfo();
			User user = userEnumerator.Current;
			return await _userService.GetUserInfo(user);

		}

		public async Task<ICollection<Tag>> SelectTags(Post post)
		{
			return (await _dbc.Mapping<Post, Tag>(post, ID_IDList.OutPutType.Value)).Keys;
		}

		public async Task<Form> SelectLikeCollectCommentCount(Post post)
		{
			if (post == null) return default;
			var likeCount = (await _dbc.MappingSelect<Post, User>(
				post, ID_IDList.OutPutType.Key, 
				selection => selection.Type = new List<string>() { "Like" }
			)).Count;
			var collectCount = (await _dbc.MappingSelect<Post, User>(
				post, ID_IDList.OutPutType.Key,
				selection => selection.Type = new List<string>() { "Collect" }
			)).Count;
			var commentCount = (await _dbc.SelectCustom<Comment>($"PostID = {post.ID}")).Count;
			return new Form()
			{
				{ "LikeCount", likeCount },
				{ "CollectCount", collectCount },
				{ "CommentCount", commentCount }
			};
		}

		public async Task<Form> SelectCommentLikeCount(Comment comment)
		{
			if (comment == null) return default;
			var likeCount = (await _dbc.MappingSelect<Comment, User>(
				comment, ID_IDList.OutPutType.Key,
				selection => selection.Type = new List<string>() { "Like" }
			)).Count;

			return new Form()
			{
				{ "LikeCount", likeCount }
			};
		}

		

		public async Task<Form> SelectIsLikeOrCollect(Post post, User user)
		{
			var isLike = await _userService.IsEntityRelationExist(user, TYPE_POST, post.ID, "Type", "Like");
			var isCollect = await _userService.IsEntityRelationExist(user, TYPE_POST, post.ID, "Type", "Collect");

			return new Form()
			{
				{ "IsLike", isLike.ToString() },
				{ "IsCollect", isCollect.ToString() }
			};

		}


		public async Task<IList<Comment>> SelectComments(Post post)
		{
			return await _dbc.SelectCustom<Comment>($"PostID = {post.ID}");
        }

		public async Task<UserInfo> SelectCommentOwnerInfo(Comment comment)
		{
			var user = (await _dbc.MappingSelect<Comment, User>(comment, ID_IDList.OutPutType.Key,
				selection => selection.Type = new List<string>() { "Owned" })).FirstOrDefault();
			return await _dbc.Select(new UserInfo()
			{
				ID = user.ID
			});
		}

		public async Task<UserInfo> SelectCommentReplyUserInfo(Comment comment)
		{
			var replyComment = (await _dbc.SelectCustom<Comment>($"ID = {comment.CommentID}")).FirstOrDefault();
			return await SelectCommentOwnerInfo(replyComment);
		}

		public async Task<Key_ListValue_Pairs<KeyValuePair<Comment, Form>, KeyValuePair<Comment, Form>>> SelectFormedCommentsAndUser(Post post, User user)
		{
			var allComments = await _dbc.SelectCustom<Comment>($"PostID = {post.ID}");

			var commentFormedDict = new Key_ListValue_Pairs<KeyValuePair<Comment, Form>, KeyValuePair<Comment, Form>>();
			
			var firstLevelComments = from comment in allComments 
				where comment.CommentID == -1
				select comment;

			foreach(var flComment in firstLevelComments)
			{
				IList<Comment> secondLevelComments = new List<Comment>();
				RecursiveSearchComments(allComments, flComment, ref secondLevelComments);
				
				var secondLevelComment_info_Pairs = new List<KeyValuePair<Comment, Form>>();
				foreach (var slComment in secondLevelComments)
				{
					var slOwnerInfo = await SelectCommentOwnerInfo(slComment);
					var slReplyUserInfo = await SelectCommentReplyUserInfo(slComment);
					var slComment_info = new KeyValuePair<Comment, Form>(slComment, new Form()
					{
						{ "OwnerHeadImage", slOwnerInfo.HeadImage },
						{ "OwnerNickName", slOwnerInfo.NickName },
						{ "ReplyUserHeadImage", slReplyUserInfo.HeadImage },
						{ "ReplyUserNickName", slReplyUserInfo.NickName }
					});
					secondLevelComment_info_Pairs.Add(slComment_info);
				}

				var ownedUserInfo = await SelectCommentOwnerInfo(flComment);
				var likeCount = (await SelectCommentLikeCount(flComment))["LikeCount"];
				var replyCount = secondLevelComments.Count();
				var isLike = await _userService.IsEntityRelationExist(user, TYPE_COMMENT, flComment.ID, "Type", "Like");
				var comment_info = new KeyValuePair<Comment, Form>(flComment, new Form()
				{
					{ "OwnerHeadImage", ownedUserInfo.HeadImage },
					{ "OwnerNickName", ownedUserInfo.NickName },
					{ "LikeCount",  likeCount },
					{ "ReplyCount",  replyCount },
					{ "IsLike", isLike.ToString() }
				});

				commentFormedDict.Add(new KeyValuePair<KeyValuePair<Comment, Form>, IList<KeyValuePair<Comment, Form>>>(comment_info, secondLevelComment_info_Pairs));
            }

			return commentFormedDict;
		}

		public void RecursiveSearchComments(IList<Comment> comments, Comment firstCommend, ref IList<Comment> originList)
		{
			var searchedComments = from comment in comments
				where comment.CommentID == firstCommend.ID
				select comment;
			foreach(var comment in searchedComments)
			{
				originList.Add(comment);
				RecursiveSearchComments(comments, comment, ref originList);
            }
        }
		#endregion

		#region Change

		/// <summary>
		/// 保存帖子
		/// </summary>
		/// <param name="author"></param>
		/// <param name="title"></param>
		/// <param name="focus"></param>
		/// <param name="content"></param>
		/// <param name="tagIDs"></param>
		/// <returns></returns>
		public async Task<bool> InsertPost(User author, string title, string focus, string summary, string content, IList<long> tagIDs)
		{
			//1. 保存帖子信息
			// string
			Post post = new Post(title, summary, focus, DateTime.Now);
			await _dbc.Insert(post);
			//2. 获取帖子ID in 数据库
			long ID = await _dbc.SelectID(post);
			//3. 保存帖子内容
			PostInfo postInfo = new PostInfo(ID, content);
			await _dbc.InsertWithID(postInfo);
			//4. 保存帖子与标签关系
			foreach (var tagID in tagIDs)
			{
				await _dbc.AppendRelation(post, new Tag(tagID), "Type", "Owned"); // 设计为添加一个Type关系, 而不是直接建立关系
			}

			//5. 保存帖子与作者关系

			// await _dbc.Connect<User,Post>(author,post,relations => relations.Type = "Owned");
			await _dbc.AppendRelation(author, post, "Type", "Owned");
			return true;

		}

		public async Task<bool> InsertAComment(User user, Post post, string commentText)
		{
			var comment = new Comment()
			{
				PostID = post.ID,
				CommentID = -1,
				Content = commentText,
				CommentDateTime = DateTime.Now
			};
			await _dbc.Insert(comment);
			var commentID = await _dbc.SelectID(comment); 
			comment.ID = commentID;
			await _dbc.AppendRelation(user, comment, "Type", "Owned");
			return true;
		}
		public async Task<bool> InsertACommentReply(User user, Post post, Comment replyComment, string commentText)
		{
			var comment = new Comment()
			{
				PostID = post.ID,
				CommentID = replyComment.ID,
				Content = commentText,
				CommentDateTime = DateTime.Now
			};
			await _dbc.Insert(comment);
			var commentID = await _dbc.SelectID(comment);
			comment.ID = commentID;
			await _dbc.AppendRelation(user, comment, "Type", "Owned");
			return true;
		}
		#endregion
	}
}