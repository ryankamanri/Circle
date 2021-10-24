using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Kamanri.WebSockets.Model;

namespace Kamanri.Extensions
{
    public static class WebSocketExtension
    {
        public static int bufferSize = 1024 * 128;

        private static byte[] buffer = new byte[bufferSize];

        /// <summary>
        /// 接受一次WebSocket请求, 将请求的内容填入 OnMessage() 的参数, 并将 OnMessage() 的返回值发回请求端
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="OnMessage"></param>
        /// <returns></returns>
        public static async Task OnReceiveMessageAsync(this WebSocket webSocket, Func<IList<WebSocketMessage>, Task<IList<WebSocketMessage>>> OnMessage)
        {
            IList<WebSocketMessage> messages = new List<WebSocketMessage>();

            WebSocketReceiveResult result;

            do
            {
                try
                {
                    result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        CancellationToken.None);

                    //messages.Add(buffer.GetWebSocketMessage());
                    buffer.GetWebSocketMessages(messages);
                }
                catch (Exception e)
                {
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(e.ToString().ToByteArray()),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );

                    throw e;
                }
            } while (!result.EndOfMessage);


            var sendMessages = await OnMessage(messages);

            if (sendMessages == default) return;

            await SendMessageAsync(webSocket, sendMessages);


        }

        /// <summary>
        /// 发送一次WebSocket请求
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="sendMessages"></param>
        /// <returns></returns>
        public static async Task SendMessageAsync(this WebSocket webSocket, IList<WebSocketMessage> sendMessages)
        {
            byte[] sendBuffer;
            
            try
            {

                if(sendMessages.Count == 0) return;

                for (int i = 0; i < sendMessages.Count - 1; i++)
                {
                    sendBuffer = sendMessages[i].SetBytes();

                    await webSocket.SendAsync(
                        new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length),
                        WebSocketMessageType.Binary,
                        false,
                        CancellationToken.None);
                }

                sendBuffer = sendMessages[sendMessages.Count - 1].SetBytes();

                await webSocket.SendAsync(
                    new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length),
                    WebSocketMessageType.Binary,
                    true,
                    CancellationToken.None);


            }catch(Exception e)
            {
                throw e;
            }
        }


    }

}