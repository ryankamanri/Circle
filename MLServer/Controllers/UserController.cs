using System;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using Microsoft.AspNetCore.Mvc;
using MLServer.Models.BasicModels.User;
using MLServer.Services;

namespace MLServer.Controllers
{
    [Controller]
    [Route("User/")]
    public class UserController: Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Match")]
        public async Task<string> Match()
        {
            if (!HttpContext.Request.Form.TryGetValue("userID", out var userID)) return "Bad Request".ToJson();
            return (await _userService.Match(userID)).ToJson();
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return new AcceptedResult();
        }
        
    }
}