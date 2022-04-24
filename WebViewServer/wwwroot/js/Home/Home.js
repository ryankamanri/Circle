import { ShowAlert, ModelView, Sleep } from '../My.js';
import Post from "../Shared/Components/Post/Post.js";

async function Init(services)
{
	await InitPostView(services);
	
	let circleName = document.querySelector("a.navbar-brand").innerText;
	if(window.location.pathname == "/Home" && window.location.hash == "#Home/Posts")
		ShowAlert("alert alert-info",`欢迎来到 ${circleName} ,`,"点击左上角可切换圈子");
}

async function InitPostView(services) {
	const mountElement = document.querySelector(".content-mount");
	const formedPosts = JSON.parse(await services.Api.Post("/Home/Home/PostsModel"));
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

	AddScrollExtraPostsEvent(services, postModelList);
}

function AddScrollExtraPostsEvent(services, postModelList) {
	let isLoaded = true;
	window.onscroll = async() => {
		if(window.location.hash !== "#Home/Posts") {
			// Cancel the event
			window.onscroll = null;
		}

		if (!isLoaded) return;

		let clients = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
		let scrollTop = document.documentElement.scrollTop;
		// 这里存在兼容问题，会把body当成div来处理，如果用document.body.scrollHeight就得不到正确的高度，用body时需要把doctype后面的html去掉
		// 这里没用body，而是用到documentElement
		let wholeHeight = document.documentElement.scrollHeight;
		if (wholeHeight - scrollTop - clients >= 100) return;

		
		// 在实际应用中可以通过请求后台获取下一页的数据，然后显示到当前位置，就能达到按页加载的效果。
		isLoaded = false;
		window.scrollTo(0, wholeHeight - clients - 200);
		ShowAlert("alert alert-info", "⏳", "加载中...");
		await Sleep(1000);
		ShowAlert("alert alert-success", "😃", "加载成功!");
		await AddScrollExtraPosts(services, postModelList);
		isLoaded = true;
		
	};
}

async function AddScrollExtraPosts(services, postModelList) {
	const formedPosts = JSON.parse(await services.Api.Post("/Home/Home/PostsExtraModel"));
	console.log(formedPosts);
	formedPosts.forEach(post => {
		postModelList.Append(post);
	});
	
}

export {
	Init
}

export default{
	Init
}


