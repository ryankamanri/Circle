import { Api, ModelView, StrIncrement } from '../My.js';
import { ShowAlert, ShowInput } from '../Show.js';
import { PostChangeRelation } from './MyInterestedTags.js';
import { GetPostID_commentLabel } from './Post.js';


let api = new Api();
let postID_commentModelList = [], postID_commentModelView = [];

let formedComments;
let commentModelList, commentModelView;
let mountElement, postID;
let commentTemplate;
let slCommentTemplate;

function Comment(mountElement_param, postID_param) {
	mountElement = mountElement_param;
	postID = postID_param;
	GenerateCommentTemplate(mountElement.children[0]);
}

function GetPostID_commentModelList() {
	return postID_commentModelList;
}

function GenerateCommentTemplate(templateElement) {
	if (slCommentTemplate == undefined || commentTemplate == undefined) {
		slCommentTemplate = document.createElement("template");
		slCommentTemplate.content.append(templateElement.querySelector(".sl-comment"));
		commentTemplate = document.createElement("template");
		commentTemplate.content.append(templateElement);
	}
	
}

async function Show(commentLabel) {
	commentLabel.onclick = event => Clean(event.currentTarget, modelView);
	postID = commentLabel.getAttribute("postid");
	let modelView = postID_commentModelView[postID];
	modelView.Show();
	ShowInputWindow(commentLabel);
}

function Clean(commentLabel) {
	commentLabel.onclick = async event => await Show(event.currentTarget, modelView);
	postID = commentLabel.getAttribute("postid");
	let modelView = postID_commentModelView[postID];
	modelView.Clean();
}


async function BuildAndShowModelView(commentLabel) {

	
	let resData = await api.Post("/Shared/SelectFormedCommentsAndUser", {
		postID: postID
	});
	formedComments = JSON.parse(resData);

	commentModelList = new ModelView.ModelList(formedComments);
	postID_commentModelList[`${postID}`] = commentModelList;
	commentModelView = new ModelView.ModelView(commentModelList, mountElement);
	postID_commentModelView[`${postID}`] = commentModelView;

	commentLabel.onclick = event => Clean(event.currentTarget, commentModelView);

	commentModelView.SetItemTemplate(_ => {
		return commentTemplate;
	}).SetTemplateViewToModelBinder((view, model, _) => {
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

		JudgeLike(event, model, view);
		let commentLabel = view.querySelector(".comment");
		commentLabel.setAttribute("owner-name", model.Key.Value.OwnerNickName);
		commentLabel.setAttribute("comment-id", model.Key.Key.ID);
		commentLabel.onclick = event => {
			let commentLabel = event.currentTarget;
			ShowSecondLevelComment(event, model, view);
			ShowInputWindow(commentLabel);
		}

		view.style.display = "inherit";
	}).Show();
}

function ShowInputWindow(commentLabel) {
	// 帖子评论
	let authorName = commentLabel.getAttribute("author-name");
	let postTitle = commentLabel.getAttribute("post-title");
	// 评论回复
	let ownerName = commentLabel.getAttribute("owner-name");
	let commentID = commentLabel.getAttribute("comment-id");

	let postID = commentLabel.getAttribute("postid");
	if (authorName != null && postTitle != null) {
		ShowInput(`评论 ${authorName} 的帖子: ${postTitle} ...`, "评论", { postID: postID }, async (text, delegateObj) => {
		console.log(text);
		let resData = await api.Post("/Api/SendAComment", {
			postID: delegateObj.postID,
			comment: text
		});

		console.log(resData);

		if (resData != "true") {
			ShowAlert("alert alert-warning", "😥", "未发送评论");
			return;
		}
		ShowAlert("alert alert-success", "😀", "已发送评论");
		// 添加一条评论...
		let commentLabel = GetPostID_commentLabel()[delegateObj.postID];
		Clean(commentLabel);
		await BuildAndShowModelView(commentLabel);
	});
	} else {
		ShowInput(`回复 ${ownerName} 的评论...`, "回复", { postID: postID, commentID: commentID }, async (text, delegateObj) => {
			console.log(text);
			let resData = await api.Post("/Api/SendACommentReply", {
				postID: delegateObj.postID,
				replyCommentID: delegateObj.commentID,
				comment: text
			});

			console.log(resData);

			if (resData != "true") {
				ShowAlert("alert alert-warning", "😥", "未回复评论");
				return;
			}
			ShowAlert("alert alert-success", "😀", "已回复评论");
			// 添加一条评论...

			let commentLabel = GetPostID_commentLabel()[delegateObj.postID];
			Clean(commentLabel);
			await BuildAndShowModelView(commentLabel);
		});
	}
	
}

function JudgeLike(event, model, view) {
	let likeLabel = view.querySelector(".like");
	if (model.Key.Value.IsLike == "False") {
		likeLabel.onclick = async event => await AppendLike(event, model);
		return;
	}
	likeLabel.onclick = async event => await RemoveLike(event, model);
	likeLabel.classList.add("liked");
}

async function AppendLike(event, model) {
	
	let likeLabel = event.currentTarget;
	let commentID = model.Key.Key.ID;
	likeLabel.onclick = async event => await RemoveLike(event, model);
	let resData = await PostChangeRelation("/Shared/AppendRelation", commentID, "Comment", "Type", "Like");
	console.log(resData);
	likeLabel.classList.add("liked");
	let count = likeLabel.querySelector(".count");
	count.innerText = StrIncrement(count.innerText, 1);
	ShowAlert("alert alert-info", "已点赞", "");
}

async function RemoveLike(event, model) {
	
	let likeLabel = event.currentTarget;
	let commentID = model.Key.Key.ID;
	likeLabel.onclick = async event => await AppendLike(event, model);
	let resData = await PostChangeRelation("/Shared/RemoveRelation", commentID, "Comment", "Type", "Like");
	console.log(resData);
	likeLabel.classList.remove("liked");
	let count = likeLabel.querySelector(".count");
	count.innerText = StrIncrement(count.innerText, -1);
	ShowAlert("alert alert-info", "已取消点赞", "");
}

async function ShowSecondLevelComment(event, flCommentModel, flCommentView) {

	let commentLabel = event.currentTarget;

	let slCommentModelList = new ModelView.ModelList(flCommentModel.Value);
	let slCommentMountElement = flCommentView.querySelector(".second-comment-mount");
	let slCommentModelView = new ModelView.ModelView(slCommentModelList, slCommentMountElement);

	commentLabel.onclick = event => Clean(event.currentTarget, slCommentModelView);

	slCommentModelView.SetItemTemplate(_ => {
		return slCommentTemplate;
	}).SetTemplateViewToModelBinder((view, model, viewType) => {
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
		commentLabel.setAttribute("owner-name", model.Value.OwnerNickName);
		commentLabel.setAttribute("comment-id", model.Key.ID);
		commentLabel.addEventListener("click", event => {
			ShowInputWindow(event.currentTarget);
		});
		
		view.style.display = "inherit";
	}).Show();

	
}

export {
	Comment, BuildAndShowModelView, GetPostID_commentModelList, Show, Clean, ShowInputWindow
}

export default {
	Comment, BuildAndShowModelView, GetPostID_commentModelList, Show, Clean, ShowInputWindow
}