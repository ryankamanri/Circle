import { Site, Sleep, parseElement, stringlify, parseFunc } from "./Kamanri/Utils.js";
import { Mutex, Critical } from "./Kamanri/Mutex.js";
import { Api } from "./Kamanri/Api.js";
import MyWebSocket from "./Kamanri/MyWebSocket.js";
import ModelView from "./Kamanri/ModelView.js";
import Storage from "./Kamanri/Storage.js";

let api = new Api();

async function Configuration(prop) {
    let config = await api.Get("/Configuration.json");
    return config[prop];
    
}

export{
    Site, Sleep, parseElement, stringlify, parseFunc, 
    Mutex, Critical, 
    Api, 
    MyWebSocket, 
    ModelView, 
    Storage, 
    Configuration
}