using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Kamanri.Utils;
using Kamanri.Extensions;
using ApiServer.Models.User;
using ApiServer.Services;

namespace ApiServer.Controllers
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
		public string GetUser()
		{
			string account = HttpContext.Request.Form["Account"].ToObject<string>();
			return _userService.GetUser(account).ToJson();
		}

		[HttpPost]
		[Route("GetUserInfo")]
		public async Task<string> GetUserInfo()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			return (await _userService.GetUserInfo(user)).ToJson();
		}

		#endregion

		#region Select

		[HttpPost]
		[Route("SelectTag")]
		public async Task<string> SelectTag()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string, object>>();
			return (await _userService.SelectTag(user, selections =>
			 {
				 Dynamic.Cover(options, selections);
			 })).ToJson();
		}

		[HttpPost]
		[Route("SelectPost")]
		public async Task<string> SelectPost()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string, object>>();
			return (await _userService.SelectPost(user, selections =>
			 {
				 Dynamic.Cover(options, selections);
			 })).ToJson();
		}

		[HttpPost]
		[Route("SelectUserInitiative")]
		public async Task<string> SelectUserInitiative()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string, object>>();
			return (await _userService.SelectUserInitiative(user, selections =>
			 {
				 Dynamic.Cover(options, selections);
			 })).ToJson();
		}

		[HttpPost]
		[Route("SelectUserPassive")]
		public async Task<string> SelectUserPassive()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string, object>>();
			return (await _userService.SelectUserPassive(user, selections =>
			 {
				 Dynamic.Cover(options, selections);
			 })).ToJson();
		}

		#endregion


		#region Mapping

		[HttpPost]
		[Route("MappingPostsByTag")]
		public async Task<string> MappingPostsByTag()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string, object>>();
			return (await _userService.MappingPostsByTag(user, selections =>
			 {
				 Dynamic.Cover(options, selections);
			 })).ToJson();
		}
		#endregion

		#region Change

		[HttpPost]
		[Route("InsertUser")]
		public async Task<string> InsertUser()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			return (await _userService.InsertUser(user)).ToJson();
		}

		[HttpPost]
		[Route("InsertOrUpdateUserInfo")]
		public async Task<string> InsertOrUpdateUserInfo()
		{
			UserInfo userInfo = HttpContext.Request.Form["UserInfo"].ToObject<UserInfo>();
			return (await _userService.InsertOrUpdateUserInfo(userInfo)).ToJson();
		}
		#endregion

		#region JudgeRelation

		[HttpPost]
		[Route("IsUserRelationExist")]
		public async Task<string> IsUserRelationExist()
		{
			User keyUser = HttpContext.Request.Form["KeyUser"].ToObject<User>();
			User valueUser = HttpContext.Request.Form["ValueUser"].ToObject<User>();
			string relationName = HttpContext.Request.Form["RelationName"].ToObject<string>();
			string relationValue = HttpContext.Request.Form["RelationValue"].ToObject<string>();
			return (await _userService.IsUserRelationExist(keyUser, valueUser, relationName, relationValue)).ToJson();
		}

		#endregion


		#region ChangeRelation

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("AppendRelation")]
		public async Task<string> AppendRelation()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			string entityType = HttpContext.Request.Form["EntityType"].ToObject<string>();
			string ID = HttpContext.Request.Form["ID"].ToObject<string>();
			string relationName = HttpContext.Request.Form["RelationName"].ToObject<string>();
			string relation = HttpContext.Request.Form["Relation"].ToObject<string>();

			if (!await _userService.AppendRelation(user, entityType, Convert.ToInt64(ID), relationName, relation)) return false.ToJson();


			return true.ToJson();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("RemoveRelation")]
		public async Task<string> RemoveRelation()
		{
			User user = HttpContext.Request.Form["User"].ToObject<User>();
			string entityType = HttpContext.Request.Form["EntityType"].ToObject<string>();
			string ID = HttpContext.Request.Form["ID"].ToObject<string>();
			string relationName = HttpContext.Request.Form["RelationName"].ToObject<string>();
			string relation = HttpContext.Request.Form["Relation"].ToObject<string>();

			if (!await _userService.RemoveRelation(user, entityType, Convert.ToInt64(ID), relationName, relation)) return false.ToJson();


			return true.ToJson();
		}




		#endregion


		#region Carculate

		[HttpPost]
		[Route("CarculateSimilarity")]
		public async Task<string> CarculateSimilarity()
		{
			User user_1 = HttpContext.Request.Form["User_1"].ToObject<User>();
			User user_2 = HttpContext.Request.Form["User_2"].ToObject<User>();

			var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string, object>>();
			return (await _userService.CarculateSimilarity(user_1, user_2, selections =>
			  {
				  Dynamic.Cover(options, selections);
			  })).ToJson();
		}

		[HttpPost]
		[Route("CarculateSimilarityFix")]
		public async Task<string> CarculateSimilarityFix()
		{
			User user_1 = HttpContext.Request.Form["User_1"].ToObject<User>();
			User user_2 = HttpContext.Request.Form["User_2"].ToObject<User>();

			var options = HttpContext.Request.Form["Selections"].ToObject<Dictionary<string, object>>();
			return (await _userService.CarculateSimilarityFix(user_1, user_2, selections =>
			  {
				  Dynamic.Cover(options, selections);
			  })).ToJson();
		}
		#endregion
	}
}