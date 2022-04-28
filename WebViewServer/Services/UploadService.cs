using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using Kamanri.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebViewServer.Services
{
    public class UploadService
    {
        public static class UploadFileType
        {
            public const string IMAGE = "Images";
        }
        public static class ImageType
        {
            public const string POST_IMAGE = "PostImage";
            public const string HEAD_IMAGE = "HeadImage";
        }
        private readonly IConfiguration _config;

        private const string PHYSICAL_FILES_PATH = "PhysicalFilesPath";

        private const string VIRTUAL_FILES_PATH = "VirtualFilesPath";

        private List<string> _supportedTypes = new List<string>()
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/tiff",
            "image/fax",
            "image/x-icon",
            "image/pnetvue",
            "image/vnd.rn-realpix",
            "image/vnd.wap.wbmp"
                
        };
        
       

        public UploadService(IConfiguration config)
        {
            _config = config;
        }

        


        public async Task<Form> UploadFile(IFormFile file, string uploadFileType, string fileDetailType)
        {
            var basePhysicalPath = $"{_config[PHYSICAL_FILES_PATH]}/{uploadFileType}/{fileDetailType}";
            var baseVirtualPath = $"{_config[VIRTUAL_FILES_PATH]}/{uploadFileType}/{fileDetailType}";
            
            var type = file.ContentType;
            if (_supportedTypes.Find(supportedType => supportedType == type) == default)
                return new Form()
                {
                    {"Status","Failure"},
                    {"Info",$"Unsupported Image Type {type}"}
                };
            
            var path = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            var fullPhysicalPath = $"{basePhysicalPath}/{path}";
            if (!Directory.Exists(fullPhysicalPath))
                Directory.CreateDirectory(fullPhysicalPath);
            
            var fileName = $"{RandomGenerator.GenerateGUID()}_{file.FileName}";
            
            var fileStream = System.IO.File.Open($"{fullPhysicalPath}/{fileName}" , FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();

            return new Form()
            {
                { "Status", "Success" },
                { "Info", $"{baseVirtualPath}/{path}/{fileName}" }
            };
            
        }
    }
}