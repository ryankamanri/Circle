using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace dotnetPrivateChatApi.Controllers
{
    [Route("WebSocket")]
    public class WebSocketController : ControllerBase
    {
        [HttpGet]
        [Route("test1")]
        public async Task Test1()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var buffer = new byte[1024 * 4];
                    
                    var result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), 
                        CancellationToken.None);

                    while (!result.CloseStatus.HasValue)
                    {
                        await webSocket.SendAsync(
                            new ArraySegment<byte>(buffer, 0, result.Count), 
                            result.MessageType, 
                            result.EndOfMessage, 
                            CancellationToken.None);

                        result = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None);
                    }
                    await webSocket.CloseAsync(
                        result.CloseStatus.Value, 
                        result.CloseStatusDescription, 
                        CancellationToken.None);
                }
            }

        }

        [Route("sendmsg")]
        public async Task SendMessage()
        {
            await Task.CompletedTask;
        }
    }
}