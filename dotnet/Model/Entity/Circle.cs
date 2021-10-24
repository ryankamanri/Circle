using System.Data.Common;
using Kamanri.Database.Model;
namespace dotnet.Model
{
    public class Circle : Entity<Circle>
    {
        public string Name { get; set; }

        public override string TableName { get ; set ; } = "circles";

        public Circle(){}
        public Circle(long ID) : base(ID){}
        public Circle(string name) => this.Name = name;

        public Circle(long ID, string name) : base(ID) => this.Name = name;

        public override string ColumnsWithoutID() => $"{TableName}.Name";

        public override string InsertString() => $"{Name}";

        public override string SelectString() => $"{TableName}.Name = {Name}";

        public override string UpdateString() => $"{TableName}.Name = {Name}";

        public override Circle GetEntityFromDataReader(DbDataReader msdr) => new Circle((long)msdr["ID"], (string)msdr["Name"]);


    }
}