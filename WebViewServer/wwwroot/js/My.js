import { Site, Sleep, ParseElement, Stringlify, ParseFunc, StrIncrement, Animate, GetType, CopyElement ,GenerateIDString, AddScript } from "../lib/Kamanri/Utils.js";
import { Mutex, Critical } from "../lib/Kamanri/Mutex.js";
import { Api } from "../lib/Kamanri/Api.js";
import { Router } from "../lib/Kamanri/Router.js";
import MyWebSocket from "../lib/Kamanri/MyWebSocket.js";
import ModelView from "../lib/Kamanri/ModelView.js";
import Storage from "../lib/Kamanri/Storage.js";

let api = new Api();

async function Configuration(prop) {
	let config = await api.Get("/Configuration.json");
	return config[prop];
	
}

export {
	Site, Sleep, ParseElement, Stringlify, ParseFunc, StrIncrement, Animate, GetType, CopyElement, GenerateIDString, AddScript,
	Mutex, Critical, 
	Api, 
	Router,
	MyWebSocket, 
	ModelView, 
	Storage, 
	Configuration
}