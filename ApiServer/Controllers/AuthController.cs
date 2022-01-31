using System;
using Kamanri.Extensions;
using Kamanri.Self;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace ApiServer.Controllers
{
	[Controller]
	[Route("Auth/")]
	public class AuthController : Controller
	{
		private readonly ILogger<AuthController> _logger;


		public AuthController(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<AuthController>();
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
	}
}