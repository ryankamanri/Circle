using System.IO;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using Kamanri.Utils;
using Microsoft.AspNetCore.Mvc;
using WebViewServer.Models.User;

namespace WebViewServer.Controllers
{
	[Route("test/")]
	[Controller]
	public class TestController : Controller
	{


		private User _user;

		private Api _api;



		public TestController(User user, Api api)
		{
			_user = user;
			_api = api;
		}

		// [HttpGet]
		// [Route("email")]
		// public IActionResult SendEmail(string emailto, string title, string body)
		// {
		// 	EmailHelper.SendThread(emailto, title, body);
		// 	return new JsonResult("OK");
		// }
		//
		// [HttpGet]
		// [Route("getapi")]
		// public async Task<string> GetApi()
		// {
		// 	return await _api.Get<string>("/api/get");
		// }
		//
		// [HttpGet]
		// [Route("postapi")]
		// public async Task<string> PostApi()
		// {
		// 	return await _api.Post<string>("/User/SelectPost",
		// 	new Form()
		// 	{
		// 		{"User",_user},
		// 		{"Selection",new Form()
		// 		{
		//
		// 		}}
		// 	});
		// }
		//
		// [HttpGet]
		// [Route("directory")]
		// public string GetDirectory()
		// {
		// 	return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "StaticFiles");
		// }
		//
		// [HttpGet]
		// [Route("getattr")]
		// public string GetAttr()
		// {
		// 	return _user.Get<string>("Account");
		// }

	}
}