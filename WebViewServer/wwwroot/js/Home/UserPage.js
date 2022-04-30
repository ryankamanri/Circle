import {ShowAlert, ModelView, Sleep, ShowLoad} from '../My.js';
import Post from "../Shared/Components/Post/Post.js";
import {SetBaseTitle} from "../Routes.js";

let postModelList, postModelView;

let type = ["Owned"];

async function Init(services){
	const mountElement = document.querySelector(".userpost-mount");
	await ShowLoad(document.querySelector(".show-mount"), "加载中...", async(fragment) => {
		await InitPostView(services, mountElement);
	});
	const myPostBtn = document.querySelector(".card-body .my-post");
	myPostBtn.onclick = async() => {
		type = ["Owned"];
		await ReloadPostView(services);
	}
	
	const myZoneBtn = document.querySelector(".card-body .my-zone");
	myZoneBtn.onclick = async() => {
		const formedPosts = JSON.parse(await services.Api.Post("/Home/Home/ZoneModel"));
		await ReloadPostView(services, formedPosts);
	}

	const myCollectBtn = document.querySelector(".card-body .my-collect");
	myCollectBtn.onclick = async() => {
		type = ["Collect"];
		await ReloadPostView(services);
	}

	const myLikeBtn = document.querySelector(".card-body .my-like");
	myLikeBtn.onclick = async() => {
		type = ["Like"];
		await ReloadPostView(services);
	}
}

async function InitPostView(services, mountElement) {
	const formedPosts = JSON.parse(await services.Api.Post("/Home/SelectPostModel", {
		Type: type
	}));
	
	postModelList = new ModelView.ModelList(formedPosts);
	postModelView = new ModelView.ModelView(postModelList, mountElement);

	await Post.Init(services);

	await postModelView.SetItemViewType(model => {

		return Post.SetItemViewType(model);
	}).SetItemTemplate(viewType => {

		return Post.SetItemTemplate(viewType);
	}).SetTemplateViewToModelBinder((view, model, viewType) => {

		Post.SetTemplateViewToModelBinder(view, model, viewType);

	}).ShowAsync();

}

async function ReloadPostView(services, posts=null) {
	await ShowLoad(document.querySelector(".show-mount"), "加载中...", async(fragment) => {
		let formedPosts = JSON.parse(await services.Api.Post("/Home/SelectPostModel", {
			Type: type
		}));
		if (posts != null) formedPosts = posts;
		postModelList.DeleteAt(0, postModelList.GetLength());
		formedPosts.forEach(post => postModelList.Append(post));
		postModelView.Clean();
		await postModelView.ShowAsync();
	});
	
}




export default{
    Init
}