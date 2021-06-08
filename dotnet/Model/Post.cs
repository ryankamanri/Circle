using MySql.Data.MySqlClient;
using System.Collections.Generic;
using dotnet.Services;

namespace dotnet.Model
{
    public class Post
    {
        public long ID { get; set; }
        public string _Post { get; set; }
        

        public Post(long ID,string post)
        {
            this.ID = ID;
            this._Post = post;
        }

        public override string ToString()
        {
            return $"{ID}  {_Post} ";
        }

        public static IList<Post> GetList(MySqlDataReader msdr)
        {
            IList<Post> Posts = new List<Post>();
            while (msdr.Read())
            {
                Posts.Add(new Post((long)msdr["ID"], (string)msdr["post"]));
            }
            msdr.Close();
            return Posts;
        }

        public static Post Find(SQL sql, long ID)
        {
            return GetList(sql.Query($"select * from posts where ID = '{ID}'"))[0];
        }

        public static IList<Post> Finds(SQL sql ,IEnumerable<long> IDs)
        {
            IList<Post> posts = new List<Post>();
            foreach(var ID in IDs)
            {
                posts.Add(Find(sql,ID));
            }
            return posts;
        }
    }
}
