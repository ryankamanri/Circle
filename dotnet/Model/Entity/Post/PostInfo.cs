using System.Data.Common;
using Kamanri.Database.Model;

namespace dotnet.Model
{
    public class PostInfo : Entity<PostInfo>
    {
        public string Content { get; set; }

        public override string TableName { get; set; } = "postsinfo";

        public override string ColumnsWithoutID() => $"{TableName}.Content";

        public PostInfo(){}

        public PostInfo(long ID) : base(ID){}

        public PostInfo(string Content)
        {
            this.Content = Content;
        }

        public PostInfo(long ID,string Content) : base(ID)
        {
            this.Content = Content;
        }

        public override string InsertString()
        {
            return $"'{Content}'";
        }

        public override string UpdateString()
        {
            return $"{TableName}.Content = '{Content}'";
        }

        public override string SelectString()
        {
            return $"{TableName}.Content = '{Content}'";
        }

        public override PostInfo GetEntityFromDataReader(DbDataReader msdr)
        {
            return new PostInfo((long)msdr["ID"],(string)msdr["Content"]);
        }
    }
}

