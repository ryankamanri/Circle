
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Dynamic;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using dotnet.Services.Database;
using dotnet.Services.Extensions;


namespace dotnet.Model
{
    public class User : Entity<User>
    {
        public string Account { get; set; }
        public string Password { get; set; }

        private void Init()
        {
            TableName = "users";

            Columns = "users.ID,Account,Password";

            ColumnsWithoutID = "Account,Password";
        }

        public User()
        {
            Init();
        }

        public User(long ID)
        {
            this.ID = ID;
            Init();
        }

        public User(string Account)
        {
            this.Account = Account;
            Init();
        }

        public User(string Account,string Password)
        {
            this.Account = Account;
            this.Password = Password;
            Init();
        }
        public User(long ID,string account,string password)
        {
            this.ID = ID;
            this.Account = account;
            this.Password = password;
            Init();
        }

        

        public override string ToString()
        {
            return $"{ID} , '{Account}' , '{Password}' ";
        }

        public override string InsertString()
        {
            return $"'{Account}' , '{Password}'";
        }

        public override string UpdateString()
        {
            return $" Account = '{Account}' , Password = '{Password}'";
        }

        public override string SelectString()
        {
            return $" Account = '{Account}'";
        }



        public override IList<User> GetList(MySqlDataReader msdr)
        {
            IList<User> users = new List<User>();
            while (msdr.Read())
            {
                users.Add(new User((long)msdr["ID"], (string)msdr["Account"], (string)msdr["Password"]));
            }
            msdr.Close();
            return users;
        }

        public override IDictionary<User,dynamic> GetRelationDictionary(MySqlDataReader msdr)
        {
            IDictionary<User,dynamic> User_Relations = new Dictionary<User,dynamic>();
            while (msdr.Read())
            {
                User_Relations.Add(new User((long)msdr["ID"], (string)msdr["Account"],(string)msdr["Password"]),
                JsonConvert.DeserializeObject<ExpandoObject>((string)msdr["relations"]));
            }
            msdr.Close();
            return User_Relations;
        }


    }
}
