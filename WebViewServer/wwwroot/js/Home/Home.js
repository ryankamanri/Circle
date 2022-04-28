import { ShowAlert, ShowLoad, ModelView, Sleep } from '../My.js';
import Post from "../Shared/Components/Post/Post.js"
import {SetBaseTitle, SetSubtitle} from "../Routes.js";

const CircleTypeApi = {
	Default : "/Home/Home/PostsModel",
	DefaultExtra : "/Home/Home/PostsExtraModel",
	Postgraduate : "/Home/Home/PostgraduatePostsModel",
	PostgraduateExtra : "/Home/Home/PostgraduatePostsExtraModel",
	Employment : "/Home/Home/EmploymentPostsModel",
	EmploymentExtra : "/Home/Home/EmploymentPostsExtraModel"
};

let postApi = CircleTypeApi.Default;
let postExtraApi = CircleTypeApi.DefaultExtra;

async function Init(services)
{
	const mountElement = document.querySelector(".post-mount");
	await ShowLoad(document.querySelector(".show-fragment"), "一大波帖子正在赶到...", async(fragment) => {
		SetBaseTitle("圈");
		await InitPostView(services, mountElement);
	});
	
	ShowWelcome();
	
	
	const postgraguateButton = document.querySelector(".menu-box .postgraduate");
	const employmentButton = document.querySelector(".menu-box .employment");
	postgraguateButton.onclick = async() => {
		postApi = CircleTypeApi.Postgraduate;
		postExtraApi = CircleTypeApi.PostgraduateExtra;

		await ShowLoad(document.querySelector(".show-fragment"), "一大波考研经验贴正在出炉...", async(fragment) => {
			SetBaseTitle("考研圈");
			await ReloadPostView(services);
			ShowWelcome();
		});
		
	};
	employmentButton.onclick = async() => {
		postApi = CircleTypeApi.Employment;
		postExtraApi = CircleTypeApi.EmploymentExtra;

		await ShowLoad(document.querySelector(".show-fragment"), "一大波就业经验贴正在路上...", async(fragment) => {
			SetBaseTitle("就业圈");
			await ReloadPostView(services);
			ShowWelcome();
		});
	};
}

function ShowWelcome() {
	const circleName = {
		"/Home/Home/PostsModel": "Circle",
		"/Home/Home/PostgraduatePostsModel": "考研圈",
		"/Home/Home/EmploymentPostsModel": "就业圈"
	};
	if(window.location.pathname === "/Home" && window.location.hash === "#Home/Posts")
		ShowAlert("alert alert-info",`欢迎来到 ${circleName[postApi]} ,`,"点击上方可切换圈子");
}


let postModelList;
let postModelView;

async function ReloadPostView(services) {
	const formedPosts = JSON.parse(await services.Api.Post(postApi));
	postModelList.DeleteAt(0, postModelList.GetLength());
	formedPosts.forEach(post => postModelList.Append(post));
	postModelView.Clean();
	await postModelView.ShowAsync();
}

async function InitPostView(services, mountElement) {
	const formedPosts = JSON.parse(await services.Api.Post(postApi));
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
	const formedPosts = JSON.parse(await services.Api.Post(postExtraApi));
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


