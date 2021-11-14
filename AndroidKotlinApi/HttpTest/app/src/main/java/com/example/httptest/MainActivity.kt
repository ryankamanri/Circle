package com.example.httptest

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import com.example.httptest.databinding.ActivityMainBinding
import com.example.httptest.models.Tag
import com.example.httptest.mykits.Api
import com.example.httptest.mykits.MyWebSocket
import com.google.gson.Gson
import org.json.JSONArray
import org.json.JSONObject
import kotlin.reflect.typeOf

class MainActivity : AppCompatActivity() {
    companion object {
        val gson : Gson = Gson()
        lateinit var api : Api
        lateinit var myWebSocket : MyWebSocket
        lateinit var mainBinding : ActivityMainBinding
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        mainBinding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(mainBinding.root)
        // api类的初始化
        api = Api("http://192.168.1.104:5030")
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
                val wsMessages = it as ArrayList<MyWebSocket.WebSocketMessage>
                mainBinding.text.text = "${wsMessages[0].Message} \n ${wsMessages[1].Message}"
            }


            return@AddEventHandler arrayListOf<MyWebSocket.WebSocketMessage>(
                MyWebSocket.WebSocketMessage(
                    MyWebSocket.WebSocketMessageEvent.OnClientConnect,
                    MyWebSocket.WebSocketMessageType.Text,
                    "你你你"
                ),
                MyWebSocket.WebSocketMessage(
                    MyWebSocket.WebSocketMessageEvent.OnClientConnect,
                    MyWebSocket.WebSocketMessageType.Text,
                    "我我我"
                )
            )
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
//                Log.i("info", "Receive Response Text : ${it}")
//                val jarr = JSONArray(it)
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


//        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////

    }

}