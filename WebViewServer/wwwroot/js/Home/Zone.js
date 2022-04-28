import {ShowAlert, ModelView, ShowLoad} from '../My.js';
import Post from "../Shared/Components/Post/Post.js";

async function Init(services)
{
	const mountElement = document.querySelector(".post-mount");
	await ShowLoad(document.querySelector(".show-fragment"), "你所关注的人都在这里...", async(fragment) => {
		await InitPostView(services, mountElement);
	});
	
}

async function InitPostView(services, mountElement) {
	
	const formedPosts = JSON.parse(await services.Api.Post("/Home/Home/ZoneModel"));
	const postModelList = new ModelView.ModelList(formedPosts);
	const postModelView = new ModelView.ModelView(postModelList, mountElement);

	await Post.Init(services);

	postModelView.SetItemViewType(model => {

		return Post.SetItemViewType(model);
	}).SetItemTemplate(viewType => {

		return Post.SetItemTemplate(viewType);
	}).SetTemplateViewToModelBinder((view, model, viewType) => {

		Post.SetTemplateViewToModelBinder(view, model, viewType);

	}).Show();
}

export {
	Init
}

export default{
	Init
}


