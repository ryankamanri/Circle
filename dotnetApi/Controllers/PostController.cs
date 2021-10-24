using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnetApi.Model;
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
        public async Task<IActionResult> GetAllPost()
        {
            return new JsonResult(await _postService.GetAllPost());
        }


        [HttpPost]
        [Route("GetPostInfo")]
        public async Task<IActionResult> GetPostInfo()
        {
            Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
            return new JsonResult(await _postService.GetPostInfo(post));
        }
            
        #endregion

        #region Select

        [HttpPost]
        [Route("SelectAuthorInfo")]
        public async Task<IActionResult> SelectAuthorInfo()
        {
            Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
            return new JsonResult(await _postService.SelectAuthorInfo(post));
        }
        [HttpPost]
        [Route("SelectTags")]
        public async Task<IActionResult> SelectTags()
        {
            Post post = HttpContext.Request.Form["Post"].ToObject<Post>();
            return new JsonResult(await _postService.SelectTags(post));
        }

        #endregion

        [HttpPost]
        [Route("InsertPost")]
        public async Task<IActionResult> InsertPost()
        {
            StringValues Author = new StringValues(), Title = new StringValues(), Focus = new StringValues(),Summary = new StringValues(), Content = new StringValues(),TagIDs = new StringValues();

            if (!HttpContext.Request.Form.TryGetValue("Author", out Author) || 
            !HttpContext.Request.Form.TryGetValue("Title", out Title) ||
            !HttpContext.Request.Form.TryGetValue("Focus", out Focus) ||
            !HttpContext.Request.Form.TryGetValue("Summary", out Summary) ||
            !HttpContext.Request.Form.TryGetValue("Content", out Content) ||
            !HttpContext.Request.Form.TryGetValue("TagIDs", out TagIDs)) 
            return new JsonResult(false);


            return new JsonResult(await _postService.InsertPost(
                Author.ToObject<User>(),
                Title.ToObject<string>(),
                Focus.ToObject<string>(), 
                Summary.ToObject<string>(),
                Content.ToObject<string>(),
                TagIDs.ToObject<IList<long>>()));
        }
    }
}