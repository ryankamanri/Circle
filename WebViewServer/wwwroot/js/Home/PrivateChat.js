import { MyWebSocket, ModelView, Api, Storage, Configuration, ShowAlert } from "../My.js";
import UserInfo from "../Shared/Components/PrivateChat/UserInfo.js";
import MessageItem from "../Shared/Components/PrivateChat/MessageItem.js";

// const wsuri = await Configuration("WebSocketUri");

let userInfo;
let objectUserInfo;
let assignID;
let objectID_MessageDict = { "0": [] };
let focusUserModelList = new ModelView.ModelList([]);
let focusUserModelListView = new ModelView.ModelView(focusUserModelList, document.createElement('div'));

let messageViewModelList = new ModelView.ModelList([]);
let messageViewList_ContentView = new ModelView.ModelView(messageViewModelList, document.createElement('div'));

const objectID_MessageViewModelList_Pairs = [{ "0": messageViewModelList }];


let api;
let mywebsocket;

const storage = Storage.Storage();

async function Init(services) {

	api = services.Api;
	mywebsocket = services.MyWebSocket;

	await InitSelfInfo(services);

	UserInfo.Init(services);

	MessageItem.Init(services);

	await InitFocusUserList();

	InitMessageContentView(messageViewModelList);

	InitSendMessage();

}

// 每次刷新都初始化, 用于websocket等
async function InitBase(services){
	InitReceiveMessage(services);
}

async function InitSelfInfo(services) {
	userInfo = services.UserInfo;

	storage.SelfInfo = userInfo;
	if (storage[`${userInfo.ID}_objectID_MessageArray`] !== undefined) {
		objectID_MessageDict = storage[`${userInfo.ID}_objectID_MessageArray`];

		for (let i in objectID_MessageDict) {
			objectID_MessageViewModelList_Pairs[i] = new ModelView.ModelList(objectID_MessageDict[i]);
		}

	}
}

async function InitFocusUserList() {
	let focusUserList;
	let userInfoListMount = document.querySelector("#userinfolist-mount");
	// init userlist
	if(storage[`${userInfo.ID}_focusUserInfoList`] !== undefined) {
		focusUserList = storage[`${userInfo.ID}_focusUserInfoList`];
	} else {
		let resData = await api.Post(
			"/Api/User/SelectUserInfoInitiative",
			{
				Selections: JSON.stringify({
					Type: ["Focus"]
				})
			});
		if (resData === "Bad Request") {
			console.error("Bad Request");
			return;
		}
		focusUserList = JSON.parse(resData);
		storage[`${userInfo.ID}_focusUserInfoList`] = focusUserList;
	}
	
	focusUserModelList = new ModelView.ModelList(focusUserList);
	focusUserModelListView = new ModelView.ModelView(focusUserModelList, userInfoListMount);
	focusUserModelListView.Clean();
	focusUserModelListView
		.SetItemTemplate(viewType => {

			return UserInfo.SetItemTemplate(viewType);
		})
		.SetTemplateViewToModelBinder((view, modelItem, viewType) => {

			UserInfo.SetTemplateViewToModelBinder(view, modelItem, viewType);

			let userinfoView = view;
			userinfoView.addEventListener("click", () => {
				document.querySelectorAll(".userinfo").forEach(viewItem => viewItem.style.background = "");
				userinfoView.style.background = "cadetblue";
				ShowMessageViewList(modelItem);
			});

			if (objectID_MessageDict[`${modelItem.ID}k`] === undefined) {
				// Create New Key Value(Message, MessageViewModelList)
				objectID_MessageDict[`${modelItem.ID}k`] = [];
				objectID_MessageViewModelList_Pairs[`${modelItem.ID}k`] = new ModelView.ModelList(objectID_MessageDict[`${modelItem.ID}k`]);
			}


		}).Show();

}

function ShowMessageViewList(modelItem) {
	objectUserInfo = modelItem;
	messageViewModelList = objectID_MessageViewModelList_Pairs[`${objectUserInfo.ID}k`];
	messageViewList_ContentView.RebindModelList(messageViewModelList);
	InitMessageContentView(messageViewModelList);
	console.log(`Chat With ${objectUserInfo.NickName}`);
	ShowAlert("alert alert-info", "私聊: ", `与 ${objectUserInfo.NickName} 聊天`, 800);
}

function InitMessageContentView(messageViewModelList) {

	let mountElement = document.querySelector("#message-content-mount");
	messageViewList_ContentView = new ModelView.ModelView(messageViewModelList, mountElement);
	messageViewList_ContentView.Clean();
	messageViewList_ContentView
		.SetItemViewType((modelItem) => {
			return MessageItem.SetItemViewType(modelItem, userInfo);
		})
		.SetItemTemplate((viewType) => {
			return MessageItem.SetItemTemplate(viewType);
		})
		.SetTemplateViewToModelBinder((view, modelItem, viewType) => {
			let messageUserID = modelItem.SendUserID;
			let messageUser;
			focusUserModelList.GetModelArray().forEach(focusUserModel => {
				if (focusUserModel.ID == messageUserID) {
					messageUser = focusUserModel;
					return;
				}
			});
			if (messageUser === undefined) messageUser = userInfo;
			return MessageItem.SetTemplateViewToModelBinder(view, modelItem, viewType, messageUser);
		}).Show();

	SetStackFromEnd();
}


function InitReceiveMessage(services) {

	services.MyWebSocket.AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerPreviousMessage, async (wsMessages) => {
		console.log(`\n${JSON.stringify(wsMessages)}`);
	}).AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerTempMessage, async (wsMessage) => {
		console.log(`\n${JSON.stringify(wsMessage)}`);
		var resMessages = wsMessage.ToMessages();
		for (const resMessage of resMessages) {
			
			let isInList = false;
			focusUserModelList.ForEach((modelItem, index) => {
				if (modelItem.ID === resMessage.SendUserID) {
					focusUserModelList.Change(index, focusUserModel => {
						focusUserModel.LastMessage = resMessage.Content;
						focusUserModel.Time = Get_MMSS_String(new Date(resMessage.Time));
					});
					isInList = true;
				}
			});
			if(isInList) {
				AddTimeMessage(resMessage, resMessage.SendUserID);
				objectID_MessageViewModelList_Pairs[`${resMessage.SendUserID}k`].Append(resMessage);
				storage[`${services.UserInfo.ID}_objectID_MessageArray`] = objectID_MessageDict;
				continue;
			}
			// not in userlist, request the user into list
			const sendUserInfo = await RequestUserInfo(services, resMessage.SendUserID);
			sendUserInfo.LastMessage = resMessage.Content;
			sendUserInfo.Time = Get_MMSS_String(new Date(resMessage.Time));
			focusUserModelList.InsertAt(0, sendUserInfo);

			objectID_MessageDict[`${resMessage.SendUserID}k`] = [];
			objectID_MessageViewModelList_Pairs[`${resMessage.SendUserID}k`] = new ModelView.ModelList(objectID_MessageDict[`${resMessage.SendUserID}k`]);
			AddTimeMessage(resMessage, resMessage.SendUserID);
			objectID_MessageViewModelList_Pairs[`${resMessage.SendUserID}k`].Append(resMessage);
			storage[`${services.UserInfo.ID}_objectID_MessageArray`] = objectID_MessageDict;
			
		}
	}).AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerMessage, async (wsMessage) => {
		console.log(`\n${JSON.stringify(wsMessage)}`);
		var resMessage = wsMessage.ToMessages()[0];
		
		let isInList = false;
		focusUserModelList.ForEach((modelItem, index) => {
			if (modelItem.ID === resMessage.SendUserID) {
				focusUserModelList.Change(index, focusUserModel => {
					focusUserModel.LastMessage = resMessage.Content;
					focusUserModel.Time = Get_MMSS_String(new Date(resMessage.Time));
				});
				isInList = true;
			}
				
		});
		if(isInList) {
			AddTimeMessage(resMessage, resMessage.SendUserID);
			objectID_MessageViewModelList_Pairs[`${resMessage.SendUserID}k`].Append(resMessage);
			storage[`${services.UserInfo.ID}_objectID_MessageArray`] = objectID_MessageDict;
			SetStackFromEnd();
			return;
		}
		// not in userlist, request the user into list
		const sendUserInfo = await RequestUserInfo(services, resMessage.SendUserID);
		sendUserInfo.LastMessage = resMessage.Content;
		sendUserInfo.Time = Get_MMSS_String(new Date(resMessage.Time));
		focusUserModelList.InsertAt(0, sendUserInfo);

		objectID_MessageDict[`${resMessage.SendUserID}k`] = [];
		objectID_MessageViewModelList_Pairs[`${resMessage.SendUserID}k`] = new ModelView.ModelList(objectID_MessageDict[`${resMessage.SendUserID}k`]);
		AddTimeMessage(resMessage, resMessage.SendUserID);
		objectID_MessageViewModelList_Pairs[`${resMessage.SendUserID}k`].Append(resMessage);
		storage[`${services.UserInfo.ID}_objectID_MessageArray`] = objectID_MessageDict;

	});

}

async function ChatWithAUser(services, userID) {
	const myID = services.UserInfo.ID;
	const focusUserList = storage[`${myID}_focusUserInfoList`];
	let isInList = false;
	focusUserList.forEach((modelItem, index) => {
		if (modelItem.ID === userID) {
			isInList = true;
		}
	});
	if(isInList) return;
	
	const sendUserInfo = await RequestUserInfo(services, userID);
	focusUserList.push(sendUserInfo);
	storage[`${myID}_focusUserInfoList`] = focusUserList;
	
}

async function RequestUserInfo(services, userID) {
	let resData = await services.Api.Post(
		"/Api/User/GetUserInfo",
		{
			User: JSON.stringify({
				ID: userID
			})
		});
	if (resData === "null") {
		console.error("Response Data Is Null");
		return;
	}
	return JSON.parse(resData);
}

function InitSendMessage() {
	document.querySelector("#button-submit").onclick = (ele, ev) => {
		let sendMessageText = document.querySelector(".input1").value;
		if (sendMessageText == '') return;
		document.querySelector(".input1").value = '';
		let sendMessage = {
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
		// get the last message time by Apr 19 2022.
		AddTimeMessage(sendMessage, objectUserInfo.ID);
		//
		objectID_MessageViewModelList_Pairs[`${objectUserInfo.ID}k`].Append(sendMessage);
		SetStackFromEnd();
		storage[`${userInfo.ID}_objectID_MessageArray`] = objectID_MessageDict;
		focusUserModelList.ForEach((modelItem, index) => {
			if (modelItem.ID == sendMessage.ReceiveID)
				focusUserModelList.Change(index, focusUserModel => {
					focusUserModel.LastMessage = sendMessage.Content;
					focusUserModel.Time = Get_MMSS_String(sendMessage.Time);
				});
		});

	}
}

function AddTimeMessage(message, objectID) {

	if(objectID_MessageViewModelList_Pairs[`${objectID}k`] === undefined) {
		// Create New Key Value(Message, MessageViewModelList)
		objectID_MessageDict[`${objectID}k`] = [];
		objectID_MessageViewModelList_Pairs[`${objectID}k`] = new ModelView.ModelList(objectID_MessageDict[`${objectID}k`]);
	}

	let length = objectID_MessageViewModelList_Pairs[`${objectID}k`].GetLength();
	let lastMessageTime;

	if (length !== 0) {
		lastMessageTime = objectID_MessageViewModelList_Pairs[`${objectID}k`].Get(length - 1).Time;
		if (new Date(message.Time) - new Date(lastMessageTime) < 5 * 60 * 1000) return;
	}

	let timeMessage = {
		ID: 0,
		SendUserID: 0,
		ReceiveID: 0,
		IsGroup: false,
		Time: message.Time,
		ContentType: "text/plain",
		Content: ""
	};
	objectID_MessageViewModelList_Pairs[`${objectID}k`].Append(timeMessage);

}

function Get_MMSS_String(date) {
	let hour = date.getHours();
	let minute = date.getMinutes();
	if (hour < 10) {
		hour = `0${hour}`;
	}
	if (minute < 10) {
		minute = `0${minute}`;
	}
	return `${hour}:${minute}`;
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
	Init, InitBase
}


export default {
	Init, InitBase, ChatWithAUser
}