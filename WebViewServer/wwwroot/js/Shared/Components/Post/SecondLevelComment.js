import { Api } from "../../../My.js";

const componentBaseUrl = "/Shared/Components/Post";
const slCommentTemplateStr = await new Api().Get(`${componentBaseUrl}/SecondLevelComment`);

function Init(services) {
    
}

const ViewType = {
    SingleItem: "SingleItem",
    Default: "Default"
}

function SetItemTemplate(viewType) {
    const template = document.createElement("template");
    switch (viewType){
        case ViewType.SingleItem:
            const div = document.createElement("div");
            div.className="sl-comment-item";
            div.innerHTML = slCommentTemplateStr;
            template.content.appendChild(div);
            return template;
        default:
            template.innerHTML = slCommentTemplateStr;
            return template;
    }
}

function SetTemplateViewToModelBinder(view, model, viewType) {
    view.setAttribute("comment-id", model.Key.ID);
    
    let headImage = view.querySelectorAll(".head-image>img")[0];
    headImage.setAttribute("src", model.Value.OwnerHeadImage);
    let nickName = view.querySelectorAll(".nickname")[0];
    nickName.innerText = model.Value.OwnerNickName;
    let replyHeadImage = view.querySelectorAll(".head-image>img")[1];
    replyHeadImage.setAttribute("src", model.Value.ReplyUserHeadImage);
    let replyNickName = view.querySelectorAll(".nickname")[1];
    replyNickName.innerText = model.Value.ReplyUserNickName;
    let commentDateTime = view.querySelector(".comment-datetime");
    commentDateTime.innerText = model.Key.CommentDateTime;
    let commentContent = view.querySelector(".comment-content");
    commentContent.innerText = model.Key.Content;

    let commentLabel = view.querySelector(".comment");
    commentLabel.setAttribute("postid", model.Key.PostID);
    commentLabel.setAttribute("owner-name", model.Value.OwnerNickName);
    commentLabel.setAttribute("comment-id", model.Key.ID);
    
    InitEvents(view, model);

    view.style.display = "inherit";
}

function InitEvents(view, model) {
    const content = view.querySelector(".comment-content");
    content.onclick = () => {
        window.location.hash = `#PostItem?PostID=${model.Key.PostID}&CommentID=${model.Key.ID}`;
    }
}

export default{
    Init, SetItemTemplate, SetTemplateViewToModelBinder, ViewType
}