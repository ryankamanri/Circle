import { Api } from "../../../My.js";

const componentBaseUrl = "/Shared/Components/PrivateChat";
const userInfoStr = await new Api().Get(`${componentBaseUrl}/UserInfo`);

function Init(services){
    return userInfoStr;
}

function SetItemTemplate(viewType) {
    let templateElement = document.createElement("template");
    templateElement.innerHTML = userInfoStr;
    return templateElement;
}

function SetTemplateViewToModelBinder(view, modelItem, viewType) {
    let headImage = view.querySelector(".chat-head-image img");
    let nickName = view.querySelector(".nickname");
    let time = view.querySelector(".time");
    let lastMessage = view.querySelector(".last-message");
    headImage.setAttribute("src", modelItem.HeadImage);
    nickName.innerText = modelItem.NickName;
    if (modelItem.Time !== undefined)
        time.innerText = modelItem.Time;
    if (modelItem.LastMessage !== undefined)
        lastMessage.innerText = modelItem.LastMessage;
}

export default{
    Init, SetItemTemplate, SetTemplateViewToModelBinder
}