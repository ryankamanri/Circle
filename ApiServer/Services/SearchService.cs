using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Kamanri.Database;
using Kamanri.Database.Models;
using Kamanri.Database.Models.Relation;
using ApiServer.Models.User;
using ApiServer.Models;
using ApiServer.Models.Post;

namespace ApiServer.Services
{
	public class SearchService
	{
		private DatabaseContext _dbc;

		private UserService _userService;

		public SearchService(DatabaseContext dbc, UserService userService)
		{
			_dbc = dbc;
			_userService = userService;
		}

		public async Task<IList<Post>> SearchPost(string searchString)
		{
			if (searchString == "" || searchString == default) return new List<Post>();
			searchString = searchString.Replace("\'", "\\\'").Replace(" ", "%");
			return await _dbc.SelectCustom<Post>(
			$"Title like '%{searchString}%' or Summary like '%{searchString}%' or Focus like '%{searchString}%'");
		}

		public async Task<IList<Tag>> SearchTag(string searchString)
		{
			if (searchString == "" || searchString == default) return new List<Tag>();
			searchString = searchString.Replace("\'", "\\\'").Replace(" ", "%");
			return await _dbc.SelectCustom<Tag>($"tag like '%{searchString}%'");
		}

		public async Task<IList<UserInfo>> SearchUserInfo(string searchString)
		{
			if (searchString == "" || searchString == default) return new List<UserInfo>();
			searchString = searchString.Replace("\'", "\\\'").Replace(" ", "%");
			return await _dbc.SelectCustom<UserInfo>(
			$"NickName like '%{searchString}%' or RealName like '%{searchString}%' or University like '%{searchString}%' or School like '%{searchString}%' or Speciality like '%{searchString}%' or Introduction like '%{searchString}%'");
		}

		/// <summary>
		/// 根据用户基本信息和用户拥有的个人标签搜索用户
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		public async Task<Key_ListValue_Pairs<UserInfo, Tag>> SearchUserInfoAndTags(string searchString)
		{
			if (searchString == "" || searchString == default) return new Key_ListValue_Pairs<UserInfo, Tag>();

			var userInfosBase = await SearchUserInfo(searchString);

			var searchedTags = await SearchTag(searchString) as List<Tag>;

			var tagsGroupByUser = await _dbc.MappingSelectUnionStatistics<Tag, User>(
				searchedTags.ConvertAll(tag => tag as Entity<Tag>), ID_IDList.OutPutType.Key,
				selection => selection.Type = new List<string>() { "Utils" });

			Key_ListValue_Pairs<UserInfo, Tag> tagsGroupByUserInfo = new Key_ListValue_Pairs<UserInfo, Tag>();
			Key_ListValue_Pairs<UserInfo, Tag> tagsGroupByUserInfoBase = new Key_ListValue_Pairs<UserInfo, Tag>();
			foreach (var user_tags in tagsGroupByUser)
			{
				tagsGroupByUserInfo.Add(new KeyValuePair<UserInfo, IList<Tag>>(await _userService.GetUserInfo(user_tags.Key), user_tags.Value));
			}

			foreach (var userInfo in userInfosBase)
			{
				tagsGroupByUserInfoBase.Add(new KeyValuePair<UserInfo, IList<Tag>>(userInfo, new List<Tag>()));
			}

			return tagsGroupByUserInfo.Union(tagsGroupByUserInfoBase);

		}

		/// <summary>
		/// 根据帖子基本信息和帖子拥有的标签搜索帖子
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		public async Task<IList<Post>> SearchPosts(string searchString)
		{
			if (searchString == "" || searchString == default) return new List<Post>();

			IList<Post> postsBase = await SearchPost(searchString);

			var searchedTags = await SearchTag(searchString) as List<Tag>;


			var tagRelationsGroupByPost = await _dbc.MappingUnionStatistics<Tag, Post>(
				searchedTags.ConvertAll(tag => tag as Entity<Tag>), ID_IDList.OutPutType.Key);

			var postsFindByTag = tagRelationsGroupByPost.Keys;



			return postsBase.Union(postsFindByTag, new Post()).ToList();

		}



	}
}