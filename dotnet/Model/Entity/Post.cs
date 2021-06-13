using MySql.Data.MySqlClient;
using System.Collections.Generic;
using dotnet.Services;
using System.Threading.Tasks;

namespace dotnet.Model
{
    public class Post : IDataOperation
    {
        public long ID { get; set; }
        public string _Post { get; set; }
        

        public Post(){}
        public Post(long ID,string post)
        {
            this.ID = ID;
            this._Post = post;
        }

        public override string ToString()
        {
            return $"{ID}  {_Post} ";
        }

        public async Task Save(SQL sql)
        {
            await sql.Execute($"insert into posts (ID,post) values({ID},'{_Post}')");
        }

        public async Task Remove(SQL sql)
        {
            await sql.Execute($"delete from posts where ID = {ID}");
        }

        public async Task Modify(SQL sql)
        {
            await sql.Execute($"update posts set post = '{_Post}' where ID = {ID}");
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
            IList<Post> result = GetList(sql.Query($"select * from posts where ID = '{ID}'"));
            if(result.Count == 0) return null;
            return result[0];
        }

        public static IList<Post> Finds(SQL sql ,IEnumerable<long> IDs)
        {
            IList<Post> posts = new List<Post>();
            Post post;
            foreach(var ID in IDs)
            {
                post = Find(sql,ID);
                if(post != null) posts.Add(post);
            }
            return posts;
        }
    }
}
