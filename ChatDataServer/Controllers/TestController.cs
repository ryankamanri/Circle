using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kamanri.Extensions;
using Kamanri.Database;
using System.Dynamic;
using Microsoft.Extensions.Logging;
using ChatDataServer.Models;
using ChatDataServer.Services;

namespace ChatDataServer.Controllers
{
    [Controller]
    [Route("Test")]
    public class TestController : ControllerBase
    {
        private MessageService _messageService;

        private readonly ILogger<TestController> _logger;

        public TestController(MessageService messageService, ILoggerFactory loggerFactory)
        {
            _messageService = messageService;
            _logger = loggerFactory.CreateLogger<TestController>();
        }

        [Route("test")]
        public string Test()
        {
            var jToken = new Dictionary<string, object>();
            jToken["id"] = Guid.NewGuid().ToString();
            jToken["name"] = "name";
            jToken["description"] = "description";
            var dObj = new ExpandoObject();

            Kamanri.Self.Dynamic.Cover(jToken, dObj);

            return dObj.ToJson();
        }

        [Route("admsg")]
        public async Task<IActionResult> AppendMessage()
        {
            return new JsonResult(await _messageService.AppendConstantMessage(0, new Message(3, 2, false, DateTime.Now, MessageContentType.Text, "love permanently".ToByteArray())));

        }


    }
}