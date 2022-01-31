using System.Collections.Generic;
using System.Threading.Tasks;
using Kamanri.Database.Models.Relation;
using Kamanri.Http;
using WebViewServer.Models;
using WebViewServer.Models.Post;
using WebViewServer.Models.User;

namespace WebViewServer.Services
{
	public class SearchService
	{

		private Api _api;

		public SearchService(Api api)
		{
			_api = api;
		}


		/// <summary>
		/// 根据用户基本信息和用户拥有的个人标签搜索用户
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		public async Task<Key_ListValue_Pairs<UserInfo, Tag>> SearchUserInfoAndTags(string searchString)
		{
			return await _api.Get<Key_ListValue_Pairs<UserInfo, Tag>>(
				$"/Search/SearchUserInfoAndTags?searchString={searchString}"
			);

		}

		/// <summary>
		/// 根据帖子基本信息和帖子拥有的标签搜索帖子
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		public async Task<IList<Post>> SearchPosts(string searchString)
		{

			return await _api.Get<IList<Post>>(
				$"/Search/SearchPosts?searchString={searchString}"
			);

		}



	}
}