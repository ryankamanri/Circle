using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnetApi.Models;
using dotnetApi.Services;
using Kamanri.Extensions;

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
        public string TagIndex(string indexString)
        {
            if(!_tagService.TagIndex.ContainsKey(indexString)) return (new List<Tag>()).ToJson();
            return (_tagService.TagIndex[indexString]).ToJson();
        }

        [HttpPost]
        [Route("FindChildTag")]
        public string FindChildTag()
        {
            Tag parentTag =  HttpContext.Request.Form["ParentTag"].ToObject<Tag>();
            return (_tagService.FindChildTag(parentTag)).ToJson();
        }

        [HttpPost]
        [Route("FindParentTag")]
        public string FindParentTag()
        {
            Tag childTag = HttpContext.Request.Form["ChildTag"].ToObject<Tag>();
            return (_tagService.FindParentTag(childTag)).ToJson();
        }

        [HttpPost]
        [Route("CalculateSimilarity")]
        public string CalculateSimilarity()
        {
            Tag tag_1 = HttpContext.Request.Form["tag_1"].ToObject<Tag>();
            Tag tag_2 = HttpContext.Request.Form["tag_2"].ToObject<Tag>();
            return (_tagService.CalculateSimilarity(tag_1,tag_2)).ToJson();
        }
    }
}