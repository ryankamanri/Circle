using System.Collections.Generic;
using System.Threading.Tasks;
using Kamanri.Http;
using WebViewServer.Models;

namespace WebViewServer.Services
{
	public class TagService
	{

		private Api _api;

		public TagService(Api api)
		{
			_api = api;
		}




		public async Task<List<Tag>> TagIndex(string indexString)
		{
			return await _api.Get<List<Tag>>($"/Tag/TagIndex?indexString={indexString}");
		}

		/// <summary>
		/// 寻找子标签
		/// </summary>
		/// <param name="parentTag"></param>
		/// <returns></returns>
		public async Task<ICollection<Tag>> FindChildTag(Tag parentTag)
		{
			return await _api.Post<ICollection<Tag>>("/Tag/FindChildTag", new Form()
			{
				{"ParentTag", parentTag}
			});
		}


		/// <summary>
		/// 寻找父标签
		/// </summary>
		/// <param name="childTag"></param>
		/// <returns></returns>
		public async Task<Tag> FindParentTag(Tag childTag)
		{
			//return TagDictionary[TagTree[childTag.ID]];
			return await _api.Post<Tag>("/Tag/FindParentTag", new Form()
			{
				{"ChildTag", childTag}
			});
		}


	}
}