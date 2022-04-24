using System;


namespace MLServer.Models
{
    public class ClusterUserDataItem
    {
        // between every user and tag
        public long UserID { get; set; }

        public float IsSelf { get; set; } = 0;
        
        public float IsInterested { get; set; } = 0;
        
        public float IsPostOwned { get; set; } = 0;
        
        public float IsPostLike { get; set; } = 0;
        
        public float IsPostCollect { get; set; } = 0;
        
        public float PostCommentCount { get; set; }

        public long TagID { get; set; }

        public float Count { get; set; } = 1;

        public static string HeaderString()
        {
            return "UserID\t" +
                   "TagID\t" +
                   "IsInterested\t" +
                   "IsSelf\t" +
                   "IsPostOwned\t" +
                   "IsPostLike\t" +
                   "IsPostCollect\t" +
                   "PostCommentCount\t" +
                   "Count\t";
        }

        public override string ToString()
        {
            return $"{UserID}\t" +
                   $"{TagID}\t" +
                   $"{IsInterested}\t\t" +
                   $"{IsSelf}\t" +
                   $"{IsPostOwned}\t\t" +
                   $"{IsPostLike}\t\t" +
                   $"{IsPostCollect}\t\t" +
                   $"{PostCommentCount}\t\t" +
                   $"{Count}\t";

        }
    }
}