
using System.Collections.Generic;
using Kamanri.Database.Models;


namespace WebViewServer.Models.User
{
	public class User : EntityObject, IEqualityComparer<User>
	{
		public string Account { get; set; }
		public string Password { get; set; }


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
			return $"{ID} , '{Account}' , '{Password}' ";
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
