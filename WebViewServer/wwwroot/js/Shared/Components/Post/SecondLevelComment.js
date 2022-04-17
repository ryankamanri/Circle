import { Api } from "../../../My.js";

const componentBaseUrl = "/Shared/Components/Post";
const slCommentTemplateStr = await new Api().Get(`${componentBaseUrl}/SecondLevelComment`);

function Init(services) {
    
}

function SetItemTemplate(viewType) {
    const template = document.createElement("template");
    template.innerHTML = slCommentTemplateStr;
    return template;
}

function SetTemplateViewToModelBinder(view, model, viewType) {
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

    view.style.display = "inherit";
}

export default{
    Init, SetItemTemplate, SetTemplateViewToModelBinder
}