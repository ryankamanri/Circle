package com.example.WebProject.models

import java.time.LocalDateTime
import java.util.*

data class Message(
    val ID: Long,
    val SendUserID: Long,
    val ReceiveID: Long,
    val IsGroup: Boolean,
    val Time: Date,
    val ContentType: String,
    val Content: Any
){
}