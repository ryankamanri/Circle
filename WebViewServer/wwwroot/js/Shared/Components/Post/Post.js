import { PostChangeRelation } from "../../MyInterestedTags.js";
import { StrIncrement, ShowAlert, ModelView } from "../../../My.js";
import Comment from '../../Comment.js';
import Tag from "../Tag.js";

let postID_commentLabel = [];

let api;
let _services;
const postModelTemplate = document.createElement("template");
const timeTemplate = document.createElement("template");


async function Init(services) {

	Tag.Init(services);
    _services = services;
    api = services.Api;
    postModelTemplate.innerHTML = await services.Api.Get("/Shared/Components/Post/Post");
	timeTemplate.innerHTML = '<div class="date mt-4"></div>';
}

function SetItemViewType(model) {
    if(model.ID === undefined) return 2;
	else return 1;
}

function SetItemTemplate(viewType) {
    if(viewType === 2) {
        return timeTemplate;
    }

    return postModelTemplate;
}

async function SetTemplateViewToModelBinder(view, model, viewType) {
    if(viewType === 2) {
		view.innerText = model.Time;
        return;
    }

    const headImage = view.querySelector(".feed-head-pic img");
    headImage.setAttribute("src", model.AuthorHeadImage);
    const nickName = view.querySelector(".feed-hd-info strong");
    nickName.innerText = model.AuthorNickName;
    const focusLabel = view.querySelector(".feed-hd-info a");
    focusLabel.setAttribute("ID", model.AuthorID);
    focusLabel.setAttribute("isfocus", model.AuthorIsFocus);

    const title = view.querySelector(".feed-title span");
    title.innerText = model.Title;
    const focus = view.querySelector(".feed-cont span.focus");
    focus.innerText = `#${model.Focus}#`;
    const summary = view.querySelector(".feed-cont span.summary");
    summary.innerHTML = model.Summary;
    const postContent = view.querySelector(".post-content input");
    postContent.setAttribute("postid", model.ID);

    const tagMount = view.querySelector(".feed-tags");
    // tags.innerText = model.Tags;
	const tagList = JSON.parse(model.Tags);
	const tagModelList = new ModelView.ModelList(tagList);
	const tagModelView = new ModelView.ModelView(tagModelList, tagMount);

	try {
		await tagModelView.SetItemViewType(model => {
			return "tagNode";
		}).SetItemTemplate(viewType => {
			return Tag.SetItemTemplate(viewType);
		}).SetTemplateViewToModelBinder((view, model, viewType) => {
			Tag.SetTemplateViewToModelBinder(view, model, viewType);
		}).ShowAsync();
	} catch (error) {
		console.error("Load Tag ModelView Error At Post: ");
		console.error(model);
		throw error;
	}
	
	Tag.FlushDropEvent(view);
	Tag.FlushDrugEvent(view);

    const likeLabel = view.querySelector(".more .like");
    likeLabel.setAttribute("isLike", model.IsLike);
    likeLabel.setAttribute("postid", model.ID);
    likeLabel.querySelector(".count").innerText = model.LikeCount;

    const collectLabel = view.querySelector('.more .collect');
    collectLabel.setAttribute("isCollect", model.IsCollect);
    collectLabel.setAttribute("postid", model.ID);
    collectLabel.querySelector(".count").innerText = model.CollectCount;

    const commentLabel = view.querySelector(".more .comment");
    commentLabel.setAttribute("postid", model.ID);
    commentLabel.setAttribute("author-name", model.AuthorNickName);
    commentLabel.setAttribute("post-title", model.Title);
    commentLabel.querySelector(".count").innerText = model.CommentCount;

    InitEvents(view);
}

function InitEvents(view) {

    const contentItem = view.querySelector(".post-content>input");
    const focusLabel = view.querySelector("p>a.tag-label");
    const likeLabel = view.querySelector(".feed-item-bd .more>.like");
    const collectLabel = view.querySelector(".feed-item-bd .more>.collect");
    const commentLabel = view.querySelector(".feed-item-bd .more>.comment");


    contentItem.onclick = async event => {
        event.preventDefault();
        event.stopPropagation();
        await ShowContent(event.target, event.currentTarget.getAttribute("postid"));
    }
    // 给a标签同样添加点击事件
    let a = contentItem.parentElement.parentElement.children[0];
    a.onclick = async event => {
        event.preventDefault();
        event.stopPropagation();
        let contentItemElement = event.currentTarget.parentElement.children[1].children[0];
        await ShowContent(contentItemElement, contentItemElement.getAttribute("postid"));
    }


    postID_commentLabel[commentLabel.getAttribute("postid")] = commentLabel;

    commentLabel.onclick = async event => {
        let commentLabel = event.currentTarget;
        event.stopPropagation();
        await Comment.Init(_services, commentLabel.offsetParent.querySelector(".comment-mount"),
            commentLabel.getAttribute("postid"));
        await Comment.BuildAndShowModelView(commentLabel);
        Comment.ShowInputWindow(commentLabel);
    }


    JudgeFocus(focusLabel);

    JudgeLike(likeLabel);

    JudgeCollect(collectLabel);


}
function GetPostID_commentLabel() {
	return postID_commentLabel;
}


function JudgeFocus(focusLabel) {
	if (focusLabel.getAttribute("isFocus") == "False") {
		focusLabel.onclick = async event => await AppendFocus(event);
		return;
	}
	focusLabel.onclick = async event => await RemoveFocus(event);
	focusLabel.innerText = "已关注";
	focusLabel.style.color = "#25bb9b";
	focusLabel.style.border = "1px solid #25bb9b";

}

function JudgeLike(likeLabel) {
	if (likeLabel.getAttribute("isLike") == "False") {
		likeLabel.onclick = async event => await AppendLike(event);
		return;
	}
	likeLabel.onclick = async event => await RemoveLike(event);
	likeLabel.classList.add("liked");
}

function JudgeCollect(collectLabel) {
	if (collectLabel.getAttribute("isCollect") == "False") {
		collectLabel.onclick = async event => await AppendCollect(event);
		return;
	}
	collectLabel.onclick = async event => await RemoveCollect(event);
	collectLabel.classList.add("collected");
}

async function AppendFocus(event)
{
	let btn = event.target;
	event.stopPropagation();
	let keyID = btn.getAttribute("ID");
	let resData = await PostChangeRelation("/Shared/AppendRelation", keyID,"User","Type","Focus");
	console.log(resData);
	btn.onclick = async event => await RemoveFocus(event);//这种必须写成匿名函数形式,直接写一个函数名会发生不可预知的执行情况
	btn.innerText = "已关注";
	btn.style.color = "#25bb9b";
	btn.style.border = "1px solid #25bb9b";
	ShowAlert("alert alert-info", "感谢关注", "");
	
}

async function RemoveFocus(event)
{
	let btn = event.target;
	event.stopPropagation();
	let keyID = event.target.getAttribute("ID");
	let resData = await PostChangeRelation("/Shared/RemoveRelation", keyID,"User","Type","Focus");
	console.log(resData);
	event.target.onclick = async event => await AppendFocus(event);
	btn.innerText = "+ 关注";
	btn.style.color = "";
	btn.style.border = "";
	ShowAlert("alert alert-info", "取关成功", "");

}

async function AppendLike(event) {
	event.stopPropagation();
	let likeLabel = event.currentTarget;
	likeLabel.onclick = async event => await RemoveLike(event);
	let postID = likeLabel.getAttribute("postid");
	let resData = await PostChangeRelation("/Shared/AppendRelation", postID, "Post", "Type", "Like");
	console.log(resData);
	likeLabel.classList.add("liked");
	let count = likeLabel.querySelector(".count");
	count.innerText = StrIncrement(count.innerText, 1);
	ShowAlert("alert alert-info", "已点赞", "");
}

async function RemoveLike(event) {
	event.stopPropagation();
	let likeLabel = event.currentTarget;
	likeLabel.onclick = async event => await AppendLike(event);
	let postID = likeLabel.getAttribute("postid");
	let resData = await PostChangeRelation("/Shared/RemoveRelation", postID, "Post", "Type", "Like");
	console.log(resData);
	likeLabel.classList.remove("liked");
	let count = likeLabel.querySelector(".count");
	count.innerText = StrIncrement(count.innerText, -1);
	ShowAlert("alert alert-info", "已取消点赞", "");
}

async function AppendCollect(event) {
	event.stopPropagation();
	let collectLabel = event.currentTarget;
	collectLabel.onclick = async event => await RemoveCollect(event);
	let postID = collectLabel.getAttribute("postid");
	let resData = await PostChangeRelation("/Shared/AppendRelation", postID, "Post", "Type", "Collect");
	console.log(resData);
	collectLabel.classList.add("collected");
	let count = collectLabel.querySelector(".count");
	count.innerText = StrIncrement(count.innerText, 1);
	ShowAlert("alert alert-info", "已收藏", "");
}

async function RemoveCollect(event) {
	event.stopPropagation();
	let collectLabel = event.currentTarget;
	collectLabel.onclick = async event => await AppendCollect(event);
	let postID = collectLabel.getAttribute("postid");
	let resData = await PostChangeRelation("/Shared/RemoveRelation", postID, "Post", "Type", "Collect");
	console.log(resData);
	collectLabel.classList.remove("collected");
	let count = collectLabel.querySelector(".count");
	count.innerText = StrIncrement(count.innerText, -1);
	ShowAlert("alert alert-info", "已取消收藏", "");
}


async function ShowContent(node, postID) {
	let resData = await api.Post("/Shared/ShowPostInfo", {
		postID: postID
	});
	let content = JSON.parse(resData).Content;
	let contentNode = document.createElement("div");
	contentNode.innerHTML = content;
	contentNode.className = "ck ck-content content-item col-md-12";

	node.parentElement.insertBefore(contentNode, node);

	node.onclick = event => {
		event.preventDefault();
		event.stopPropagation();
		HideContent(event.target, contentNode);
	}
	// a
	let a = node.parentElement.parentElement.children[0];
	a.onclick = event => {
		event.preventDefault();
		event.stopPropagation();
		let contentItemElement = event.currentTarget.parentElement.children[1].children[1];
		HideContent(contentItemElement, contentNode);
	}
	node.value = "收起全文";
	
}



function HideContent(node, contentNode) {
	
	node.parentElement.removeChild(contentNode);

	node.onclick = async event => {
		event.preventDefault();
		event.stopPropagation();
		await ShowContent(event.target, event.target.getAttribute("postid"));
	}
	// a
	let a = node.parentElement.parentElement.children[0];
	a.onclick = event => {
		event.preventDefault();
		event.stopPropagation();
		let contentItemElement = event.currentTarget.parentElement.children[1].children[0];
		ShowContent(contentItemElement, contentItemElement.getAttribute("postid"));
	}
	node.value = "查看全文";
	console.log("hide content");
}



export default {
	Init, SetItemViewType, SetItemTemplate, SetTemplateViewToModelBinder
}

export {
	GetPostID_commentLabel
}