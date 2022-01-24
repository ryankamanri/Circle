import { Sleep } from '../My.js';

const IS_EFFECTIVE_CODE = 200;
const HEADER_LENGTH = 10;

const UINT32_LENGTH = 4;
const UINT8_LENGTH = 1;

const IS_EFFECTIVE_LOCATION = 0;
const MESSAGE_EVENT_CODE_LOCATION = 1;

const MESSAGE_TYPE_LOCATION = 5;
const LENGTH_LOCATION = 6;
const MESSAGE_LOCATION = 10;

var WebSocketMessageEvent = 
{
    OnConnect : {Code : 1},
    OnStore : {Code : 2},
    OnClientConnect : {Code : 100},
    OnServerConnect : {Code : 101},
    OnDataSideConnect : {Code : 102},
    OnClientDisconnect : {Code : 200},
    OnServerDisconnect : {Code : 201},
    OnDataSideDisconnect : {Code : 202},
    OnClientMessage : {Code : 300}, 
    OnServerMessage : {Code : 301}, 
    OnDataSideMessage : {Code : 302}, 
    OnClientTempMessage : {Code : 400}, 
    OnServerTempMessage : {Code : 401},
    OnDataSideTempMessage : {Code : 402},
    OnClientPreviousMessage : {Code : 500},
    OnServerPreviousMessage : {Code : 501},
    OnDataSidePreviousMessage : {Code : 502}
}

var WebSocketMessageType = 
{
    Text : 0,
    Binary : 1,
    Close : 2
}

function WebSocketMessage(messageEvent, messageType, message)
{
    this.MessageEvent = messageEvent;
    this.MessageType = messageType;
    this.Message = message; 
    this.Length = this.MessageType == 0 ? new TextEncoder('utf-8').encode(message).length : message.length;
}


function MyWebSocket(url)
{
    this.websocket = new WebSocket(url);
    this.IsReady = false;
    this.Open = () => this.IsReady = true;
    this.Close = () => this.websocket.close();
    this.eventHandlers = [];
    this.OnMessageHandler = OnMessageHandler;
    this.SendMessages = (wsMessages) => SendMessages(this.websocket, wsMessages);
    this.AddEventHandler = (wsmEvent, Handler) => 
    {
        this.eventHandlers[wsmEvent.Code] = Handler;
    }
    this.websocket.onopen = async(ev) => 
    {
        while (!this.IsReady) await Sleep(500);
        console.log("The WebSocket Connection Opened");
        SendMessages(this.websocket, [new WebSocketMessage(WebSocketMessageEvent.OnConnect, WebSocketMessageType.Text, "Hello")]);
    }

    this.websocket.onclose = ev =>
         console.log("The WebSocket Connection Closed");
    this.websocket.onerror = ev =>
         console.error(`An Error Has Occured When ${ev.type}`);
    this.websocket.onmessage = ev =>
         OnMessageHandler(ev, this.websocket, this.eventHandlers);
    
    // return this;
}


async function OnMessageHandler(ev, websocket, eventHandlers) 
{
    var bytes = await ev.data.arrayBuffer();
    var wsMessages = [];
    var byteOffSet = 0;
    var isEffective, eventCode, messageType, length, message, wsMessage;
    do
    {
        isEffective = new DataView(bytes, byteOffSet, UINT8_LENGTH).getUint8(0);
        if(isEffective != IS_EFFECTIVE_CODE || bytes.length < HEADER_LENGTH)
        {
            console.error("The Stream Of ByteArray Is Not A Effective Message");
        }
        eventCode = new DataView(bytes,byteOffSet + MESSAGE_EVENT_CODE_LOCATION, UINT32_LENGTH).getInt32(0, true);
        messageType = new DataView(bytes,byteOffSet + MESSAGE_TYPE_LOCATION, UINT8_LENGTH).getUint8(0);
        length = new DataView(bytes,byteOffSet + LENGTH_LOCATION, UINT32_LENGTH).getInt32(0, true);
        message = new DataView(bytes,byteOffSet + MESSAGE_LOCATION, length)
        if(messageType == WebSocketMessageType.Text)
        {
            message = new TextDecoder("utf-8").decode(message);
            // length = message.length;
        }
        
        wsMessage = new WebSocketMessage({Code : eventCode}, messageType, message);
        wsMessages.push(wsMessage);
        byteOffSet += (length + HEADER_LENGTH);
    }while(byteOffSet < bytes.byteLength);

    try {
        if(eventHandlers[eventCode] == undefined) return;
        var sendMessage = await eventHandlers[eventCode](wsMessages);
        await SendMessages(websocket, sendMessage);
        
    } catch (error) {
        console.error(error);
        console.log(`eventCode : ${eventCode}`);
    }

    



}
/// <summary>
/// 由`WebSocketMessage` 对象转换为 字节数组.
/// {
/// 基于websocket协议的自定义协议 : 
///     
///     byte 0 <==> IsEffective ? The Effective Code Is `200`
///     byte 1 ~ 4 <==> WebSocketMessage.eventCode.Code (int32) little endian
///     byte 5 <==> WebSocketMessage.MessageType (enum = 3)
///     byte 6 ~ 9 <==> WebSocketMessage.Length (int32)
///     byte 10 ~ 10 + length <==> WebSocketMessage.Message
/// }
/// </summary>
/// <param name="bytes"></param>
/// <returns></returns>
function SetAMessage(bytes, startIndex, wsMessage) 
{

    new DataView(bytes, startIndex + IS_EFFECTIVE_LOCATION, UINT8_LENGTH).setUint8(0, IS_EFFECTIVE_CODE);
    new DataView(bytes, startIndex + MESSAGE_EVENT_CODE_LOCATION, UINT32_LENGTH).setInt32(0, wsMessage.MessageEvent.Code, true);
    new DataView(bytes, startIndex + MESSAGE_TYPE_LOCATION, UINT8_LENGTH).setUint8(0, wsMessage.MessageType);
    new DataView(bytes, startIndex + LENGTH_LOCATION, UINT32_LENGTH).setInt32(0, wsMessage.Length, true);
    var messageSegment = new DataView(bytes, startIndex + MESSAGE_LOCATION, wsMessage.Length);
    if (wsMessage.MessageType == WebSocketMessageType.Text)//Text
    {
        var messageStr = new TextEncoder("utf-8").encode(wsMessage.Message);
        for (var i = 0; i < wsMessage.Length; i++) {
            messageSegment.setUint8(i, messageStr[i]);
        }
    }
    else if (wsMessage.MessageType == WebSocketMessageType.Binary)
        for (var i = 0; i < wsMessage.Length; i++) {
            messageSegment.setUint8(i, wsMessage.Message[i]);
        }

    return wsMessage.Length + HEADER_LENGTH;


}

function SendMessages(websocket, wsMessages)
{
    if(wsMessages == undefined || wsMessages.length == 0) return;
    return new Promise(resolve => 
    {
        var totalLength = 0;
        wsMessages.forEach(wsMessage => {
            totalLength += wsMessage.Length + HEADER_LENGTH;
        });
        var bytes = new ArrayBuffer(totalLength);
        var byteOffset = 0;
        wsMessages.forEach(wsMessage => {
            byteOffset += SetAMessage(bytes, byteOffset, wsMessage);
        });
        if (websocket.readyState == WebSocket.OPEN)
            websocket.send(bytes);
        resolve(0);
    });
    
}

export default
{
    MyWebSocket, WebSocketMessage, WebSocketMessageType, WebSocketMessageEvent
}