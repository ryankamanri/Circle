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
using Kamanri.Database.Model.Relation;
using Kamanri.WebSockets.Model;
using Kamanri.Self;
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
            
            var msgs = new Dictionary<string, object>()
            {
                {"key1", 1},
                {"key2", new ID_IDList()
                {
                    new ID_ID(1, 2, "")
                }}
            };


            var jstr = JsonConvert.SerializeObject(msgs);

            //input jstr
            var jToken = JToken.Parse(jstr);

            dynamic obj = JsonDynamic.JTokenToObject(jToken);

            return jToken["key2"][0]["ID"].ToString();
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

