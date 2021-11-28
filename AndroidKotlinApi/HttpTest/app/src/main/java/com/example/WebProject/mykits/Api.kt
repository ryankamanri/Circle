package com.example.WebProject.mykits

import android.app.Activity
import android.util.Log
import okhttp3.*
import okhttp3.MediaType.Companion.toMediaType
import java.io.IOException

class Api(host : String) {
    val ENCTYPE = "application/x-www-form-urlencoded"
    val Host : String = host
    private val httpClient : OkHttpClient = OkHttpClient()

    fun Get(uri : String, setOnResponseHandler : (Response) -> Unit) {
        val builder = Request.Builder()
            .url("${Host}${uri}")
            .get()

        httpClient.newCall(builder.build()).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                Log.e("CallError", e.toString())
            }

            override fun onResponse(call: Call, response: Response) {
                try {
                    setOnResponseHandler(response)
                }catch (e : Exception) {
                    Log.e(toString(),"An Exception Occured While Executing The setOnResponseHandler \n Caused By : $e : ${e.message}")
                    e.printStackTrace()
                }

            }

        })
    }

    fun Post(uri: String, setRequestBodyHandler : (FormBody.Builder) -> Unit, setOnResponseHandler: (Response) -> Unit){
        val mediaType = ENCTYPE.toMediaType()
        val requestBody = FormBody.Builder();
        setRequestBodyHandler(requestBody)

        val builder = Request.Builder()
            .url("${Host}${uri}")
            .post(requestBody.build())

        httpClient.newCall(builder.build()).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                Log.e("CallError", e.toString())
            }

            override fun onResponse(call: Call, response: Response) {
                try {
                    setOnResponseHandler(response)
                }catch (e : Exception) {
                    Log.e(toString(),"An Exception Occured While Executing The setOnResponseHandler \n Caused By : $e : ${e.message}")
                    e.printStackTrace()
                }

            }

        })
    }
}