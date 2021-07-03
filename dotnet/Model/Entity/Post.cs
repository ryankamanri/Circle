using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Services.Database;
using dotnet.Services.Extensions;


namespace dotnet.Model
{
    public class Post : Entity<Post>
    {
        public long ID { get; set; }
        public string _Post { get; set; }
        

        public Post(){}

        public Post(string post)
        {
            this.ID = RandomGenerator.GenerateID();
            this._Post = post;
        }
        public Post(long ID,string post)
        {
            this.ID = ID;
            this._Post = post;
        }

        public override string ToString()
        {
            return $"{ID}  {_Post} ";
        }

        public override async Task Insert(SQL sql)
        {
            await sql.Execute($"insert into posts (ID,post) values({ID},'{_Post}')");
        }

        public override async Task Delete(SQL sql)
        {
            await sql.Execute($"delete from posts where ID = {ID}");
        }

        public override async Task Update(SQL sql)
        {
            await sql.Execute($"update posts set post = '{_Post}' where ID = {ID}");
        }
        public override IList<Post> GetList(MySqlDataReader msdr)
        {
            IList<Post> Posts = new List<Post>();
            while (msdr.Read())
            {
                Posts.Add(new Post((long)msdr["ID"], (string)msdr["post"]));
            }
            msdr.Close();
            return Posts;
        }

        public override Post Select(SQL sql, long ID)
        {
            IList<Post> result = GetList(sql.Query($"select * from posts where ID = '{ID}'"));
            if(result.Count == 0) return null;
            this.ID = result[0].ID;
            this._Post = result[0]._Post;
            return result[0];
        }

        public override IList<Post> Selects(SQL sql ,IEnumerable<long> IDs)
        {
            IList<Post> posts = new List<Post>();
            Post post;
            foreach(var ID in IDs)
            {
                post = Select(sql,ID);
                if(post != null) posts.Add(post);
            }
            return posts;
        }

        public override long SelectID(SQL sql, Post entity)
        {
            throw new System.NotImplementedException();
        }

        public override List<long> SelectIDs(SQL sql, List<Post> entities)
        {
            throw new System.NotImplementedException();
        }

    }
}
