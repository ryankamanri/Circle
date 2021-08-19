using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authentication;
using dotnet.Model;
using dotnet.Services;
using dotnet.Services.Cookie;
using dotnet.Services.Http;
using dotnet.Services.Self;


namespace dotnet.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        private ICookie _cookie;

        private Api _api;

        private User _user;

        private UserService _userService;

        //保存账号跟验证码
        private Dictionary<string,string> account_authCode;
        public AuthController(Api api,UserService userService,ICookie cookie,Dictionary<string, string> account_authCode)
        {
            _api = api;
            _cookie = cookie;
            _userService = userService;
            _user = new User();
            this.account_authCode = account_authCode;
        }

        #region Index
            
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View("Index");
        }

        #endregion

 
        #region LogIn

        [HttpGet]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [Route("LogInSubmit")]
        public async Task<IActionResult> LogInSubmit()
        {
            string account = "",password = "";
            StringValues authInfo = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("User[]",out authInfo) == false) return new JsonResult("错误的请求");

            account = authInfo[0];
            password = authInfo[1];


            User user = await _userService.GetUser(account);

            if(user == default)
                return new JsonResult("账号不存在");

            if(user.Password != password)
                return new JsonResult("账号或密码错误");

            var userClaims = new List<Claim>()
            {
                new Claim("ID",user.ID.ToString()),
                new Claim("Account",account)
            };

            var userIdentity = new ClaimsIdentity(userClaims,"user");

            var userIdentities = new List<ClaimsIdentity>(){userIdentity};

            var claimsPrincipal = new ClaimsPrincipal(userIdentities);

            await HttpContext.SignInAsync(claimsPrincipal);

            return new JsonResult("登录成功");
        }

            
        #endregion

        

        #region SignIn

         [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("GetAuthCode")]
        public IActionResult GetAuthCode()
        {
            StringValues authInfo = new StringValues();
            if(!HttpContext.Request.Form.TryGetValue("User[]",out authInfo)) return new JsonResult("bad request");
            string account = authInfo[0];
            try
            {
                //获取随机验证码
                string authCode = RandomGenerator.GenerateAuthCode().ToString();

                //发送验证码
                EmailHelper.SendThread(account,"验证码",authCode);


                //向字典中添加或更新cookie内容
                if(account_authCode.ContainsKey(account))
                    account_authCode[account] = authCode;
                account_authCode.Add(account,authCode);

                return new JsonResult("验证码发送成功");
            }catch(Exception)
            {
                return new JsonResult("验证码发送失败");
            }
            
        }

        [HttpPost]
        [Route("SignInSubmit")]
        public async Task<IActionResult> SignInSubmit()
        {
            StringValues authInfo = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("User[]",out authInfo) == false) return new JsonResult("错误的请求");

            string account = authInfo[0];
            string password = authInfo[1];
            string authCode = authInfo[2];
            string originAuthCode = default;


            if(!account_authCode.TryGetValue(account,out originAuthCode)) return new JsonResult("未发送验证码");

            if(originAuthCode != authCode) return new JsonResult("验证码错误");

            User user = await _userService.GetUser(account);

            if(user.ID != long.MinValue) return new JsonResult("账号已存在");

            

            //验证成功

            User newUser = new User(account,password);

            newUser.ID = await _userService.InsertUser(newUser);

            var userClaims = new List<Claim>()
            {
                new Claim("ID",newUser.ID.ToString()),
                new Claim("Account",account)
            };

            var userIdentity = new ClaimsIdentity(userClaims,"user");

            var userIdentities = new List<ClaimsIdentity>(){userIdentity};

            var claimsPrincipal = new ClaimsPrincipal(userIdentities);


            await HttpContext.SignInAsync(claimsPrincipal);

            return new JsonResult("注册成功");
        }
            
        #endregion

       

        #region SelectCircle

        [HttpGet]
        [Route("SelectCircle")]
        public IActionResult SelectCircle()
        {
            return View();
        }

        [HttpPost]
        [Route("SelectCircleSubmit")]
        public IActionResult SelectCircleSubmit()
        {
            StringValues Circle = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("Circle",out Circle) == false) return new JsonResult("bad request");

            _cookie.AppendCookie(HttpContext,"Circle",Circle);
            return new JsonResult("select circle succeed");
        }
            
        #endregion

        

        [HttpPost]
        [Route("LogOutSubmit")]
        public async Task<IActionResult> LogOutSubmit()
        {
            await HttpContext.SignOutAsync();
            return new JsonResult("logout succeed");
        }

        [HttpGet]
        [Route("GetCookie")]
        public string GetCookie()
        {
            string result = "";

            ClaimsPrincipal claimsPrincipal =  HttpContext.User;

            foreach (var i in claimsPrincipal.Identities)
            {
                foreach (var c in i.Claims)
                {
                    result += $"{c.Type} : {c.Value}\n";
                }
            }

            return result;
        }


        
    }
}