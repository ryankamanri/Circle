using System.Threading.Tasks;
using ApiServer.Models.Post;
using Kamanri.Database;
using Kamanri.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
	[Controller]
	[Route("test/")]
	public class TestController
	{
		private readonly DatabaseContext _dbc;
		public TestController(DatabaseContext dbc)
		{
			_dbc = dbc;
		}
		[HttpGet]
		[Route("1")]
		public async Task<string> Test1()
		{
		
			return new Comment()
			{
				ID = 1,
				PostID = 2,
				CommentID = 3,
				CommentDateTime = System.DateTime.Now
			}.EntityAttributesString.Build().ToJson();
		}
	}
}
