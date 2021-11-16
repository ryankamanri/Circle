using System.Data.Common;
using Kamanri.Database.Models;
namespace dotnet.Models
{
    public class Circle : EntityView
    {
        public string Name { get; set; }


        public Circle(){}
        public Circle(long ID) : base(ID){}
        public Circle(string name) => this.Name = name;

        public Circle(long ID, string name) : base(ID) => this.Name = name;



    }
}