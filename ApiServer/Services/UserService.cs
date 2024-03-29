using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using Kamanri.Database.Models;
using Kamanri.Database.Models.Relation;
using Kamanri.Database;
using ApiServer.Models.User;
using ApiServer.Models;
using ApiServer.Models.Post;

namespace ApiServer.Services
{
	public class UserService
	{
		private DatabaseContext _dbc;

		private TagService _tagService;

		private IDictionary<string, object> entityDictionary;

		public IList<User> Users { get; private set; }

		#region Init


		public UserService(DatabaseContext dbc, TagService tagService)
		{
			_dbc = dbc;
			_tagService = tagService;
			InitUsers().Wait();

			entityDictionary = new Dictionary<string, object>();
			entityDictionary.Add("Comment", new Comment());
			entityDictionary.Add("Post", new Post());
			entityDictionary.Add("PostInfo", new PostInfo());
			entityDictionary.Add("User", new User());
			entityDictionary.Add("UserInfo", new UserInfo());
			entityDictionary.Add("Tag", new Tag());
		}

		private async Task InitUsers()
		{
			Users = await _dbc.SelectAll<User>();
		}

		#endregion

		#region Get

		public User GetUser(string account)
		{
			return (from userItem in Users
					where userItem.Account == account
					select userItem).FirstOrDefault();
		}


		public async Task<UserInfo> GetUserInfo(User user)
		{
			var userInfo = await _dbc.Select(new UserInfo(user.ID));

			if (userInfo == default) return new UserInfo(user.ID);

			return userInfo;
		}

		#endregion

		#region Select


		/// <summary>
		/// 用户选择标签
		/// </summary>
		/// <param name="type"></param>
		/// <param name="SetSelections"></param>
		/// <returns></returns>
		public async Task<IList<Tag>> SelectTag(User user, Action<dynamic> SetSelections)
		{
			return await _dbc.MappingSelect<User, Tag>(user, ID_IDList.OutPutType.Value, SetSelections);
		}

		public async Task<IList<Post>> SelectPost(User user, Action<dynamic> SetSelections)
		{
			return await _dbc.MappingSelect<User, Post>(user, ID_IDList.OutPutType.Value, SetSelections);
		}

		public async Task<IList<Comment>> SelectComment(User user, Action<dynamic> SetSelections)
		{
			return await _dbc.MappingSelect<User, Comment>(user, ID_IDList.OutPutType.Value, SetSelections);
		}

		/// <summary>
		/// 选择你主动去采取行为的用户.例如关注,拉黑
		/// </summary>
		/// <param name="user"></param>
		/// <param name="SetSelections"></param>
		/// <returns></returns>
		public async Task<IList<User>> SelectUserInitiative(User user, Action<dynamic> SetSelections)
		{
			return await _dbc.MappingSelect<User, User>(user, ID_IDList.OutPutType.Value, SetSelections);
		}

		/// <summary>
		/// 选择对你采取行为的用户.例如关注,拉黑
		/// </summary>
		/// <param name="user"></param>
		/// <param name="SetSelections"></param>
		/// <returns></returns>
		public async Task<IList<User>> SelectUserPassive(User user, Action<dynamic> SetSelections)
		{
			return await _dbc.MappingSelect<User, User>(user, ID_IDList.OutPutType.Key, SetSelections);
		}

		#endregion


		#region Mapping


		public async Task<ICollection<Post>> MappingPostsByTag(User user, Action<dynamic> SetUserTagRelation)
		{
			var interestedTags = await _dbc.MappingSelect<User, Tag>(user, ID_IDList.OutPutType.Value, SetUserTagRelation) as List<Tag>;
			Key_ListValue_Pairs<Post, KeyValuePair<Tag, dynamic>> interestedPosts = await _dbc.MappingUnionStatistics<Tag, Post>(
				interestedTags.ConvertAll(tag => tag as Entity<Tag>),
				 ID_IDList.OutPutType.Key);
			return interestedPosts.Keys;
		}



		#endregion

		#region Change

		public async Task<long> InsertUser(User user)
		{
			await _dbc.Insert(user);
			user.ID = await _dbc.SelectID(user);
			Users.Add(user);
			return user.ID;
		}

		

		/// <summary>
		/// 如果用户信息不存在, 插入用户信息, 否则更新用户信息
		/// </summary>
		/// <param name="userInfo"></param>
		/// <returns></returns>
		public async Task<bool> InsertOrUpdateUserInfo(UserInfo userInfo)
		{
			var oldUserInfo = await _dbc.Select(userInfo);
			if (oldUserInfo == default) await _dbc.InsertWithID(userInfo);
			else await _dbc.Update(userInfo);
			return true;

		}


		#endregion

		#region JudgeRelation

		/// <summary>
		/// 判断某两个用户的特定关系是否存在
		/// </summary>
		/// <param name="keyUser"></param>
		/// <param name="valueUser"></param>
		/// <param name="relationName"></param>
		/// <param name="relationValue"></param>
		/// <returns></returns>
		public async Task<bool> IsUserRelationExist(User keyUser, User valueUser, string relationName, string relationValue)
		{
			IList<object> relationValueList = (IList<object>)await _dbc.SelectRelation(keyUser, valueUser, relationName);
			if (relationValueList == default) return false;
			return relationValueList.Contains(relationValue);
		}


		public async Task<bool> IsEntityRelationExist(User user, string entityType, long ID, string relationName, string relationValue)
		{
			entityDictionary.TryGetValue(entityType, out dynamic entity);
			if(entity == null) return false;
			entity.ID = Convert.ToInt64(ID);
			var relationValueList = (IList<object>)await _dbc.SelectRelation(user, entity, relationName);
			if (relationValueList == default) return false;
			return relationValueList.Contains(relationValue);
		}

		#endregion


		#region ChangeRelation

		#region Framework


		/// <summary>
		/// 新增本用户与实体之间的一个关系
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="tagRelation"></param>
		/// <returns></returns>
		public async Task<bool> AppendRelation(User user, string entityType, long ID, string relationName, string newRelation)
		{

			entityDictionary.TryGetValue(entityType, out dynamic entity);

			if (entity == null) return false;

			entity.ID = Convert.ToInt64(ID);

			await _dbc.AppendRelation(user, entity, relationName, newRelation);

			return true;
		}



		/// <summary>
		/// 移除本用户与实体之间的一个关系
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="tagRelation"></param>
		/// <returns></returns>
		public async Task<bool> RemoveRelation(User user, string entityType, long ID, string relationName, string oldRelation)
		{

			entityDictionary.TryGetValue(entityType, out dynamic entity);

			if (entity == null) return false;

			entity.ID = Convert.ToInt64(ID);

			await _dbc.RemoveRelation(user, entity, relationName, oldRelation);

			return true;
		}

		#endregion



		#endregion


		#region Carculate


		public async Task<double> CarculateSimilarity(User user1, User user2, Action<dynamic> SetMyTagsType)
		{
			IList<Tag> tagsOfU1 = await SelectTag(user1, SetMyTagsType);
			IList<Tag> tagsOfU2 = await SelectTag(user2, selection => selection.Type = new List<string> { "Self" });

			double tagSimilarity = 0, tagSimilarityMax = 0, tagSimilaritySum = 0;
			double tagCrossJoin = tagsOfU1.Count * tagsOfU2.Count;
			foreach (var tagOfU1 in tagsOfU1)
			{
				tagSimilarityMax = 0;
				foreach (var tagOfU2 in tagsOfU2)
				{
					tagSimilarity = _tagService.CalculateSimilarity(tagOfU1, tagOfU2);
					tagSimilarityMax = Math.Max(tagSimilarity, tagSimilarityMax);

				}
				tagSimilaritySum += tagSimilarityMax * tagsOfU2.Count;
			}

			return tagSimilaritySum / tagCrossJoin * (Math.Min(tagsOfU1.Count, tagsOfU2.Count) / (double)Math.Max(tagsOfU1.Count, tagsOfU2.Count));

		}


		public async Task<double> CarculateSimilarityFix(User user1, User user2, Action<dynamic> SetMyTagsType)
		{
			double result = await CarculateSimilarity(user1, user2, SetMyTagsType);

			return Math.Sqrt(result * (2 - result));//圆形修正相似度
		}
		#endregion
	}
}