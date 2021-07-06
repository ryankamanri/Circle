using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json;
using dotnet.Services.Database;
using dotnet.Services.Extensions;


namespace dotnet.Model
{
    public class Post : Entity<Post>
    {


        public string _Post { get; set; }

        private void Init()
        {
            TableName  = "posts";

            Columns = "posts.ID,post";

            ColumnsWithoutID = "post";
        }
        
        public Post()
        {
            Init();
        }

        public Post(long ID)
        {
            this.ID = ID;
            Init();
        }

        public Post(string post)
        {
            this._Post = post;
            Init();
        }
        public Post(long ID,string post)
        {
            this.ID = ID;
            this._Post = post;
            Init();
        }

        public override string ToString()
        {
            return $"{ID}  {_Post} ";
        }

        public override string InsertString()
        {
            return $"'{_Post}'";
        }

        public override string UpdateString()
        {
            return $"post = '{_Post}'";
        }

        public override string SelectString()
        {
            return $"post = '{_Post}'";
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

        public override IDictionary<Post,dynamic> GetRelationDictionary(MySqlDataReader msdr)
        {
            IDictionary<Post,dynamic> Post_Relations = new Dictionary<Post,dynamic>();
            while (msdr.Read())
            {
                Post_Relations.Add(new Post((long)msdr["ID"], (string)msdr["post"]),
                JsonConvert.DeserializeObject<ExpandoObject>((string)msdr["relations"]));
            }
            msdr.Close();
            return Post_Relations;
        }

    

    }
}
