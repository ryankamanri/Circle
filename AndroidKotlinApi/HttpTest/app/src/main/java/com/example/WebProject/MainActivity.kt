package com.example.WebProject

import android.os.Build
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.TextView
import androidx.annotation.RequiresApi
import com.example.WebProject.databinding.ActivityMainBinding
import com.example.WebProject.models.Message
import com.example.WebProject.models.Tag
import com.example.WebProject.mykits.Api
import com.example.WebProject.mykits.MyWebSocket
import com.google.gson.Gson
import org.json.JSONArray
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.*
import kotlin.concurrent.thread
import kotlin.properties.Delegates

class MainActivity : AppCompatActivity() {
    companion object {
        val gson : Gson = Gson()
        var assignedID : Long = 0
        lateinit var api : Api
        lateinit var myWebSocket : MyWebSocket
        lateinit var mainBinding : ActivityMainBinding
    }
    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        mainBinding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(mainBinding.root)
        // api类的初始化
        api = Api("http://192.168.1.104:5031")
        myWebSocket = MyWebSocket("ws://192.168.1.104:5042")

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // WebSocket 双向通信
        // AddEventHandler 添加事件处理程序 此处为响应 `OnServerConnect` 服务端接受连接事件
        // 传入参数it和返回值均为 `ArrayList<WebSocketMessage>` 分别为来自服务器的消息和要返回给服务器的消息
        // 也可以使用 myWebSocket.SendMessages 单独向服务端发送信息
        myWebSocket.AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerConnect) {
            Log.i(toString(), it.toString())
            Thread.sleep(1000)

            // 涉及到UI的代码一定要放到runOnUiThread里, 否则报错
            runOnUiThread {
                val textView = TextView(this)
                textView.text = it.toString()
                mainBinding.textLayout.addView(textView);
            }
            assignedID = (it[0].Message as String).toLong()
            thread {
                // 这个消息必须发送, 且作为第一条, 表示要作为哪个用户连接
                myWebSocket.SendMessages(
                    arrayListOf(
                        MyWebSocket.WebSocketMessage(
                            MessageEvent = MyWebSocket.WebSocketMessageEvent.OnClientConnect,
                            MessageType = MyWebSocket.WebSocketMessageType.Text,
                            Message = assignedID.toString()
                        ),
                        MyWebSocket.WebSocketMessage(
                            MessageEvent = MyWebSocket.WebSocketMessageEvent.OnClientConnect,
                            MessageType = MyWebSocket.WebSocketMessageType.Text,
                            Message = 8.toString() // 8 Is User ID
                        )
                    )
                )
//                myWebSocket.SendMessages(
//                    arrayListOf(
//                        MyWebSocket.WebSocketMessage(
//                            MessageEvent = MyWebSocket.WebSocketMessageEvent.OnClientPreviousMessage,
//                            MessageType = MyWebSocket.WebSocketMessageType.Text,
//                            Message = 8.toString()
//                        ),
//                        MyWebSocket.WebSocketMessage(
//                            MessageEvent = MyWebSocket.WebSocketMessageEvent.OnClientPreviousMessage,
//                            MessageType = MyWebSocket.WebSocketMessageType.Text,
//                            Message = 2.toString()
//                        ))
//                )
            }


            return@AddEventHandler arrayListOf<MyWebSocket.WebSocketMessage>()
        }

        myWebSocket.AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerPreviousMessage){
            runOnUiThread {
                val textView = TextView(this)
                textView.text = it.toString()
                mainBinding.textLayout.addView(textView);
            }
            Log.i(toString(), "Receive ${it.size} Messages")
            return@AddEventHandler arrayListOf<MyWebSocket.WebSocketMessage>()
        }

        mainBinding.button.setOnClickListener {
            val sendMessageText = mainBinding.editText.text
            val formatter = DateTimeFormatter.ofPattern("MM/dd/yyyy HH:mm:ss")
            val sendMessage = Message(
                ID = 0,
                SendUserID = 8,
                ReceiveID = 2,
                IsGroup = false,
                Time = Date(),
                ContentType = "text/plain",
                Content = sendMessageText.toString()
            )
            myWebSocket.SendMessages(
                arrayListOf(
                    MyWebSocket.WebSocketMessage(
                        MessageEvent = MyWebSocket.WebSocketMessageEvent.OnClientMessage,
                        MessageType = MyWebSocket.WebSocketMessageType.Text,
                        Message = gson.toJson(sendMessage)
                    )
                )
            )
            runOnUiThread {
                val textView = TextView(this)
                textView.text = "[${sendMessage.Time.toString()}]  Me ${sendMessage.SendUserID} : ${sendMessage.Content}"
                mainBinding.textLayout.addView(textView)
            }
        }

        myWebSocket.AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerTempMessage) {
            val resMessage = gson.fromJson(it[0].Message as String, Message::class.java)
            runOnUiThread {
                val textView = TextView(this)
                textView.text = "[${resMessage.Time.toString()}]  User ${resMessage.SendUserID} : ${resMessage.Content}"
                mainBinding.textLayout.addView(textView)
            }
            return@AddEventHandler arrayListOf<MyWebSocket.WebSocketMessage>()

        }

        myWebSocket.AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerMessage){
            val resMessage = gson.fromJson(it[0].Message as String, Message::class.java)
            runOnUiThread {
                val textView = TextView(this)
                textView.text = "[${resMessage.Time.toString()}]  User ${resMessage.SendUserID} : ${resMessage.Content}"
                mainBinding.textLayout.addView(textView)
            }

            return@AddEventHandler arrayListOf<MyWebSocket.WebSocketMessage>()
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // http 通信
        // 分为 `Get` 和 `Post` 方法, 这里使用的是Post方法

//        mainBinding.button.setOnClickListener {
//            api.Post("/Tag/FindChildTag", { //这里请求的uri将返回一个包含所有子标签的数组
//                // setRequestBodyHandler
//                // 表示 `设置POST请求要发送的表单` 的处理函数
//                // 这个闭包函数传入参数为it : FormBody.BuilderD, it为要设置的表单
//                // 可使用it.add()方法添加键值对
//                it.add("ParentTag",
//                        gson.toJson(
//                            Tag( mainBinding.editText.text.toString().toInt(),// 这里将EditText中输入的值作为 `Tag` 的ID
//                                ""
//                            )
//                        )
//                    )
//                }
//            ) {
//                // setOnResponseHandler
//                // 表示 `设置如何处理返回的JSON字符串` 的处理函数
//                // 这个闭包函数传入参数为it : String, it为返回的字符串
//                // 如果返回的json字符串以 `{}` 包括, 使用JSONObject(it)将其处理为json对象
//                // 如果返回的json字符串以 `[]` 包括, 使用JSONArray(it)将其处理为json数组
//                val resStr = it.body!!.string()
//                Log.i("info", "Receive Response Text : ${resStr}")
//                val jarr = JSONArray(resStr)
//                var formedText : String = ""
//                for (index in 0..jarr.length() - 1){
//                    val tag = gson.fromJson(jarr[index].toString(), Tag::class.java)
//                    formedText += "${tag.toString()} \n"
//                }
//                // 涉及到UI的代码一定要放到runOnUiThread里, 否则报错
//                runOnUiThread {
//                    MainActivity.mainBinding.text.text = formedText
//                }
//
//
//            }
//
//
//        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////

    }

}