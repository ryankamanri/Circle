using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using dotnetApi.Model;
using dotnetApi.Services;
using dotnetApi.Services.Self;

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
            [Route("GetUser")]
            public IActionResult GetUser()
            {
                string account = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["Account"]);
                return new JsonResult(_userService.GetUser(account));
            }

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
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selections"]);
                return new JsonResult(await _userService.SelectTag(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("SelectPost")]
            public async Task<IActionResult> SelectPost()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selections"]);
                return new JsonResult(await _userService.SelectPost(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("SelectUserInitiative")]
            public async Task<IActionResult> SelectUserInitiative()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selections"]);
                return new JsonResult(await _userService.SelectUserInitiative(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("SelectUserPassive")]
            public async Task<IActionResult> SelectUserPassive()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selections"]);
                return new JsonResult(await _userService.SelectUserPassive(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }
            
        #endregion


        #region Mapping
            
            [HttpPost]
            [Route("MappingPostsByTag")]
            public async Task<IActionResult> MappingPostsByTag()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selections"]);
                return new JsonResult(await _userService.MappingPostsByTag(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }
        #endregion

        #region Change
            
            [HttpPost]
            [Route("InsertUser")]
            public async Task<IActionResult> InsertUser()
            {
                User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
                return new JsonResult(await _userService.InsertUser(user));
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AppendRelation")]
        public async Task<IActionResult> AppendRelation()
        {
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
            string entityType = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["EntityType"]);
            string ID = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["ID"]);
            string relationName = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["RelationName"]);
            string relation = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["Relation"]);

            if(!await _userService.AppendRelation(user,entityType,Convert.ToInt64(ID),relationName,relation)) return new JsonResult(false);
            

            return new JsonResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveRelation")]
        public async Task<IActionResult> RemoveRelation()
        {
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User"]);
            string entityType = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["EntityType"]);
            string ID = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["ID"]);
            string relationName = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["RelationName"]);
            string relation = JsonConvert.DeserializeObject<string>(HttpContext.Request.Form["Relation"]);

            if(!await _userService.RemoveRelation(user,entityType,Convert.ToInt64(ID),relationName,relation)) return new JsonResult(false);
            

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
                
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selections"]);
                return new JsonResult(await _userService.CarculateSimilarity(user_1,user_2,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("CarculateSimilarityFix")]
            public async Task<IActionResult> CarculateSimilarityFix()
            {
                User user_1 = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User_1"]);
                User user_2 = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["User_2"]);
                
                var options = JsonConvert.DeserializeObject<Dictionary<string,object>>(HttpContext.Request.Form["Selections"]);
                return new JsonResult(await _userService.CarculateSimilarityFix(user_1,user_2,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }
        #endregion
    }
}