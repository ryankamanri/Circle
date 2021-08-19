
namespace dotnet.Model
{
    public class PostInfo : Entity<PostInfo>
    {
        public string Content { get; set; }

        public override string TableName { get; set; } = "postsinfo";


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


    }
}

