using System.Data.Common;
using Kamanri.Database.Models;

namespace MLServer.Models.BasicModels.Post
{
	public class PostInfo : Entity<PostInfo>
	{
		public string Content { get; set; }

		public override string TableName { get; set; } = "postsinfo";

		public PostInfo() { }

		public PostInfo(long ID) : base(ID) { }

		public PostInfo(string Content)
		{
			this.Content = Content;
		}

		public PostInfo(long ID, string Content) : base(ID)
		{
			this.Content = Content;
		}


		public override PostInfo GetEntityFromDataReader(DbDataReader ddr)
		{
			return new PostInfo((long)ddr["ID"], (string)ddr["Content"]);
		}

		public override PostInfo GetEntity()
		{
			return this;
		}
	}
}

