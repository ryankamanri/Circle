using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Kamanri.Extensions;
using Kamanri.WebSockets;
using dotnetDataSide.Services;
namespace dotnetDataSide.Controllers
{
    public class IndexController : ControllerBase
    {
        ILogger<IndexController> _logger;

        IWebSocketClient _wsClient;

        public IndexController(ILoggerFactory loggerFactory, IWebSocketClient wsClient)
        {
            _logger = loggerFactory.CreateLogger<IndexController>();
            _wsClient = wsClient;
        }

        [HttpGet]
        [Route("/")]
        public async Task Indexer()
        {

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                //注册WebSocket
                await _wsClient.AcceptWebSocketInjection(webSocket);
            }

            
        }


    }
}