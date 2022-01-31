using Kamanri.Database.Models;
using Kamanri.Extensions;
using System;
using System.Data.Common;

namespace WebViewServer.Models.Post
{
	public class Comment : EntityObject
	{
		// 基于哪个帖子或哪个评论的评论
		public long PostID { get; set; }
		public long CommentID { get; set; }
		public string Content { get; set; }
		public DateTime CommentDateTime { get; set; }

		public Comment(long ID): base(ID) { }
	}
}