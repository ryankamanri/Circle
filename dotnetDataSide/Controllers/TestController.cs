using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kamanri.Extensions;
using Kamanri.Database;
using dotnetDataSide.Services;
using dotnetDataSide.Model;
namespace dotnetDataSide
{
    [Controller]
    [Route("Test")]
    public class TestController : ControllerBase
    {
        private MessageService _messageService;

        private DataBaseContext _dbc;

        public TestController(MessageService messageService,DataBaseContext dbc)
        {
            _messageService = messageService;
            _dbc = dbc;
        }

        [Route("test")]
        public async Task<string> Test()
        {
            var messages = await _dbc.SelectCustom<Message>(new Message(),$"SendUserID = 3");
            return messages.ToJson();
        }

        [Route("admsg")] 
        public async Task<IActionResult> AppendMessage()
        {
           return new JsonResult(await _messageService.AppendMessage(0, new Message(3,2,false,DateTime.Now,MessageContentType.Text,"love permanently".ToByteArray())));
            
        }
    }
}