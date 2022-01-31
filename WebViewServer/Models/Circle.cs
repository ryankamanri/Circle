using Kamanri.Database.Models;

namespace WebViewServer.Models
{
	public class Circle : EntityObject
	{
		public string Name { get; set; }


		public Circle() { }
		public Circle(long ID) : base(ID) { }
		public Circle(string name) => Name = name;

		public Circle(long ID, string name) : base(ID) => Name = name;



	}
}