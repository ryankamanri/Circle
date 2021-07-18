using System;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnet.Model;
using dotnet.Model.Relation;
using dotnet.Services;
using dotnet.Services.Extensions;
using dotnet.Services.Database;
using dotnet.Services.Cookie;



namespace dotnet.Controllers
{
    [Controller]
    [Authorize]
    [Route("Shared/")]
    public class MappingController : Controller
    {
        private DataBaseContext _dbc;
        private SQL _sql;

        private TagService _tagService;

        private PostService _postService;

        private ICookie _cookie;
        public MappingController(SQL sql,DataBaseContext dbc,ICookie cookie,TagService tagService,PostService postService)
        {
            _sql = sql;
            _dbc = dbc;
            _cookie = cookie;
            _tagService = tagService;
            _postService = postService;
        }



        /// <summary>
        /// 搜索标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search()
        {
            string search = "";
            List<string> tagsString = new List<string>();
            
            StringValues searchInfo = new StringValues();
            if(HttpContext.Request.Form.TryGetValue("search",out searchInfo) == false) return new JsonResult("bad request");

            search = searchInfo;

            if(!_tagService.TagIndex.ContainsKey(search)) return new JsonResult(JsonConvert.SerializeObject(tagsString));
            
            var tagList = _tagService.TagIndex[search];

            
            foreach(var tag in tagList)
            {
                tagsString.Add(await this.RenderViewAsync<Tag>("Tag",tag,true));
            }
            var resultJSON = JsonConvert.SerializeObject(tagsString);
            return new JsonResult(resultJSON);
            
        }

        [HttpPost]
        [Route("ShowPostInfo")]
        public async Task<IActionResult> ShowPostInfo()
        {
            StringValues postID = new StringValues();
            if(!HttpContext.Request.Form.TryGetValue("postID",out postID)) return new JsonResult("bad request");

            PostInfo postInfo = await _postService.GetPostInfo(new Post(Convert.ToInt64(postID.ToString())));

            return new JsonResult(JsonConvert.SerializeObject(postInfo));
        }
    }
}