import { ShowAlert, ModelView } from '../My.js';
import Post from "../Shared/Components/Post/Post.js";

async function Init(services)
{
	await InitPostView(services);
}

async function InitPostView(services) {
	const mountElement = document.querySelector(".content-mount");
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


