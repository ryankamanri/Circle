using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace dotnet.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }

        public User(int ID,string account,string password)
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
                users.Add(new User((int)msdr["ID"], (string)msdr["Account"], (string)msdr["Password"]));
            }
            msdr.Close();
            return users;
        }
    }
}
