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
    view.setAttribute("comment-id", model.Key.ID);
    
    let headImage = view.querySelector(".head-image>img");
    headImage.setAttribute("src", model.Value.OwnerHeadImage);
    let nickName = view.querySelector(".nickname");
    nickName.innerText = model.Value.OwnerNickName;
    let commentDateTime = view.querySelector(".comment-datetime");
    commentDateTime.innerText = model.Key.CommentDateTime;
    let commentContent = view.querySelector(".comment-content");
    commentContent.innerText = model.Key.Content;
    let likeCount = view.querySelector(".like>.count");
    likeCount.innerText = model.Value.LikeCount;
    let replyCount = view.querySelector(".comment>.count");
    replyCount.innerText = model.Value.ReplyCount;

    const likeLabel = view.querySelector(".more .like");
    likeLabel.setAttribute("postid", model.Key.PostID);

    const commentLabel = view.querySelector(".comment");
    commentLabel.setAttribute("postid", model.Key.PostID);
    commentLabel.setAttribute("owner-name", model.Value.OwnerNickName);
    commentLabel.setAttribute("comment-id", model.Key.ID);

    // add events
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
    Init, SetItemTemplate, SetTemplateViewToModelBinder
}