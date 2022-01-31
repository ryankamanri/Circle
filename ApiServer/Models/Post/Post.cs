using System;
using System.Data.Common;
using System.Collections.Generic;
using Kamanri.Database.Models;
using Kamanri.Extensions;

namespace ApiServer.Models.Post
{
	public class Post : Entity<Post>, IEqualityComparer<Post>
	{
		/// <summary>
		/// 标题
		/// </summary>
		/// <value>string</value>
		public string Title { get; set; }

		/// <summary>
		/// 摘要
		/// </summary>
		/// <value>string</value>
		public string Summary { get; set; }

		/// <summary>
		/// 聚焦
		/// </summary>
		/// <value>string</value>
		public string Focus { get; set; }

		/// <summary>
		/// 发布日期
		/// </summary>
		/// <value>DateTime</value>
		public DateTime PostDateTime { get; set; }



		public override string TableName { get; set; } = "posts";


		public Post()
		{

		}

		public Post(long ID) : base(ID)
		{

		}

		public Post(string Title, string Summary, string Focus, DateTime PostDateTime)
		{
			this.Title = Title;
			this.Summary = Summary;
			this.Focus = Focus;
			this.PostDateTime = PostDateTime;
		}
		public Post(long ID, string Title, string Summary, string Focus, DateTime PostDateTime) : base(ID)
		{
			this.Title = Title;
			this.Summary = Summary;
			this.Focus = Focus;
			this.PostDateTime = PostDateTime;
		}


		public override Post GetEntityFromDataReader(DbDataReader ddr)
		{
			return new Post((long)ddr["ID"], (string)ddr["Title"], (string)ddr["Summary"], (string)ddr["Focus"], DateTime.Parse((string)ddr["PostDateTime"]));
		}

		public override Post GetEntity()
		{
			return this;
		}

		public bool Equals(Post post_1, Post post_2)
		{
			return post_1.ID == post_2.ID;
		}

		public int GetHashCode(Post post)
		{
			return (int)post.ID;
		}


	}
}
