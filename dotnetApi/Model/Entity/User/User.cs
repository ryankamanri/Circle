
using System.Collections.Generic;
using System.Data.Common;
using Kamanri.Database.Model;


namespace dotnetApi.Model
{
    public class User : Entity<User>,IEqualityComparer<User>
    {
        public string Account { get; set; }
        public string Password { get; set; }

        public override string TableName { get ; set ;} = "users";

        public override string ColumnsWithoutID() => $"{TableName}.Account,{TableName}.Password";


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

        public override string InsertString()
        {
            return $"'{Account}' , '{Password}'";
        }

        public override string UpdateString()
        {
            return $" {TableName}.Account = '{Account}' , {TableName}.Password = '{Password}'";
        }

        public override string SelectString()
        {
            return $" {TableName}.Account = '{Account}'";
        }



        public override User GetEntityFromDataReader(DbDataReader msdr)
        {
            return new User((long)msdr["ID"], (string)msdr["Account"], (string)msdr["Password"]);
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
