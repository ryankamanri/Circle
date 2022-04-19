using System;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using Kamanri.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebViewServer.Controllers
{
    [Controller]
    [Route("Upload")]
    public class UploadController : Controller
    {
        private readonly IConfiguration _config;

        private const string PHYSICAL_FILES_PATH = "PhysicalFilesPath";

        private const string VIRTUAL_FILES_PATH = "VirtualFilesPath";

        public string _basePhysicalPath { get; private set; }
        
        public string _baseVirtualPath { get; private set; }

        public UploadController(IConfiguration config)
        {
            _config = config;
            _basePhysicalPath = Path.Combine(_config[PHYSICAL_FILES_PATH], "Images/PostImage");
            _baseVirtualPath = Path.Combine(_config[VIRTUAL_FILES_PATH], "Images/PostImage");
        }

        
        // [HttpGet]
        // [Route("GetImage")]
        // public async Task<IActionResult> GetImage(string key)
        // {
        //     // if (key == null) return new JsonResult("Bad Request");
        //     // var response = await new HttpClient().GetAsync($"{_config["Api"]}/Redis/GetImage?key={key}");
        //     // var imageBytes = await response.Content.ReadAsByteArrayAsync();
        //     // return new FileContentResult(imageBytes, "image/jpeg");
        //     // return imageBytes;
        //     return new ForbidResult();
        // }

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
            
            var type = imageFile.ContentType;
            if (type != "image/jpeg" && type != "image/png")
                return new Form()
                {
                    {"Status","Failure"},
                    {"Info",$"Unsupported Image Type {type}"}
                }.ToJson();
            
            var path = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            var fullPhysicalPath = Path.Combine(_basePhysicalPath, path);
            if (!Directory.Exists(fullPhysicalPath))
                Directory.CreateDirectory(fullPhysicalPath);
            
            var fileName = $"{RandomGenerator.GenerateGUID()}_{imageFile.FileName}";
            
            var fileStream = System.IO.File.Open(Path.Combine(fullPhysicalPath, fileName) , FileMode.Create);
            await imageFile.CopyToAsync(fileStream);
            fileStream.Close();

            return new Form()
            {
                { "Status", "Success" },
                { "Info", Path.Combine(_baseVirtualPath, path, fileName) }
            }.ToJson();
            // var response = await new HttpClient().PostAsync($"{_config["Api"]}/Redis/SetImage", new StreamContent(imageFile.OpenReadStream()));
            // return await response.Content.ReadAsStringAsync();



        }
    }
}