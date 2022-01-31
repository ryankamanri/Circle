using Kamanri.Database.Models;
using Kamanri.Extensions;
using System;
using System.Data.Common;

namespace WebViewServer.Models.Post
{
	public class Comment : EntityObject
	{
		// �����ĸ����ӻ��ĸ����۵�����
		public long PostID { get; set; }
		public long CommentID { get; set; }
		public string Content { get; set; }
		public DateTime CommentDateTime { get; set; }

		public Comment(long ID): base(ID) { }
	}
}