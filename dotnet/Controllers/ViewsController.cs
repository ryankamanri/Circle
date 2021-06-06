using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using dotnet.Services;
namespace dotnet.Controllers
{
    [Controller]
    public class ViewsController : Controller
    {

        private ICookie _cookie;
        public ViewsController(ICookie cookie)
        {
            _cookie = cookie;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpGet]
        [Route("Home")]
        [Authorize]
        public IActionResult Home()
        {
            return View("Home");
        }

        [HttpGet]
        [Route("LogIn")]
        public void LogIn()
        {
            //Claim类似于身份证的某条内容，一条内容对应一条Claim.例如：民族：汉、籍贯：浙江杭州 此处用的是学校的学生证
            var schoolClaims = new List<Claim>()
              {
                  new Claim(ClaimTypes.Name,"李雷"),//姓名
                  new Claim(ClaimTypes.SerialNumber,"0001"),//学号
                  new Claim("Gender","男"),//性别
              };

            //Claim类似于身份证的某条内容，一条内容对应一条Claim.例如：民族：汉、籍贯：浙江杭州 此处用的是社会上的驾照
            var drivePass = new List<Claim>()
             {
                 new Claim(ClaimTypes.Name, "李雷"),//姓名
                 new Claim(ClaimTypes.SerialNumber, "浙A00000"),//车牌号
                 new Claim("Driver", "GoodJob"),//开车技术怎么样...随便写的
             };

            //ClaimsIdentity 类似于身份证、学生证。它是有一条或者多条Claim组合而成。这样就是组成了一个学生证和驾照
            var schoolIdentity = new ClaimsIdentity(schoolClaims, "school"); 
            var govIdentity = new ClaimsIdentity(drivePass, "gov");

            //claimsPrincipal相当于一个人，你可以指定这个人持有哪些ClaimsIdentity（证件）,我指定他持有schoolIdentity、govIdentity那么他就是
            //在学校里是学生，在社会上是一名好司机
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new[] { schoolIdentity, govIdentity });

            _cookie.SetCookie(HttpContext,claimsPrincipal);

            
        }


        [HttpGet]
        [Route("LogOut")]
        public void LogOut()
        {
            _cookie.DeleteCookie(HttpContext);
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