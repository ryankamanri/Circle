using MySql.Data.MySqlClient;
using System.Collections.Generic;
using dotnet.Services;
using System.Threading.Tasks;

namespace dotnet.Model
{
    public class User : IDataOperation
    {
        public long ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }

        public User(){}
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

        public async Task Save(SQL sql)
        {
            await sql.Execute($"insert into users (ID,Account,Password) values({ID},'{Account}','{Password}')");
        }

        public async Task Remove(SQL sql)
        {
            await sql.Execute($"delete from users where ID = {ID}");
        }

        public async Task Modify(SQL sql)
        {
            await sql.Execute($"update users set Account = '{Account}',Password = '{Password}' where ID = {ID}");
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
            IList<User> result = GetList(sql.Query($"select * from users where ID = '{ID}'"));
            if(result.Count == 0) return null;
            return result[0];
        }

        public static long FindID(SQL sql, string account)
        {
            IList<User> result = GetList(sql.Query($"select * from users where Account = '{account}'"));
            if(result.Count == 0) return long.MinValue;
            return result[0].ID;
        }

        public static IList<User> Finds(SQL sql ,IEnumerable<long> IDs)
        {
            IList<User> users = new List<User>();
            User user;
            foreach(var ID in IDs)
            {
                user = Find(sql,ID);
                if(user != null) users.Add(user);
            }
            return users;
        }

    }
}
