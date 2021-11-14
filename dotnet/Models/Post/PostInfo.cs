using System.Data.Common;
using Kamanri.Database.Models;

namespace dotnet.Models
{
    public class PostInfo : Entity<PostInfo>
    {
        public string Content { get; set; }

        public override string TableName { get; set; } = "postsinfo";

        public override string ColumnNamesString() => $"{TableName}.Content";

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

        public override string InsertValuesString()
        {
            return $"'{Content}'";
        }

        public override string UpdateSetString()
        {
            return $"{TableName}.Content = '{Content}'";
        }

        public override string CandidateKeySelectionString()
        {
            return $"{TableName}.Content = '{Content}'";
        }

        public override PostInfo GetEntityFromDataReader(DbDataReader msdr)
        {
            return new PostInfo((long)msdr["ID"],(string)msdr["Content"]);
        }
    }
}

