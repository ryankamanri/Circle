using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Kamanri.Extensions;
using ApiServer.Services;

namespace ApiServer.Controllers
{
	[ApiController]
	[Route("Search")]
	public class SearchController : ControllerBase
	{
		private SearchService _searchService;
		public SearchController(SearchService searchService)
		{
			_searchService = searchService;
		}

		[HttpGet]
		[Route("SearchUserInfoAndTags")]
		public async Task<string> SearchUserInfoAndTags(string searchString)
		{
			return (await _searchService.SearchUserInfoAndTags(searchString)).ToJson();
		}

		[HttpGet]
		[Route("SearchPosts")]
		public async Task<string> SearchPosts(string searchString)
		{
			return (await _searchService.SearchPosts(searchString)).ToJson();
		}


	}
}