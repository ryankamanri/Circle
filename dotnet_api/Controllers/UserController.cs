using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnetApi.Model;
using dotnetApi.Services;

namespace dotnetApi.Controller
{
    [ApiController]
    [Route("User/")]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        #region Get
            
            [HttpPost]
            [Route("GetUserInfo")]
            public async Task<IActionResult> GetUserInfo()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                return new JsonResult(await _userService.GetUserInfo(user));
            }

        #endregion

        #region Select
            
            [HttpPost]
            [Route("SelectTag")]
            public async Task<IActionResult> SelectTag()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selection"]);
                return new JsonResult(await _userService.SelectTag(user,selections => 
                {
                    foreach(var option in options)
                    {
                        ((IDictionary<string,object>)selections)[option.Key] = option.Value;
                    }
                }));
            }

            [HttpPost]
            [Route("SelectPost")]
            public async Task<IActionResult> SelectPost()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selection"]);
                return new JsonResult(await _userService.SelectPost(user,selections => 
                {
                    foreach(var option in options)
                    {
                        ((IDictionary<string,object>)selections)[option.Key] = option.Value;
                    }
                }));
            }

            [HttpPost]
            [Route("SelectUserInitiative")]
            public async Task<IActionResult> SelectUserInitiative()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selection"]);
                return new JsonResult(await _userService.SelectUserInitiative(user,selections => 
                {
                    foreach(var option in options)
                    {
                        ((IDictionary<string,object>)selections)[option.Key] = option.Value;
                    }
                }));
            }

            [HttpPost]
            [Route("SelectUserPassive")]
            public async Task<IActionResult> SelectUserPassive()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selection"]);
                return new JsonResult(await _userService.SelectUserPassive(user,selections => 
                {
                    foreach(var option in options)
                    {
                        ((IDictionary<string,object>)selections)[option.Key] = option.Value;
                    }
                }));
            }
            
        #endregion


        #region Mapping
            
            [HttpPost]
            [Route("MappingPostsByTag")]
            public async Task<IActionResult> MappingPostsByTag()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selection"]);
                return new JsonResult(await _userService.MappingPostsByTag(user,selections => 
                {
                    foreach(var option in options)
                    {
                        ((IDictionary<string,object>)selections)[option.Key] = option.Value;
                    }
                }));
            }
        #endregion

        #region JudgeRelation

            [HttpPost]
            [Route("IsUserRelationExist")]
            public async Task<IActionResult> IsUserRelationExist()
            {
                User keyUser = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["KeyUser"]);
                User valueUser = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["ValueUser"]);
                string relationName = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["RelationName"]);
                string relationValue = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["RelationValue"]);
                return new JsonResult(await _userService.IsUserRelationExist(keyUser,valueUser,relationName,relationValue));
            }
            
        #endregion


        #region ChangeRelation

            [HttpPost]
            [Route("AppendFocusUser")]
            public async Task<IActionResult> AppendFocusUser()
            {
                User yourself = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["Yourself"]);
                User focusUser = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["FocusUser"]);
                string userRelation = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["UserRelation"]);
                await _userService.AppendFocusUser(yourself,focusUser,userRelation);
                return new JsonResult(true);
            }

            [HttpPost]
            [Route("RemoveFocusUser")]
            public async Task<IActionResult> RemoveFocusUser()
            {
                User yourself = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["Yourself"]);
                User focusUser = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["FocusUser"]);
                string userRelation = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["UserRelation"]);
                await _userService.RemoveFocusUser(yourself,focusUser,userRelation);
                return new JsonResult(true);
            }
        

        #endregion


        #region Carculate

            [HttpPost]
            [Route("CarculateSimilarity")]
            public async Task<IActionResult> CarculateSimilarity()
            {
                User user_1 = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User_1"]);
                User user_2 = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User_2"]);
                
                var myTagsType = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["MyTagsType"]);
                return new JsonResult(await _userService.CarculateSimilarity(user_1,user_2,selections => 
                {
                    foreach(var option in myTagsType)
                    {
                        ((IDictionary<string,object>)selections)[option.Key] = option.Value;
                    }
                }));
            }

            [HttpPost]
            [Route("CarculateSimilarityFix")]
            public async Task<IActionResult> CarculateSimilarityFix()
            {
                User user_1 = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User_1"]);
                User user_2 = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User_2"]);
                
                var myTagsType = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["MyTagsType"]);
                return new JsonResult(await _userService.CarculateSimilarityFix(user_1,user_2,selections => 
                {
                    foreach(var option in myTagsType)
                    {
                        ((IDictionary<string,object>)selections)[option.Key] = option.Value;
                    }
                }));
            }
        #endregion
    }
}