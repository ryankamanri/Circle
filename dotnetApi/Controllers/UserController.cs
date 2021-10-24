using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using dotnetApi.Model;
using dotnetApi.Services;
using Kamanri.Self;
using Kamanri.Extensions;

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
                string account = HttpContext.Request.Form["Account"].ToObject<string>();
                return new JsonResult(_userService.GetUser(account));
            }

            [HttpPost]
            [Route("GetUserInfo")]
            public async Task<IActionResult> GetUserInfo()
            {
                User user = HttpContext.Request.Form["User"].ToObject<User>();
                return new JsonResult(await _userService.GetUserInfo(user));
            }

        #endregion

        #region Select
            
            [HttpPost]
            [Route("SelectTag")]
            public async Task<IActionResult> SelectTag()
            {
                User user = HttpContext.Request.Form["User"].ToObject<User>();
                var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string,object>>();
                return new JsonResult(await _userService.SelectTag(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("SelectPost")]
            public async Task<IActionResult> SelectPost()
            {
                User user = HttpContext.Request.Form["User"].ToObject<User>();
                var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string,object>>();
                return new JsonResult(await _userService.SelectPost(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("SelectUserInitiative")]
            public async Task<IActionResult> SelectUserInitiative()
            {
                User user = HttpContext.Request.Form["User"].ToObject<User>();
                var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string,object>>();
                return new JsonResult(await _userService.SelectUserInitiative(user,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("SelectUserPassive")]
            public async Task<IActionResult> SelectUserPassive()
            {
                User user = HttpContext.Request.Form["User"].ToObject<User>();
                var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string,object>>();
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
                User user = HttpContext.Request.Form["User"].ToObject<User>();
                var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string,object>>();
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
                User user = HttpContext.Request.Form["User"].ToObject<User>();
                return new JsonResult(await _userService.InsertUser(user));
            }
        #endregion

        #region JudgeRelation

            [HttpPost]
            [Route("IsUserRelationExist")]
            public async Task<IActionResult> IsUserRelationExist()
            {
                User keyUser = HttpContext.Request.Form["KeyUser"].ToObject<User>();
                User valueUser = HttpContext.Request.Form["ValueUser"].ToObject<User>();
                string relationName = HttpContext.Request.Form["RelationName"].ToObject<string>();
                string relationValue = HttpContext.Request.Form["RelationValue"].ToObject<string>();
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
            User user = HttpContext.Request.Form["User"].ToObject<User>();
            string entityType = HttpContext.Request.Form["EntityType"].ToObject<string>();
            string ID = HttpContext.Request.Form["ID"].ToObject<string>();
            string relationName = HttpContext.Request.Form["RelationName"].ToObject<string>();
            string relation = HttpContext.Request.Form["Relation"].ToObject<string>();

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
            User user = HttpContext.Request.Form["User"].ToObject<User>();
            string entityType = HttpContext.Request.Form["EntityType"].ToObject<string>();
            string ID = HttpContext.Request.Form["ID"].ToObject<string>();
            string relationName = HttpContext.Request.Form["RelationName"].ToObject<string>();
            string relation = HttpContext.Request.Form["Relation"].ToObject<string>();

            if(!await _userService.RemoveRelation(user,entityType,Convert.ToInt64(ID),relationName,relation)) return new JsonResult(false);
            

            return new JsonResult(true);
        }

            
        

        #endregion


        #region Carculate

            [HttpPost]
            [Route("CarculateSimilarity")]
            public async Task<IActionResult> CarculateSimilarity()
            {
                User user_1 = HttpContext.Request.Form["User_1"].ToObject<User>();
                User user_2 = HttpContext.Request.Form["User_2"].ToObject<User>();
                
                var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string,object>>();
                return new JsonResult(await _userService.CarculateSimilarity(user_1,user_2,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }

            [HttpPost]
            [Route("CarculateSimilarityFix")]
            public async Task<IActionResult> CarculateSimilarityFix()
            {
                User user_1 = HttpContext.Request.Form["User_1"].ToObject<User>();
                User user_2 = HttpContext.Request.Form["User_2"].ToObject<User>();
                
                var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string,object>>();
                return new JsonResult(await _userService.CarculateSimilarityFix(user_1,user_2,selections => 
                {
                    JsonDynamic.FillSelections(options,selections);
                }));
            }
        #endregion
    }
}