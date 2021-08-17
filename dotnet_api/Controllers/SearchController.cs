using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnetApi.Model;
using dotnetApi.Services;

namespace dotnetApi.Controller
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
        public async Task<IActionResult> SearchUserInfoAndTags(string searchString)
        {
            return new JsonResult(await _searchService.SearchUserInfoAndTags(searchString));
        }

        [HttpGet]
        [Route("SearchPosts")]
        public async Task<IActionResult> SearchPosts(string searchString)
        {
            return new JsonResult(await _searchService.SearchPosts(searchString));
        }


    }
}