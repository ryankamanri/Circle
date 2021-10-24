using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Kamanri.Extensions;
using Kamanri.WebSockets;
namespace dotnetDataSide.Controllers
{
    public class IndexController : ControllerBase
    {
        ILogger<IndexController> _logger;

        WebSocketClient _wsClient;

        WebSocketMessageService _wsmService;


        public IndexController(ILoggerFactory loggerFactory, WebSocketClient wsClient, WebSocketMessageService wsmService)
        {
            _logger = loggerFactory.CreateLogger<IndexController>();
            _wsClient = wsClient;
            _wsmService = wsmService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Indexer()
        {

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                //注册WebSocket
                await _wsClient.AcceptWebSocketInjection(webSocket);
            }

            return new OkResult();
            
        }


    }
}