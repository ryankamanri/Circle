
namespace dotnet.Model
{
    public class User : Entity<User>
    {
        public string Account { get; set; }
        public string Password { get; set; }

        public override string TableName { get ; set ;} = "users";



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

        



    }
}
