using System.Data.Common;
using Kamanri.Database.Models;

namespace dotnet.Models
{
    public class PostInfo : EntityView
    {
        public string Content { get; set; }

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

