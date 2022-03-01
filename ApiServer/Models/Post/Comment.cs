using System;
using System.Data.Common;
using Kamanri.Database.Models;
using Kamanri.Database.Models.Attributes;
using Kamanri.Extensions;

namespace ApiServer.Models.Post
{
	public class Comment : Entity<Comment>
	{
		// �����ĸ����ӻ��ĸ����۵�����

		public long PostID { get; set; }

		public long CommentID { get; set; }

		public string Content { get; set; }
		[CandidateKeyIgnore]
		public DateTime CommentDateTime { get; set; }
		public override string TableName { get; set; } = "comments";

		public override Comment GetEntity() => this;

		public override Comment GetEntityFromDataReader(DbDataReader ddr) =>
			new Comment()
			{
				PostID = (long)ddr["PostID"],
				CommentID = (long)ddr["CommentID"],
				Content = (string)ddr["Content"],
				CommentDateTime = DateTime.Parse((string)ddr["CommentDateTime"])
			};

	}

	
}