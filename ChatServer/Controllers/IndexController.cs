using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Kamanri.Extensions;
using Kamanri.WebSockets;

namespace ChatServer.Controllers
{
    public class IndexController : ControllerBase
    {
        ILogger<IndexController> _logger;

        IWebSocketSession _wsSession;

        public IndexController(ILoggerFactory loggerFactory, IWebSocketSession wsSession)
        {
            _logger = loggerFactory.CreateLogger<IndexController>();
            _wsSession = wsSession;
        }

        [HttpGet]
        [Route("/")]
        public async Task Indexer()
        {

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                //注册WebSocket
                await _wsSession.AcceptWebSocketInjection(webSocket);
            }


        }


    }
}