using System;
using System.Threading.Tasks;
using ApiServer.Models.User;
using Kamanri.Database;
using Kamanri.Extensions;
using Kamanri.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace ApiServer.Controllers
{



	[Controller]
	[Route("Auth/")]
	public class AuthController : Controller
	{
		private DatabaseContext _dbc;


		private readonly ILogger<AuthController> _logger;


		public AuthController(ILoggerFactory loggerFactory,DatabaseContext dbc)
		{
			_logger = loggerFactory.CreateLogger<AuthController>();
			_dbc = dbc;
		}

		[HttpPost]
		[Route("GetAuthCode")]
		public string GetAuthCode()
		{
			if (!HttpContext.Request.Form.TryGetValue("Account", out var accountJson)) return "-2".ToJson();
			try
			{
				//获取随机验证码
				string authCode = RandomGenerator.GenerateAuthCode().ToString();
				string account = accountJson.ToObject<string>();

				_logger.LogDebug($"Account : {account}");
				//发送验证码
				EmailHelper.SendThread(account.ToString(), "验证码", authCode);


				return authCode.ToJson();
			}
			catch (Exception)
			{
				return "-1".ToJson();
			}

		}
		[HttpPost]
		[Route("InfoSummit")]
		public async Task<string> InfoSummit()
		{
			if (!HttpContext.Request.Form.TryGetValue("userID", out var userIDJson)) return "-2".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("nickname", out var nicknameJson)) return "-2".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("realname", out var realnameJson)) return "-2".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("university", out var universityJson)) return "-2".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("school", out var schoolJson)) return "-2".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("speciality", out var specialityJson)) return "-2".ToJson();
			if (!HttpContext.Request.Form.TryGetValue("schoolyear", out var schoolyearJson)) return "-2".ToJson();


			UserInfo userInfo = new UserInfo(Convert.ToInt64(userIDJson), nicknameJson, realnameJson, universityJson,
				schoolJson, specialityJson, JsonConvert.DeserializeObject<DateTime>(schoolyearJson), "", "/StaticFiles/Images/HeadImage/syt5.com.1638601045.jpg");
			var id = Convert.ToInt64(userIDJson);
			_logger.LogInformation($"Insert {userInfo.ToJson()}");
			if((await _dbc.Select(new UserInfo(id))) == null)
				await _dbc.InsertWithID(userInfo);
			else await _dbc.Update(userInfo);
			return "1";

		}
	}
}