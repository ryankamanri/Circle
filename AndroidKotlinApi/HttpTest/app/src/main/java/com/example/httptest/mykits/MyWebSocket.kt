package com.example.httptest.mykits


import android.net.http.HttpResponseCache.install
import android.util.Log
import androidx.collection.arrayMapOf
import okhttp3.OkHttpClient
import okhttp3.Response
import okhttp3.WebSocket
import okio.ByteString
import okio.ByteString.Companion.toByteString
import java.lang.Exception
import java.nio.Buffer
import java.nio.ByteBuffer
import java.nio.ByteOrder
import java.nio.charset.Charset
import java.util.*
import kotlin.collections.ArrayList
import kotlin.experimental.or
import kotlin.properties.Delegates


class MyWebSocket(url: String) {
    companion object {

        const val IS_EFFECTIVE_CODE: UByte = 200u
        const val HEADER_LENGTH = 10

        const val UINT32_LENGTH = 4
        const val UINT8_LENGTH = 1

        const val IS_EFFECTIVE_LOCATION = 0
        const val MESSAGE_EVENT_CODE_LOCATION = 1

        const val MESSAGE_TYPE_LOCATION = 5
        const val LENGTH_LOCATION = 6
        const val MESSAGE_LOCATION = 10

    }

    data class WebSocketMessageEvent(val Code: Int) {
        companion object {
            val OnConnect = WebSocketMessageEvent(1)
            val OnStore = WebSocketMessageEvent(2)
            val OnClientConnect = WebSocketMessageEvent(100)
            val OnServerConnect = WebSocketMessageEvent(101)
            val OnDataSideConnect = WebSocketMessageEvent(102)
            val OnClientDisconnect = WebSocketMessageEvent(200)
            val OnServerDisconnect = WebSocketMessageEvent(201)
            val OnDataSideDisconnect = WebSocketMessageEvent(202)
            val OnClientMessage = WebSocketMessageEvent(300)
            val OnServerMessage = WebSocketMessageEvent(301)
            val OnDataSideMessage = WebSocketMessageEvent(302)
            val OnClientKeepAlive = WebSocketMessageEvent(400)
            val OnServerKeepAlive = WebSocketMessageEvent(401)
            val OnDataSideKeepAlive = WebSocketMessageEvent(402)
            val OnClientPreviousMessage = WebSocketMessageEvent(500)
            val OnServerPreviousMessage = WebSocketMessageEvent(501)
            val OnDataSidePreviousMessage = WebSocketMessageEvent(502)
        }
    }

    enum class WebSocketMessageType {
        Text,
        Binary,
        Close
    }


    data class WebSocketMessage(
        val EventCode: WebSocketMessageEvent,
        val MessageType: WebSocketMessageType,
        val Message: Any
    ) {

        var Length: Int = 0

        init {
            try {
                Length = when (this.MessageType) {
                    WebSocketMessageType.Text -> (Message as String).toByteArray().size
                    WebSocketMessageType.Binary -> (Message as ByteArray).size
                    else -> (Message as ByteArray).size
                }
            } catch (e: Exception) {
                Log.e(
                    toString(),
                    "Failed To Init The WebSocketMessage Class \n Caused By : ${e.message}"
                )
                e.printStackTrace()
            }

        }

    }

    val Url = url
    private val websocket = OkHttpWebSocket(Url)
    private val eventHandlers =
        arrayMapOf<Int, (ArrayList<WebSocketMessage>) -> ArrayList<WebSocketMessage>>()

    fun AddEventHandler(
        wsmEvent: WebSocketMessageEvent,
        Handler: (ArrayList<WebSocketMessage>) -> ArrayList<WebSocketMessage>
    ) {
        this.eventHandlers[wsmEvent.Code] = Handler
    }

    init {
        websocket.OnOpen = {
            try {
                Log.i(toString(), "The WebSocket Connection Opened");
                SendMessages(
                    arrayListOf<WebSocketMessage>(
                        WebSocketMessage(
                            WebSocketMessageEvent.OnClientConnect,
                            WebSocketMessageType.Text,
                            "Hello"
                        )
                    )
                )
            } catch (e: Exception) {
                Log.e(
                    toString(),
                    "Failed To Init MyWebSocket And Establish Connection \n Caused By : $e : ${e.message}"
                )
                e.printStackTrace()
            }

        }

        websocket.OnClosed = { code: Int, reason: String ->
            Log.i(toString(), "The WebSocket Connection Closed")
        }

        websocket.OnFaliure = { t: Throwable, response: Response? ->
            Log.e(toString(), "An Error Has Occured Caused By : $t : ${t.message}")
        }

        websocket.OnMessage = { bytes: ByteString ->
            try {
                OnMessageHandler(bytes.toByteArray())
            }catch (e : Exception) {
                Log.e(toString(),"An Exception Occured While Executing The OnMessageHandler \n Caused By : $e : ${e.message}")
                e.printStackTrace()
            }

        }

    }


    private fun OnMessageHandler(bytes: ByteArray) {
        val wsMessages = arrayListOf<WebSocketMessage>()
        var byteOffSet = 0
        var isEffective: UByte
        var eventCode: Int
        var messageType: WebSocketMessageType
        var length: Int
        var message: Any = byteArrayOf()
        var wsMessage: WebSocketMessage
        do {
            val wrapedBytes = ByteBuffer.wrap(bytes)
            isEffective = bytes[byteOffSet].toUByte()
            if (isEffective != IS_EFFECTIVE_CODE || bytes.lastIndex < HEADER_LENGTH) {
                Log.e(toString(), "The Stream Of ByteArray Is Not A Effective Message");
            }
            eventCode =
                ByteBuffer.wrap(bytes, byteOffSet + MESSAGE_EVENT_CODE_LOCATION, UINT32_LENGTH)
                    .order(ByteOrder.LITTLE_ENDIAN)
                    .int
            val wsMessageTypeCode = bytes[byteOffSet + MESSAGE_TYPE_LOCATION].toUByte()

            messageType = when (wsMessageTypeCode) {
                UByte.MIN_VALUE.or(0u) -> WebSocketMessageType.Text
                UByte.MIN_VALUE.or(1u) -> WebSocketMessageType.Binary
                else -> WebSocketMessageType.Close
            }.also {
                length =
                    ByteBuffer.wrap(bytes, byteOffSet + LENGTH_LOCATION, UINT32_LENGTH)
                        .order(ByteOrder.LITTLE_ENDIAN)
                        .int
                when (it) {
                    WebSocketMessageType.Text -> {
                        message = String(
                            ByteBuffer.wrap(bytes, byteOffSet + MESSAGE_LOCATION, length).toByteString().toByteArray(),

                        )
                    }
                    WebSocketMessageType.Binary -> {
                        message =
                            ByteBuffer.wrap(bytes, byteOffSet + MESSAGE_LOCATION, length).toByteString().toByteArray()
                    }

                }
            }


            wsMessage = WebSocketMessage(WebSocketMessageEvent(eventCode), messageType, message)
            wsMessages.add(wsMessage)
            byteOffSet += (length + HEADER_LENGTH)
        } while (byteOffSet <= bytes.lastIndex)

        val sendMessage = eventHandlers[eventCode]?.let { it(wsMessages) }

        if (sendMessage != null) {
            SendMessages(sendMessage)
        }


    }

    ///// <summary>
///// 由`WebSocketMessage` 对象转换为 字节数组.
///// {
///// 基于websocket协议的自定义协议 :
/////
/////     byte 0 <==> IsEffective ? The Effective Code Is `200`
/////     byte 1 ~ 4 <==> WebSocketMessage.eventCode.Code (int32) little endian
/////     byte 5 <==> WebSocketMessage.MessageType (enum = 3)
/////     byte 6 ~ 9 <==> WebSocketMessage.Length (int32)
/////     byte 10 ~ 10 + length <==> WebSocketMessage.Message
///// }
///// </summary>
///// <param name="bytes"></param>
///// <returns></returns>
    private fun SetAMessage(bytes: ByteArray, startIndex: Int, wsMessage: WebSocketMessage): Int {
        val wsMessageTypeCode: UByte = when (wsMessage.MessageType) {
            WebSocketMessageType.Text -> 0u
            WebSocketMessageType.Binary -> 1u
            else -> 2u
        }

        val isEffeciveSegment = ByteBuffer.wrap(
            bytes,
            startIndex + IS_EFFECTIVE_LOCATION,
            UINT8_LENGTH
        )
        val eventCodeSegment = ByteBuffer.wrap(
            bytes,
            startIndex + MESSAGE_EVENT_CODE_LOCATION,
            UINT32_LENGTH
        )
        val messageTypeSegment = ByteBuffer.wrap(
            bytes,
            startIndex + MESSAGE_TYPE_LOCATION,
            UINT8_LENGTH
        )
        val lengthSegment = ByteBuffer.wrap(
            bytes,
            startIndex + LENGTH_LOCATION,
            UINT32_LENGTH
        )
        val messageSegment = ByteBuffer.wrap(bytes, startIndex + MESSAGE_LOCATION, wsMessage.Length)
        isEffeciveSegment.put(IS_EFFECTIVE_CODE.toByte())
        eventCodeSegment.order(ByteOrder.LITTLE_ENDIAN).putInt(wsMessage.EventCode.Code)
        messageTypeSegment.put(wsMessageTypeCode.toByte())
        lengthSegment.order(ByteOrder.LITTLE_ENDIAN).putInt(wsMessage.Length)
        if (wsMessage.MessageType == WebSocketMessageType.Text)//Text
        {
            val messageStr = wsMessage.Message as String

            messageSegment.put(messageStr.toByteArray())

        } else if (wsMessage.MessageType == WebSocketMessageType.Binary) {
            messageSegment.put(wsMessage.Message as ByteArray)
        }

        return wsMessage.Length + HEADER_LENGTH;


    }

    fun SendMessages(wsMessages: ArrayList<WebSocketMessage>) {

        var totalLength = 0;
        wsMessages.forEach {
            totalLength += it.Length + HEADER_LENGTH
        }
        val bytes = ByteArray(totalLength)
        var byteOffset = 0
        wsMessages.forEach {
            byteOffset += SetAMessage(bytes, byteOffset, it);
        }

        websocket.Send(ByteBuffer.wrap(bytes).toByteString())

    }
}