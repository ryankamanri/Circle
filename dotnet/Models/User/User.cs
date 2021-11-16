
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Dynamic;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Kamanri.Database.Models;


namespace dotnet.Models
{
    public class User : EntityView,IEqualityComparer<User>
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

        public User(string Account,string Password)
        {
            this.Account = Account;
            this.Password = Password;
        }
        public User(long ID,string account,string password) : base(ID)
        {
            this.Account = account;
            this.Password = password;
        }

        

        public override string ToString()
        {
            return $"{ID} , '{Account}' , '{Password}' ";
        }


        public bool Equals(User user_1,User user_2)
        {
            return user_1.ID == user_2.ID;
        }

        public int GetHashCode(User user)
        {
            return (int)user.ID;
        }


    }
}
