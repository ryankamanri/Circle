using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Models;
using Kamanri.Extensions;
using Kamanri.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet.Controllers
{
    [Controller]
    [Route("Api")]
    public class ApiController : Controller
    {
        private readonly User _user;

        private readonly Api _api;

        private readonly ILogger<ApiController> _logger;

        private readonly string BAD_REQUEST = "Bad Request";

        public ApiController(User user, Api api, ILoggerFactory loggerFactory)
        {
            _user = user;
            _api = api;
            _logger = loggerFactory.CreateLogger<ApiController>();
        }
        
        
        [HttpPost]
        [Route("User/GetSelfInfo")]
        public async Task<string> GetSelf()
        {
            var selfInfo = await _api.Post<UserInfo>("/User/GetUserInfo", new JsonObject()
            {
                { "User", _user }
            });

            return selfInfo.ToJson();
        }

        
        [HttpPost]
        [Route("User/GetUserInfo")]
        public async Task<string> GetUserInfo()
        {
            if (!HttpContext.Request.Form.ContainsKey("User")) return BAD_REQUEST;
            var user = HttpContext.Request.Form["User"];
            var userInfo = await _api.Post<UserInfo>("/User/GetUserInfo", new JsonObject()
            {
                { "User", user }
            });
            return userInfo.ToJson();
        }

        
        [HttpPost]
        [Route("User/SelectUserInfoInitiative")]
        public async Task<string> SelectUserInfoInitiative()
        {
            if (!HttpContext.Request.Form.ContainsKey("Selections")) return BAD_REQUEST;
            var selections = HttpContext.Request.Form["Selections"].ToObject<JsonObject>();
            var users = await _api.Post<List<User>>("/User/SelectUserInitiative", new JsonObject()
            {
                { "User", _user}, 
                { "Selections", selections }
            });

            var userInfoList = new List<UserInfo>();
            foreach (var user in users)
            {
                var userInfo = await _api.Post<UserInfo>("/User/GetUserInfo", new JsonObject()
                {
                    { "User", user }
                });
                userInfoList.Add(userInfo);
            }
            
            _logger.LogDebug($"Focus User Count : {userInfoList.Count}");
            return userInfoList.ToJson();
        }
    }
}