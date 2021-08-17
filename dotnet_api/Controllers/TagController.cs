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

        [HttpPost]
        [Route("FindChildTag")]
        public IActionResult FindChildTag()
        {
            Tag parentTag = (Tag)JsonConvert.DeserializeObject(HttpContext.Request.Form["ParentTag"]);
            return new JsonResult(_tagService.FindChildTag(parentTag));
        }

        [HttpPost]
        [Route("FindParentTag")]
        public IActionResult FindParentTag()
        {
            Tag childTag = (Tag)JsonConvert.DeserializeObject(HttpContext.Request.Form["ChildTag"]);
            return new JsonResult(_tagService.FindChildTag(childTag));
        }

        [HttpPost]
        [Route("CalculateSimilarity")]
        public IActionResult CalculateSimilarity()
        {
            Tag tag_1 = (Tag)JsonConvert.DeserializeObject(HttpContext.Request.Form["tag_1"]);
            Tag tag_2 = (Tag)JsonConvert.DeserializeObject(HttpContext.Request.Form["tag_2"]);
            return new JsonResult(_tagService.CalculateSimilarity(tag_1,tag_2));
        }
    }
}