using System.Data.Common;
using Kamanri.Database.Models;
namespace dotnetDataSide.Models
{
    public class Circle : Entity<Circle>
    {
        public string Name { get; set; }

        public override string TableName { get ; set ; } = "circles";

        public Circle(){}
        public Circle(long ID) : base(ID){}
        public Circle(string name) => this.Name = name;

        public Circle(long ID, string name) : base(ID) => this.Name = name;

        public override string ColumnNamesString() => $"{TableName}.Name";

        public override string InsertValuesString() => $"{Name}";

        public override string CandidateKeySelectionString() => $"{TableName}.Name = {Name}";

        public override string UpdateSetString() => $"{TableName}.Name = {Name}";

        public override Circle GetEntityFromDataReader(DbDataReader msdr) => new Circle((long)msdr["ID"], (string)msdr["Name"]);


    }
}