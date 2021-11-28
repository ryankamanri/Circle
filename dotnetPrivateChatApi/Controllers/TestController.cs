using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Kamanri.Extensions;
using Kamanri.Database;
using Kamanri.Database.Models.Relation;
using Kamanri.WebSockets.Model;
using Kamanri.Self;
using dotnetPrivateChatApi.Models;
namespace dotnetPrivateChatApi.Controllers
{
    [Controller]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private ILogger<TestController> _logger;
        public TestController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TestController>();
        }
        [Route("rdfile")]
        public byte[] ReadFile()
        {
            // byte[] bytes = System.IO.File.ReadAllBytes(
            //     "C:\\Users\\97448\\Pictures\\QQ图片20210714185547.jpg");
            // return new FileContentResult(bytes,MediaTypeHeaderValue.Parse("image/jpeg"));
            // var bytes = "hhhheeeelllllllloooo    世    界!!".ToByteArray();
            // Stream stream = new MemoryStream();
            // stream.ReadAsync(bytes,0,bytes.Length);
            // stream.Wr
            
            return default;
        }
        [Route("jsontest")]
        public string Test()
        {
            Tag tag = new Tag(1, "hahaha");
            return tag.ToJson();
        }
        [Route("wsmtest")]
        public string WSMTest()
        {
            var msg = new WebSocketMessage(
                WebSocketMessageEvent.OnClientConnect, 
                System.Net.WebSockets.WebSocketMessageType.Text, 
                "Client Hello  你好");
            var bytes = msg.SetBytes();
            var showstr =  "\n" + bytes.ShowArrayItems();
            _logger.LogInformation(showstr);
            showstr =  "\n" + bytes.ShowArrayItems();
            _logger.LogInformation(showstr);
            //var recMsg = bytes.GetWebSocketMessage();
            _logger.LogInformation(msg.ToJson());

                
            return showstr;
        }
    }

}

