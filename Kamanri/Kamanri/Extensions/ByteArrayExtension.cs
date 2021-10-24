using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Linq;
using System.Text;
using Kamanri.WebSockets.Model;

namespace Kamanri.Extensions
{
    public static class ByteArrayExtension
    {


        /// <summary>
        /// 将字节数组转换为字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToStringContent(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 将字节数组以0作为结尾进行截断
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] CutZeroEnd(this byte[] bytes)
        {
            int i = 0;
            while(bytes[i] != 0) i++;
            return bytes.Take(i).ToArray();
        }

        public static string ShowArrayItems(this byte[] bytes)
        {
            string result = "";
            var count = 0;
            foreach(var _byte in bytes)
            {
                count ++;
                result += $" {String.Format("{0:X2}", _byte.CompareTo(0))} ";
                if(count == 0x10) 
                {
                    result += "\n";
                    count = 0;
                }
            }
            return result;
        }

        public static void AddValues(this byte[] bytes, int length, int startIndex = 0, byte value = 1)
        {
            Span<byte> bytesSegment = bytes.AsSpan().Slice(startIndex, length);
            for(int i = 0;i < length; i++)
            {
                bytesSegment[i] += value;
            }
        }


        /// <summary>
        /// 由字节数组转换为 `WebSocketMessage` 对象.
        /// {
        /// 基于websocket协议的自定义协议 : 
        ///     
        ///     byte 0 <==> IsEffective ? The Effective Code Is `200`
        ///     byte 1 ~ 4 <==> WebSocketMessage.MessageEvent.Code (int32)
        ///     byte 5 <==> WebSocketMessage.MessageType (enum = 3)
        ///     byte 6 ~ 9 <==> WebSocketMessage.Length (int32)
        ///     byte 10 ~ 10 + length <==> WebSocketMessage.Message
        /// }
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>length of a websocketmessage</returns>
        public static int GetAWebSocketMessage(this byte[] bytes, IList<WebSocketMessage> wsMessages, int startIndex = 0)
        {
            
            if(bytes == default) throw new NullReferenceException("The Input Reference Can Not Be Null");
            Span<byte> bytesFromStartSegment = bytes.AsSpan().Slice(startIndex);
            if(bytesFromStartSegment[0] != WebSocketMessage.IS_EFFECTIVE_CODE || bytesFromStartSegment.Length < WebSocketMessage.IS_EFFECTIVE_CODE) 
                throw new Exception("The Stream Of ByteArray Is Not A Effective Message");

            var lengthSeg = bytesFromStartSegment.Slice(WebSocketMessage.LENGTH_LOCATION, WebSocketMessage.UINT32_LENGTH);
            var length = BitConverter.ToInt32(lengthSeg);
            Span<byte> bytesSegment = bytes.AsSpan().Slice(startIndex, length + WebSocketMessage.HEADER_LENGTH);
            //bytes = bytes.CutZeroEnd();
            
            int eventCode = BitConverter.ToInt32(bytesSegment.Slice(WebSocketMessage.MESSAGE_EVENT_CODE_LOCATION, WebSocketMessage.UINT32_LENGTH));
            var wsMessageTypeCode = bytesSegment[WebSocketMessage.MESSAGE_TYPE_LOCATION];
            WebSocketMessageType wsMessageType = wsMessageTypeCode == 0 ? WebSocketMessageType.Text : (wsMessageTypeCode == 1 ? WebSocketMessageType.Binary : WebSocketMessageType.Close);
            //int length = bytes.ToInt32(6);
            object message = bytesSegment.Slice(WebSocketMessage.MESSAGE_LOCATION).ToArray();
            if(wsMessageType == WebSocketMessageType.Text) message = (message as byte[]).ToStringContent();

            var result = new WebSocketMessage(
                new WebSocketMessageEvent(eventCode),
                wsMessageType,
                length,
                message);// 后有\0

            bytes[startIndex] = 0; // 设置该消息已经无效
            if(wsMessages != default) wsMessages.Add(result);
            return length + WebSocketMessage.HEADER_LENGTH;
        }

        public static void GetWebSocketMessages(this byte[] bytes, IList<WebSocketMessage> wsMessages, int startIndex = 0)
        {
            try
            {
                int byteOffset = startIndex;
                do
                {
                    byteOffset += GetAWebSocketMessage(bytes, wsMessages, byteOffset);
                    
                }while(bytes[byteOffset] == WebSocketMessage.IS_EFFECTIVE_CODE);

            }catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        
    }
}