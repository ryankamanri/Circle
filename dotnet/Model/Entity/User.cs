using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Services.Database;
using dotnet.Services.Extensions;


namespace dotnet.Model
{
    public class User : Entity<User>
    {
        public long ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }

        public User(){}

        public User(string Account,string Password)
        {
            this.ID = RandomGenerator.GenerateID();
            this.Account = Account;
            this.Password = Password;
        }
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

        public override async Task Insert(SQL sql)
        {
            await sql.Execute($"insert into users (ID,Account,Password) values({ID},'{Account}','{Password}')");
        }

        public override async Task Delete(SQL sql)
        {
            await sql.Execute($"delete from users where ID = {ID}");
        }

        public override async Task Update(SQL sql)
        {
            await sql.Execute($"update users set Account = '{Account}',Password = '{Password}' where ID = {ID}");
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

        public override User Select(SQL sql, long ID)
        {
            IList<User> result = GetList(sql.Query($"select * from users where ID = '{ID}'"));
            if(result.Count == 0) return null;
            this.ID = result[0].ID;
            this.Account = result[0].Account;
            this.Password = result[0].Password;
            return result[0];
        }

        public override long SelectID(SQL sql, User user)
        {
            IList<User> result = GetList(sql.Query($"select * from users where Account = '{user.Account}'"));
            if(result.Count == 0) return long.MinValue;
            this.ID = result[0].ID;
            return result[0].ID;
        }

        public override IList<User> Selects(SQL sql ,IEnumerable<long> IDs)
        {
            IList<User> users = new List<User>();
            User user;
            foreach(var ID in IDs)
            {
                user = Select(sql,ID);
                if(user != null) users.Add(user);
            }
            return users;
        }
        public override List<long> SelectIDs(SQL sql, List<User> users)
        {
            throw new System.NotImplementedException();
        }

    }
}
