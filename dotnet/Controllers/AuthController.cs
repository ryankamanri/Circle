using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using dotnet.Model;
using dotnet.Services.Cookie;
using dotnet.Services.Database;
using dotnet.Services.Extensions;


namespace dotnet.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        private ICookie _cookie;
        private DataBaseContext _dbc;

        private User _user;

        //保存账号跟验证码
        private Dictionary<string,string> account_authCode;
        public AuthController(DataBaseContext dbc,ICookie cookie,Dictionary<string, string> account_authCode)
        {
            _dbc = dbc;
            _cookie = cookie;
            _user = new User();
            this.account_authCode = account_authCode;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpGet]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            return View();
        }

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

                return new JsonResult("auth succeed");
            }catch(Exception)
            {
                return new JsonResult("auth failure");
            }
            
        }

        [HttpPost]
        [Route("SignInSubmit")]
        public async Task<IActionResult> SignInSubmit()
        {
            StringValues authInfo = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("User[]",out authInfo) == false) return new JsonResult("bad request");

            string account = authInfo[0];
            string password = authInfo[1];
            string authCode = authInfo[2];

            if(account_authCode.Count == 0 ) return new JsonResult("none");

            if(account_authCode[account] != authCode) return new JsonResult("auth code error");

            if(_dbc.SelectID(new Model.User(account,"")) != long.MinValue) return new JsonResult("account exist");
            

            //验证成功

            User newUser = new User(account,password);

            //await newUser.Insert(_sql);
            await _dbc.Insert<User>(newUser);

            var userClaims = new List<Claim>()
            {
                new Claim("ID",newUser.ID.ToString()),
                new Claim("Account",account)
            };

            var userIdentity = new ClaimsIdentity(userClaims,"user");

            var userIdentities = new List<ClaimsIdentity>()
            {
                userIdentity
            };

            var claimsPrincipal = new ClaimsPrincipal(userIdentities);

            _cookie.SetCookie(HttpContext,claimsPrincipal);

            return new JsonResult("signin succeed");
        }


        [HttpPost]
        [Route("LogInSubmit")]
        public IActionResult LogInSubmit()
        {
            string account = "",password = "";
            StringValues authInfo = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("User[]",out authInfo) == false) return new JsonResult("bad request");

            account = authInfo[0];
            password = authInfo[1];

            User user = new User(account);

            _dbc.SelectID(user);

            user = _dbc.Select(user);

            if(user == default)
                return new JsonResult("account not found");

            if(user.Password != password)
                return new JsonResult("password is error");

            var userClaims = new List<Claim>()
            {
                new Claim("ID",user.ID.ToString()),
                new Claim("Account",account)
            };

            var userIdentity = new ClaimsIdentity(userClaims,"user");

            var userIdentities = new List<ClaimsIdentity>(){userIdentity};

            var claimsPrincipal = new ClaimsPrincipal(userIdentities);

            _cookie.SetCookie(HttpContext,claimsPrincipal);

            return new JsonResult("login succeed");
        }


        [HttpPost]
        [Route("LogOutSubmit")]
        public IActionResult LogOutSubmit()
        {
            _cookie.DeleteCookie(HttpContext);
            return new JsonResult("logout succeed");
        }

        [HttpGet]
        [Route("GetCookie")]
        public string GetCookie()
        {
            string result = "";

            ClaimsPrincipal claimsPrincipal =  _cookie.GetCookie(HttpContext);

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