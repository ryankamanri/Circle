import {ShowAlert, ModelView, Sleep, ShowLoad} from '../My.js';
import Post from "../Shared/Components/Post/Post.js";
import FirstLevelComment from "../Shared/Components/Post/FirstLevelComment.js";
import SecondLevelComment from "../Shared/Components/Post/SecondLevelComment.js";
import {ShowInputWindow} from "../Shared/Comment.js";

let modelList, modelView;

const ModelType = {
	Post: 1,
	PostTime: 2,
	FirstLevelComment: 3,
	SecondLevelComment: 4
}

let modelType = ModelType.Post;
let type = ["Owned"];

async function Init(services){
	const mountElement = document.querySelector(".userpost-mount");
	await ShowLoad(document.querySelector(".show-mount"), "加载中...", async(fragment) => {
		modelType = ModelType.Post;
		await InitView(services, mountElement);
	});
	const myPostBtn = document.querySelector(".card-body .my-post");
	myPostBtn.onclick = async() => {
		type = ["Owned"];
		modelType = ModelType.Post;
		await ReloadView(services);
	}
	
	const myZoneBtn = document.querySelector(".card-body .my-zone");
	myZoneBtn.onclick = async() => {
		modelType = ModelType.Post;
		const formedModels = JSON.parse(await services.Api.Post("/Home/Home/ZoneModel"));
		await ReloadView(services, formedModels);
	}

	const myCollectBtn = document.querySelector(".card-body .my-collect");
	myCollectBtn.onclick = async() => {
		type = ["Collect"];
		modelType = ModelType.Post;
		await ReloadView(services);
	}

	const myLikeBtn = document.querySelector(".card-body .my-like");
	myLikeBtn.onclick = async() => {
		type = ["Like"];
		modelType = ModelType.Post;
		await ReloadView(services);
	}
	
	const myCommentBtn = document.querySelector(".card-body .my-comment");
	myCommentBtn.onclick = async() => {
		const formedData = JSON.parse(await services.Api.Post("/Home/SelectCommentModel", {
			Type: ["Owned"]
		}));
		modelType = ModelType.FirstLevelComment;
		await ReloadView(services, formedData);
	}
}

async function InitView(services, mountElement) {
	const formedModels = JSON.parse(await services.Api.Post("/Home/SelectPostModel", {
		Type: type
	}));
	
	modelList = new ModelView.ModelList(formedModels);
	modelView = new ModelView.ModelView(modelList, mountElement);

	await Post.Init(services);

	await modelView.SetItemViewType(model => {
		if(modelType === ModelType.Post)
			return Post.SetItemViewType(model);
		else {
			if(model.Key.CommentID === -1)
				return ModelType.FirstLevelComment;
			return ModelType.SecondLevelComment;
		}
	}).SetItemTemplate(viewType => {
		switch (viewType) {
			case ModelType.Post:
			case ModelType.PostTime:
				return Post.SetItemTemplate(viewType);
			case ModelType.FirstLevelComment:
				return FirstLevelComment.SetItemTemplate(FirstLevelComment.ViewType.SingleItem);
			case ModelType.SecondLevelComment:
				return SecondLevelComment.SetItemTemplate(SecondLevelComment.ViewType.SingleItem);
		}
		
	}).SetTemplateViewToModelBinder((view, model, viewType) => {
		let commentLabel = view.querySelector(".comment");
		switch (viewType) {
			case ModelType.Post:
			case ModelType.PostTime:
				Post.SetTemplateViewToModelBinder(view, model, viewType);
				break;
			case ModelType.FirstLevelComment:
				FirstLevelComment.SetTemplateViewToModelBinder(view, model, viewType);
				commentLabel.addEventListener("click", event => {
					ShowInputWindow(commentLabel);
				});
				break;
			case ModelType.SecondLevelComment:
				SecondLevelComment.SetTemplateViewToModelBinder(view, model, viewType);
				commentLabel.addEventListener("click", event => {
					ShowInputWindow(commentLabel);
				});
				break;
		}

	}).ShowAsync();

}

async function ReloadView(services, models=null) {
	await ShowLoad(document.querySelector(".show-mount"), "加载中...", async(fragment) => {
		let formedModels = JSON.parse(await services.Api.Post("/Home/SelectPostModel", {
			Type: type
		}));
		if (models != null) formedModels = models;
		modelList.DeleteAt(0, modelList.GetLength());
		formedModels.forEach(model => modelList.AppendAsync(model));
		modelView.Clean();
		await modelView.ShowAsync();
	});
	
}




export default{
    Init
}