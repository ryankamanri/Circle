using System;
using System.IO;
using System.Threading.Tasks;
using ApiServer.Services;
using Kamanri.Extensions;
using Kamanri.Http;
using Kamanri.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;
using WebOptimizer;
using Mutex = System.Threading.Mutex;

namespace ApiServer.Controllers
{
    [Controller]
    [Route("Redis/")]
    public class RedisController: ControllerBase
    {
        // private readonly RedisService _redisService;
        // private readonly ILogger<RedisController> _logger;
        // private const int MAX_IMAGE_LENGTH = 100000;
        // private readonly byte[] _imageBytes = new byte[MAX_IMAGE_LENGTH];
        // private readonly Memory<byte> _imageMemory;
        // private readonly Kamanri.Utils.Mutex _bufferMutex = new Kamanri.Utils.Mutex();
        //
        // public RedisController(RedisService redisService, ILoggerFactory loggerFactory)
        // {
        //     _logger = loggerFactory.CreateLogger<RedisController>();
        //     _redisService = redisService;
        //     _imageMemory = new Memory<byte>(_imageBytes);
        // }

        // [HttpGet]
        // [Route("GetImage")]
        // public async Task<IActionResult> GetImage(string key)
        // {
        //     // if (key == null) return "Invalid Request".ToByteArray();
        //     // if (key == null) return new JsonResult("Invalid Request");
        //     // return new FileContentResult( await _redisService.GetAsync(RedisService.StoredObjectType.IMAGE, key), "image/jpeg");
        //     return new BadRequestResult();
        // }
        
        // [HttpPost]
        // [Route("SetImage")]
        // public async Task<IActionResult> SetImage()
        // {
        //     // if (!HttpContext.Request.Form.TryGetValue("ImageBytes", out var )) return "-2".ToJson();
        //     // Console.WriteLine(
        //     //     HttpContext.Request);
        //     //
        //     // _bufferMutex.Wait();
        //     //
        //     // var length = await new BufferedStream(HttpContext.Request.Body).ReadAsync(_imageMemory);
        //     // if (length >= MAX_IMAGE_LENGTH) 
        //     //     return new Form()
        //     //     {
        //     //         {"Status","Failure"},
        //     //         {"Info","Set Failed (Image Too Big)"}
        //     //     }.ToJson();
        //     // var key = $"{DateTime.Now.Ticks}{RandomGenerator.GenerateGUID()}";
        //     // _logger.LogDebug($"Key: {key}");
        //     // var status = await _redisService.SetAsync(RedisService.StoredObjectType.IMAGE, key, _imageMemory[..length]);
        //     // _bufferMutex.Signal();
        //     // if (!status) 
        //     //     return new Form()
        //     //     {
        //     //         {"Status","Failed"},
        //     //         {"Info","Set Failed"}
        //     //     }.ToJson();
        //     // return new Form()
        //     // {
        //     //     { "Status", "Success" },
        //     //     { "Info", key }
        //     // }.ToJson();
        //     return default;
        // }
    }
}