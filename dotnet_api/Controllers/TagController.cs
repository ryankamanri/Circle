using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnetApi.Model;
using dotnetApi.Services;

namespace dotnetApi.Controller
{
    [ApiController]
    [Route("Tag/")]
    public class TagController : ControllerBase
    {
        private TagService _tagService;
        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [Route("TagIndex")]
        public IActionResult TagIndex(string indexString)
        {
            if(!_tagService.TagIndex.ContainsKey(indexString)) return new JsonResult(new List<Tag>());
            return new JsonResult(_tagService.TagIndex[indexString]);
        }

        [HttpPost]
        [Route("FindChildTag")]
        public IActionResult FindChildTag()
        {
            Tag parentTag = JsonConvert.DeserializeObject<Tag>(HttpContext.Request.Form["ParentTag"]);
            return new JsonResult(_tagService.FindChildTag(parentTag));
        }

        [HttpPost]
        [Route("FindParentTag")]
        public IActionResult FindParentTag()
        {
            Tag childTag = JsonConvert.DeserializeObject<Tag>(HttpContext.Request.Form["ChildTag"]);
            return new JsonResult(_tagService.FindParentTag(childTag));
        }

        [HttpPost]
        [Route("CalculateSimilarity")]
        public IActionResult CalculateSimilarity()
        {
            Tag tag_1 = JsonConvert.DeserializeObject<Tag>(HttpContext.Request.Form["tag_1"]);
            Tag tag_2 = JsonConvert.DeserializeObject<Tag>(HttpContext.Request.Form["tag_2"]);
            return new JsonResult(_tagService.CalculateSimilarity(tag_1,tag_2));
        }
    }
}