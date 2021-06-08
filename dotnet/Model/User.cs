using MySql.Data.MySqlClient;
using System.Collections.Generic;
using dotnet.Services;

namespace dotnet.Model
{
    public class User
    {
        public long ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }

        public User(long ID,string account,string password)
        {
            this.ID = ID;
            this.Account = account;
            this.Password = password;
        }

        public override string ToString()
        {
            return $"{ID}  {Account}  {Password} ";
        }

        public static IList<User> GetList(MySqlDataReader msdr)
        {
            IList<User> users = new List<User>();
            while (msdr.Read())
            {
                users.Add(new User((long)msdr["ID"], (string)msdr["Account"], (string)msdr["Password"]));
            }
            msdr.Close();
            return users;
        }

        public static User Find(SQL sql, long ID)
        {
            return GetList(sql.Query($"select * from users where ID = '{ID}'"))[0];
        }

        public static long FindID(SQL sql, string account,string password)
        {
            return GetList(sql.Query($"select * from users where Account = '{account}' and Password = '{password}'"))[0].ID;
        }

        public static IList<User> Finds(SQL sql ,IEnumerable<long> IDs)
        {
            IList<User> users = new List<User>();
            foreach(var ID in IDs)
            {
                users.Add(Find(sql,ID));
            }
            return users;
        }

    }
}
