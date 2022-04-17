import { Api } from "../../../My.js";

const componentBaseUrl = "/Shared/Components/Post";
const commentTemplateStr = await new Api().Get(`${componentBaseUrl}/FirstLevelComment`);

function Init(services) {
    
}

function SetItemTemplate(viewType) {
    const template = document.createElement("template");
    template.innerHTML = commentTemplateStr;
    return template;
}

function SetTemplateViewToModelBinder(view, model, viewType) {
    let headImage = view.querySelector(".head-image>img");
    headImage.setAttribute("src", model.Key.Value.OwnerHeadImage);
    let nickName = view.querySelector(".nickname");
    nickName.innerText = model.Key.Value.OwnerNickName;
    let commentDateTime = view.querySelector(".comment-datetime");
    commentDateTime.innerText = model.Key.Key.CommentDateTime;
    let commentContent = view.querySelector(".comment-content");
    commentContent.innerText = model.Key.Key.Content;
    let likeCount = view.querySelector(".like>.count");
    likeCount.innerText = model.Key.Value.LikeCount;
    let replyCount = view.querySelector(".comment>.count");
    replyCount.innerText = model.Key.Value.ReplyCount;

    let commentLabel = view.querySelector(".comment");
    commentLabel.setAttribute("owner-name", model.Key.Value.OwnerNickName);
    commentLabel.setAttribute("comment-id", model.Key.Key.ID);

    view.style.display = "inherit";
}

export default{
    Init, SetItemTemplate, SetTemplateViewToModelBinder
}