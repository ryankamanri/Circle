
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Dynamic;
using System.Data.Common;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Kamanri.Database.Models;
using Kamanri.Extensions;


namespace ChatDataServer.Models.User
{
	public class User : Entity<User>, IEqualityComparer<User>
	{
		public string Account { get; set; }
		public string Password { get; set; }

		public override string TableName { get; set; } = "users";


		public User()
		{

		}

		public User(long ID) : base(ID)
		{

		}

		public User(string Account)
		{
			this.Account = Account;
		}

		public User(string Account, string Password)
		{
			this.Account = Account;
			this.Password = Password;
		}
		public User(long ID, string account, string password) : base(ID)
		{
			Account = account;
			Password = password;
		}



		public override string ToString()
		{
			return $"(ID = {ID})";
		}


		public override User GetEntityFromDataReader(DbDataReader ddr)
		{
			return new User((long)ddr["ID"], (string)ddr["Account"], (string)ddr["Password"]);
		}

		public override User GetEntity()
		{
			return this;
		}

		public bool Equals(User user_1, User user_2)
		{
			return user_1.ID == user_2.ID;
		}

		public int GetHashCode(User user)
		{
			return (int)user.ID;
		}


	}
}
