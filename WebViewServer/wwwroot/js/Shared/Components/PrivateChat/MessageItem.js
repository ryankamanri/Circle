import { Api } from "../../../My.js";

const api = new Api();

const componentBaseUrl = "/Shared/Components/PrivateChat";

const timeMessageItemStr = await api.Get(`${componentBaseUrl}/TimeMessageItem`);
const selfMessageItemStr = await api.Get(`${componentBaseUrl}/SelfMessageItem`);
const otherMessageItemStr = await api.Get(`${componentBaseUrl}/OtherMessageItem`);


function Init(services) {

}

function SetItemViewType(modelItem, userInfo) {
    if (modelItem.ReceiveID == userInfo.ID) return 0;
    else if (modelItem.SendUserID == userInfo.ID) return 1;
    else return 2;
}

function SetItemTemplate(viewType) {
    let templateDivElement;
    if (viewType == 0) templateDivElement = otherMessageItemStr;
    else if (viewType == 1) templateDivElement = selfMessageItemStr;
    else templateDivElement = timeMessageItemStr;

    let templateElement = document.createElement("template");
    templateElement.innerHTML = templateDivElement;
    return templateElement;
}

function SetTemplateViewToModelBinder(view, modelItem, viewType, messageUser) {
    if (viewType == 2) {
        let time = new Date(modelItem.Time);
        view.innerText = Get_MMSS_String(time);
        return;
    }
    let headImage = view.querySelector(".chat-head-image img");
    let message = view.querySelector(".message");
    headImage.setAttribute("src", messageUser.HeadImage);
    message.innerText = modelItem.Content;
}

export default {
    Init, SetItemViewType, SetItemTemplate, SetTemplateViewToModelBinder
}

function Get_MMSS_String(date) {
	let hour = date.getHours();
	let minute = date.getMinutes();
	if(hour < 10) {
		hour = `0${hour}`;
	}
	if(minute < 10) {
		minute = `0${minute}`;
	}
	return `${hour}:${minute}`;
}
