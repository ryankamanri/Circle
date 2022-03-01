import { MyWebSocket, ModelView, Api, Storage, Configuration } from "../My.js";

let wsuri = await Configuration("WebSocketUri");

let userInfo;
let objectUserInfo;

let focusUserModelList = new ModelView.ModelList([]);
let focusUserModelListView = new ModelView.ModelView(focusUserModelList, document.createElement('div'));

let messageViewModelList = new ModelView.ModelList([]);
let messageViewList_ContentView = new ModelView.ModelView(messageViewModelList, document.createElement('div'));

let objectID_MessageViewModelList_Pairs = [{ "0": messageViewModelList }];
let objectID_MessageDict = { "0": [] };

let api = new Api();
let mywebsocket = new MyWebSocket.MyWebSocket(wsuri);
let assignID;
let storage = Storage.Storage();


async function PrivateChat() {


	await InitSelfInfo();

	await InitFocusUserList();

	InitMessageContentView(messageViewModelList);

	InitMyWebSocket();


}

async function InitSelfInfo() {
	let resData = await api.Post("/Api/User/GetSelfInfo", {});
	userInfo = JSON.parse(resData);
	storage.SelfInfo = userInfo;
	if (storage.objectID_MessageArray != undefined) {
		objectID_MessageDict = storage.objectID_MessageArray;


		for (let i in objectID_MessageDict) {
			objectID_MessageViewModelList_Pairs[i] = new ModelView.ModelList(objectID_MessageDict[i]);
		}

	}
}

async function InitFocusUserList() {
	let focusUserList;
	let userInfoListMount = document.querySelector("#userinfolist-mount");
	let resData = await api.Post(
		"/Api/User/SelectUserInfoInitiative",
		{
			Selections: JSON.stringify({
				Type: ["Focus"]
			})
		});
	if (resData == "Bad Request") {
		console.error("Bad Request");
		return;
	}
	focusUserList = JSON.parse(resData);
	focusUserModelList = new ModelView.ModelList(focusUserList);
	focusUserModelListView = new ModelView.ModelView(focusUserModelList, userInfoListMount);
	focusUserModelListView.Clean();
	focusUserModelListView
		.SetItemTemplate(viewType => {
			let templateDivElement = document.querySelector("#userinfo-template");
			templateDivElement.style.display = 'none';
			let templateElement = document.createElement("template");
			templateElement.innerHTML = templateDivElement.innerHTML;
			return templateElement;
		})
		.SetTemplateViewToModelBinder((view, modelItem, viewType) => {
			let headImage = view.querySelector(".chat-head-image img");
			let nickName = view.querySelector(".nickname");
			let time = view.querySelector(".time");
			let lastMessage = view.querySelector(".last-message");
			headImage.setAttribute("src", modelItem.HeadImage);
			nickName.innerText = modelItem.NickName;
			if (modelItem.Time != undefined)
				time.innerText = modelItem.Time;
			if (modelItem.LastMessage != undefined)
				lastMessage.innerText = modelItem.LastMessage;
			let userinfoView = view;
			userinfoView.addEventListener("click", () => {
				document.querySelectorAll(".userinfo").forEach(viewItem => viewItem.style.background = "");
				userinfoView.style.background = "cadetblue";
				ShowMessageViewList(modelItem);
			});
			if (objectID_MessageViewModelList_Pairs[String(modelItem.ID) + "k"] == undefined)
				objectID_MessageViewModelList_Pairs[String(modelItem.ID) + "k"] = new ModelView.ModelList([]);
			if (objectID_MessageDict[String(modelItem.ID) + "k"] == undefined)
				objectID_MessageDict[String(modelItem.ID) + "k"] = [];

		}).Show();

}

function ShowMessageViewList(modelItem) {
	objectUserInfo = modelItem;
	messageViewModelList = objectID_MessageViewModelList_Pairs[String(objectUserInfo.ID) + "k"];
	messageViewList_ContentView.RebindModelList(messageViewModelList);
	InitMessageContentView(messageViewModelList);
	console.log(`Chat With ${objectUserInfo.NickName}`);
}

function InitMessageContentView(messageViewModelList) {

	let mountElement = document.querySelector("#message-content-mount");
	messageViewList_ContentView = new ModelView.ModelView(messageViewModelList, mountElement);
	messageViewList_ContentView.Clean();
	messageViewList_ContentView
		.SetItemViewType((modelItem) => {
			if (modelItem.ReceiveID == userInfo.ID) return 0;
			else if (modelItem.SendUserID == userInfo.ID) return 1;
			else return 2;
		})
		.SetItemTemplate((viewType) => {
			let templateDivElement;
			if (viewType == 0) templateDivElement = document.querySelector('#other-message-item-template');
			else if (viewType == 1) templateDivElement = document.querySelector('#self-message-item-template');
			else templateDivElement = document.querySelector('#time-message-item-template');
			templateDivElement.style.display = 'none';
			let templateElement = document.createElement("template");
			templateElement.innerHTML = templateDivElement.innerHTML;
			return templateElement;
		})
		.SetTemplateViewToModelBinder((view, modelItem, viewType) => {
			if (viewType == 2) {
				let time = view.querySelector(".time-message-item");
				time.innerText = modelItem.Time;
				return;
			}

			let messageUserID = modelItem.SendUserID;
			let messageUser;
			focusUserModelList.GetModelArray().forEach(focusUserModel => {
				if (focusUserModel.ID == messageUserID) {
					messageUser = focusUserModel;
					return;
				}
			});
			let headImage = view.querySelector(".chat-head-image img");
			let message = view.querySelector(".message");
			headImage.setAttribute("src", messageUser.HeadImage);
			message.innerText = modelItem.Content;
		}).Show();

	SetStackFromEnd();
}


function InitMyWebSocket() {

	mywebsocket.AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerConnect, async (wsMessages) => {
		console.log(wsMessages);
		console.log(`\n${JSON.stringify(wsMessages)}`);
		assignID = new Number(wsMessages[0].Message);
		if (userInfo != undefined) {
			await new Promise(resolve => {
				mywebsocket.SendMessages([
					new MyWebSocket.WebSocketMessage(
						MyWebSocket.WebSocketMessageEvent.OnClientConnect,
						MyWebSocket.WebSocketMessageType.Text,
						assignID.toString()
					),
					new MyWebSocket.WebSocketMessage(
						MyWebSocket.WebSocketMessageEvent.OnClientConnect,
						MyWebSocket.WebSocketMessageType.Text,
						String(userInfo.ID)
					)
				]);
			});
		}
	}).AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerPreviousMessage, async (wsMessages) => {
		console.log(`\n${JSON.stringify(wsMessages)}`);
	}).AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerTempMessage, async (wsMessage) => {
		console.log(`\n${JSON.stringify(wsMessage)}`);
		var resMessages = wsMessage.ToMessages();
		resMessages.forEach(resMessage => {
			objectID_MessageViewModelList_Pairs[String(resMessage.SendUserID) + "k"].Append(resMessage);
			objectID_MessageDict[String(resMessage.SendUserID) + "k"].push(resMessage);
			storage.objectID_MessageArray = objectID_MessageDict;
			focusUserModelList.ForEach((modelItem, index) => {
				if (modelItem.ID == resMessage.SendUserID)
					focusUserModelList.Change(index, focusUserModel => focusUserModel.LastMessage = resMessage.Content);
			});
			
		});
	}).AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerMessage, async (wsMessage) => {
		console.log(`\n${JSON.stringify(wsMessage)}`);
		var resMessage = wsMessage.ToMessages()[0];
		objectID_MessageViewModelList_Pairs[String(resMessage.SendUserID) + "k"].Append(resMessage);
		objectID_MessageDict[String(resMessage.SendUserID) + "k"].push(resMessage);
		storage.objectID_MessageArray = objectID_MessageDict;
		focusUserModelList.ForEach((modelItem, index) => {
			if (modelItem.ID == resMessage.SendUserID)
				focusUserModelList.Change(index, focusUserModel => focusUserModel.LastMessage = resMessage.Content);
		});

	}).Open();

	document.querySelector("#button-submit").onclick = (ele, ev) => {
		var sendMessageText = document.querySelector(".input1").value;
		var sendMessage = {
			ID: 0,
			SendUserID: userInfo.ID,
			ReceiveID: objectUserInfo.ID,
			IsGroup: false,
			Time: new Date(),
			ContentType: "text/plain",
			Content: sendMessageText
		};
		mywebsocket.SendMessages([
			new MyWebSocket.WebSocketMessage(
				MyWebSocket.WebSocketMessageEvent.OnClientMessage,
				MyWebSocket.WebSocketMessageType.Text,
				JSON.stringify(sendMessage)
			)
		]);
		objectID_MessageViewModelList_Pairs[String(objectUserInfo.ID) + "k"].Append(sendMessage);
		objectID_MessageDict[String(objectUserInfo.ID) + "k"].push(sendMessage);
		storage.objectID_MessageArray = objectID_MessageDict;
		focusUserModelList.ForEach((modelItem, index) => {
			if (modelItem.ID == sendMessage.ReceiveID)
				focusUserModelList.Change(index, focusUserModel => focusUserModel.LastMessage = sendMessage.Content);
		});

	}

	
}

function SetStackFromEnd() {
	let messageContentView = document.querySelector(".message-content-view");
	let messageCountMount = document.querySelector("#message-content-mount");
	messageContentView.scrollTop = messageCountMount.offsetHeight;

}

Array.prototype.ToWebSocketMessageList = function (wsmEvent) {
	let msgList = [];
	this.forEach(message => {
		if (message.ContentType == "text/plain") {
			msgList.push(new MyWebSocket.WebSocketMessage(
				wsmEvent,
				MyWebSocket.WebSocketMessageType.Text,
				JSON.stringify(message)
			));
		}
		else {
			let bytes = message.Content;
			message.Content = null;
			msgList.push(new MyWebSocket.WebSocketMessage(
				wsmEvent,
				MyWebSocket.WebSocketMessageType.Text,
				JSON.stringify(message)
			));
			msgList.push(new MyWebSocket.WebSocketMessage(
				wsmEvent,
				MyWebSocket.WebSocketMessageType.Binary,
				bytes
			));
			message.Content = bytes;
		}
	});
	return msgList;
}

Array.prototype.ToMessages = function (offset = 0, length = -1) {
	let result = [];
	let _offset = offset;
	while (_offset < this.length && (_offset - offset < length || length == -1)) {
		let message = JSON.parse(String(this[_offset].Message));
		if (message.ContentType != "text/plain") {
			let bytesContent = this[_offset + 1].Message;
			message.Content = bytesContent;
			result.push(message);
			_offset += 2;
		}
		else {
			result.push(message);
			_offset++;
		}
	}
	return result;
}

export {
	PrivateChat
}


export default {
	PrivateChat
}