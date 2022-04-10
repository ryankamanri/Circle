import { Site, Sleep, ParseElement, Stringlify, ParseFunc, StrIncrement, Animate, GetType, CopyElement ,GenerateIDString } from "./Kamanri/Utils.js";
import { Mutex, Critical } from "./Kamanri/Mutex.js";
import { Api } from "./Kamanri/Api.js";
import { Router } from "./Kamanri/Router.js";
import MyWebSocket from "./Kamanri/MyWebSocket.js";
import ModelView from "./Kamanri/ModelView.js";
import Storage from "./Kamanri/Storage.js";

let api = new Api();

async function Configuration(prop) {
	let config = await api.Get("/Configuration.json");
	return config[prop];
	
}

export {
	Site, Sleep, ParseElement, Stringlify, ParseFunc, StrIncrement, Animate, GetType, CopyElement, GenerateIDString,
	Mutex, Critical, 
	Api, 
	Router,
	MyWebSocket, 
	ModelView, 
	Storage, 
	Configuration
}