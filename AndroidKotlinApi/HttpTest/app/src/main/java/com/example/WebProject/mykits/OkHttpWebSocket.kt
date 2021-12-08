package com.example.WebProject.mykits

import android.icu.util.TimeUnit
import android.util.Log
import okhttp3.*
import okio.ByteString
import java.lang.Exception

class OkHttpWebSocket(url: String) {
    val MAX_RECONNECT_COUNT = 10
    val webSocketUrl = url
    var reconnectCount = 0
    private var IsOpen = false
    private var IsClosed = false
    private var ConnectionMutex = false
    lateinit var webSocket: WebSocket
    lateinit var OnOpen: (response: Response) -> Unit
    lateinit var OnMessage: (bytes: ByteString) -> Unit
    lateinit var OnClosed: (code: Int, reason: String) -> Unit
    lateinit var OnFaliure: (t: Throwable, response: Response?) -> Unit

    init {
        WebSocketConnect()
    }

    private fun WebSocketConnect() {
        // ...
        val httpClient = OkHttpClient.Builder()
            .pingInterval(40, java.util.concurrent.TimeUnit.SECONDS) // 设置 PING 帧发送间隔
            .build()
        val request = Request.Builder()
            .url(webSocketUrl)
            .build()
        httpClient.newWebSocket(request, object : WebSocketListener() {
            override fun onOpen(wSocket: WebSocket, response: Response) {
                super.onOpen(wSocket, response)
                // WebSocket 连接建立
                webSocket = wSocket
                IsOpen = true
                ConnectionMutex = true
                OnOpen(response)
            }


            override fun onMessage(wSocket: WebSocket, bytes: ByteString) {
                super.onMessage(webSocket, bytes)
                OnMessage(bytes)
            }

            override fun onClosed(wSocket: WebSocket, code: Int, reason: String) {
                super.onClosed(webSocket, code, reason)
                // WebSocket 连接关闭
                IsClosed = true
                ConnectionMutex = false
                OnClosed(code, reason)
            }

            override fun onFailure(wSocket: WebSocket, t: Throwable, response: Response?) {
                super.onFailure(wSocket, t, response)
                // 出错了

                Log.e(
                    toString(),
                    "Failed To Correspond With The Server \n Caused By $t : ${t.message} \n "
                )
                t.printStackTrace()
                Log.i(
                    toString(),
                    "Try To Reconnect To $webSocketUrl... ,Reconnect Count : $reconnectCount"
                )
                while (!IsOpen && !IsClosed && reconnectCount < MAX_RECONNECT_COUNT) {
                    Thread.sleep(5000)
                    reconnectCount++
                    WebSocketConnect()
                }

                ConnectionMutex = false
                if (!IsOpen) Log.w(toString(), "Connection Has NOT Been Opened")
                if (IsClosed) Log.w(toString(), "Connection Had Been Closed")
                if (reconnectCount >= MAX_RECONNECT_COUNT)
                    Log.e(toString(), "Reach the maximum number of reconnections")
                OnFaliure(t, response)
            }
        })
    }

    fun TryReconnect() : Boolean{
        if(!ConnectionMutex){
            WebSocketConnect()
            return true
        }
        return false
    }

    fun Send(bytes: ByteString) {
        try {
            if (IsOpen && !IsClosed) {
                webSocket.send(bytes)
            }
        } catch (e: Exception) {
            throw Exception("Failed To Send Message \n Caused By : ", e)
        }


    }
}