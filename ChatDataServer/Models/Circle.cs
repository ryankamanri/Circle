using System.Data.Common;
using Kamanri.Database.Models;
namespace ChatDataServer.Models
{
	public class Circle : Entity<Circle>
	{
		public string Name { get; set; }

		public override string TableName { get; set; } = "circles";

		public Circle() { }
		public Circle(long ID) : base(ID) { }
		public Circle(string name) => Name = name;

		public Circle(long ID, string name) : base(ID) => Name = name;

		public override Circle GetEntityFromDataReader(DbDataReader ddr) => new Circle((long)ddr["ID"], (string)ddr["Name"]);

		public override Circle GetEntity() => this;



	}
}