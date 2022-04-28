using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using Kamanri.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebViewServer.Services;

namespace WebViewServer.Controllers
{
    [Controller]
    [Route("Upload")]
    public class UploadController : Controller
    {
        private readonly IConfiguration _config;

        private readonly UploadService _uploadService;
        
        public UploadController(IConfiguration config, UploadService uploadService)
        {
            _config = config;
            _uploadService = uploadService;
        }

        

        [HttpPost]
        [Route("PostImage")]
        public async Task<string> PostImage()
        {
            if (HttpContext.Request.Form.Files.Count == 0) 
                return new Form()
                {
                    {"Status","Failure"},
                    {"Info","Post Failed (No File)"}
                }.ToJson();
            var imageFile = HttpContext.Request.Form.Files[0];
            return (await _uploadService.UploadFile(
                imageFile, 
                UploadService.UploadFileType.IMAGE, 
                UploadService.ImageType.POST_IMAGE)).ToJson();
        }

        [HttpPost]
        [Route("HeadImage")]
        public async Task<string> HeadImage()
        {
            if (HttpContext.Request.Form.Files.Count == 0) 
                return new Form()
                {
                    {"Status","Failure"},
                    {"Info","Post Failed (No File)"}
                }.ToJson();
            var imageFile = HttpContext.Request.Form.Files[0];
            return (await _uploadService.UploadFile(
                imageFile, 
                UploadService.UploadFileType.IMAGE, 
                UploadService.ImageType.HEAD_IMAGE)).ToJson();
        }
        
        
    }
}