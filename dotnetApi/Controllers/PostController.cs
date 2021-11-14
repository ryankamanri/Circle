using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnetApi.Models;
using dotnetApi.Services;
using Kamanri.Extensions;

namespace dotnetApi.Controller
{
    [ApiController]
    [Route("Post/")]
    public class PostController : ControllerBase
    {
        

        private PostService _postService;
        public PostController(PostService postService)
        {
            _postService = postService;
        }

        #region Get
        
        [HttpGet]
        [Route("GetAllPost")]
        public async Task<string> GetAllPost()
        {
            return (await _postService.GetAllPost()).ToJson();
        }


        [HttpPost]
        [Route("GetPostInfo")]
        public async Task<string> GetPostInfo()
        {
            Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
            return (await _postService.GetPostInfo(post)).ToJson();
        }
            
        #endregion

        #region Select

        [HttpPost]
        [Route("SelectAuthorInfo")]
        public async Task<string> SelectAuthorInfo()
        {
            Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
            return (await _postService.SelectAuthorInfo(post)).ToJson();
        }
        [HttpPost]
        [Route("SelectTags")]
        public async Task<string> SelectTags()
        {
            Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
            return (await _postService.SelectTags(post)).ToJson();
        }

        #endregion

        [HttpPost]
        [Route("InsertPost")]
        public async Task<string> InsertPost()
        {
            StringValues Author = new StringValues(), Title = new StringValues(), Focus = new StringValues(),Summary = new StringValues(), Content = new StringValues(),TagIDs = new StringValues();

            if (!HttpContext.Request.Form.TryGetValue("Author", out Author) || 
            !HttpContext.Request.Form.TryGetValue("Title", out Title) ||
            !HttpContext.Request.Form.TryGetValue("Focus", out Focus) ||
            !HttpContext.Request.Form.TryGetValue("Summary", out Summary) ||
            !HttpContext.Request.Form.TryGetValue("Content", out Content) ||
            !HttpContext.Request.Form.TryGetValue("TagIDs", out TagIDs)) 
            return (false).ToJson();


            return (await _postService.InsertPost(
                Author.ToObject<User>(),
                Title.ToObject<string>(),
                Focus.ToObject<string>(), 
                Summary.ToObject<string>(),
                Content.ToObject<string>(),
                TagIDs.ToObject<IList<long>>())).ToJson();
        }
    }
}