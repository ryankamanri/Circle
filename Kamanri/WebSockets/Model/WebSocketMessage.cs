using System;
using System.Net.WebSockets;
using System.Collections.Generic;
using Kamanri.Extensions;
using Newtonsoft.Json;

namespace Kamanri.WebSockets.Model
{
    public sealed class WebSocketMessage
    {
        public static int IS_EFFECTIVE_CODE = 200;
        public static int HEADER_LENGTH = 10;

        public static int UINT32_LENGTH = 4;
        public static int UINT8_LENGTH = 1;

        public static int IS_EFFECTIVE_LOCATION = 0;
        public static int MESSAGE_EVENT_CODE_LOCATION = 1;

        public static int MESSAGE_TYPE_LOCATION = 5;
        public static int LENGTH_LOCATION = 6;
        public static int MESSAGE_LOCATION = 10;

        /// <summary>
        /// 该消息所响应的事件
        /// </summary>
        /// <value></value>
        public WebSocketMessageEvent MessageEvent { get; set;}

        /// <summary>
        /// 该消息种类 (Text, Binary, Close)
        /// </summary>
        /// <value></value>
        public WebSocketMessageType MessageType { get; }

        /// <summary>
        /// 该消息长度 (统一设置为 byte[] 的长度)
        /// </summary>
        /// <value></value>
        public int Length {get; private set; }

        private object message;

        public object Message 
        {
            get => message;
            set
            {
                message = value;
                if(message == default) return;
                if((MessageType == WebSocketMessageType.Text && message.GetType() != typeof(string)) || 
                MessageType == WebSocketMessageType.Binary && message.GetType() != typeof(byte[]))
                {
                    var exceptionInfo = $"WebSocketMessageError : The Required MessageType Is '{MessageType}' But Offered Unmatched Type '{message.GetType()}'";
                    throw new WebSocketModelException(exceptionInfo);
                }
                    
                if(MessageType == WebSocketMessageType.Binary) Length = ((byte[])message).Length;
                else Length = ((string)message).ToByteArray().Length;
            }
        }

        public WebSocketMessage(){}

        public WebSocketMessage(WebSocketMessageEvent messageEvent, WebSocketMessageType messageType, object message)
        {
            MessageEvent = messageEvent;
            MessageType = messageType;
            Message = message;
        }
        [JsonConstructor]
        public WebSocketMessage(WebSocketMessageEvent MessageEvent, WebSocketMessageType MessageType, int Length, object Message)
        {
            this.MessageEvent = MessageEvent;
            this.MessageType = MessageType;
            this.Length = Length;
            this.Message = Message;
        }

        public WebSocketMessage MessageWithoutContent()
        {
            return new WebSocketMessage(this.MessageEvent, this.MessageType, this.Length, default);
        }


        /// <summary>
        /// 由`WebSocketMessage` 对象转换为 字节数组.
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
        /// <returns></returns>
        public byte[] SetBytes()
        {
            var bytes = new byte[Length + HEADER_LENGTH];// 消息长度 + 10 字节报文头部 + 1 字节结尾
            bytes[IS_EFFECTIVE_LOCATION] = ((byte)IS_EFFECTIVE_CODE);// 设置为有效
            bytes[MESSAGE_TYPE_LOCATION] = (byte)MessageType;// 设置消息类型
            var eventCodeSegment = bytes.AsSpan().Slice(MESSAGE_EVENT_CODE_LOCATION, UINT32_LENGTH);
            var lengthSegment = bytes.AsSpan().Slice(LENGTH_LOCATION, UINT32_LENGTH);
            var messageSegment = bytes.AsSpan().Slice(MESSAGE_LOCATION, Length);
            BitConverter.GetBytes(MessageEvent.Code).CopyTo(eventCodeSegment);
            BitConverter.GetBytes(Length).CopyTo(lengthSegment);
            //Message 类型可能是string 或 byte[] 这里需要进行辨别
            if(MessageType == WebSocketMessageType.Text)
                (Message as string).ToByteArray().CopyTo(messageSegment);
            else if(MessageType == WebSocketMessageType.Binary)
                (Message as byte[]).CopyTo(messageSegment);
            else throw new WebSocketModelException($"The Message Type {MessageType} Is Invalid To Cast To ByteArray");
            return bytes;
        }

    }

    
    
}