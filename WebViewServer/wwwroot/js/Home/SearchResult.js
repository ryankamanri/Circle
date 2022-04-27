import SearchUserInfo from '../Shared/Components/SearchUserInfo.js'
import Post from '../Shared/Components/Post/Post.js'
import {ModelView} from "../My.js";


let userContainer,searchResult,searchString,splitedSearchStrings;
let HasBeenExecuted = false;
async function Init(services)
{
	// if(HasBeenExecuted) return;
	const searchStringForm = window.location.hash.split("?")[1];
	const formedResults = JSON.parse(await services.Api.Get(`/Home/SearchResultModel?${searchStringForm}`));
	await InitUserInfoView(services, formedResults["UserInfos"]);
	await InitPostView(services, formedResults["Posts"]);
	///由于这一部分作用为替换关键字为高亮,将部分dom元素进行了替换,所以在此之前绑定过的监听元素会失效
	///如需绑定监听元素,请在此段之后
	// MatchKeyWords();
	///

	SearchUserInfo.Init(services);
	Post.Init(services);
	
	HasBeenExecuted = true;
}

async function InitUserInfoView(services, formedUserInfos) {
	const mountElement = document.querySelector(".user-container");
	// const formedPosts = JSON.parse(await services.Api.Post("/Home/Home/PostsModel"));
	const userModelList = new ModelView.ModelList(formedUserInfos);
	const userModelView = new ModelView.ModelView(userModelList, mountElement);

	await SearchUserInfo.Init(services);

	await userModelView.SetItemViewType(model => {

		return SearchUserInfo.SetItemViewType(model);
	}).SetItemTemplate(viewType => {

		return SearchUserInfo.SetItemTemplate(viewType);
	}).SetTemplateViewToModelBinder((view, model, viewType) => {

		SearchUserInfo.SetTemplateViewToModelBinder(view, model, viewType, view => {
			MatchKeyWords(view);
		});

	}).ShowAsync();
}

async function InitPostView(services, formedPosts) {
	const mountElement = document.querySelector(".post-mount");
	// const formedPosts = JSON.parse(await services.Api.Post("/Home/Home/PostsModel"));
	const postModelList = new ModelView.ModelList(formedPosts);
	const postModelView = new ModelView.ModelView(postModelList, mountElement);

	await Post.Init(services);

	await postModelView.SetItemViewType(model => {

		return Post.SetItemViewType(model);
	}).SetItemTemplate(viewType => {

		return Post.SetItemTemplate(viewType);
	}).SetTemplateViewToModelBinder((view, model, viewType) => {

		Post.SetTemplateViewToModelBinder(view, model, viewType, view => {
			MatchKeyWords(view);
		});

	}).ShowAsync();
}

function MatchKeyWords(view){
	searchResult = view.innerHTML;
	searchString = decodeURI(window.location.href.split('searchString=')[1]);
	document.querySelector("input#search").value = searchString;
	splitedSearchStrings = searchString.split(" ");
	
	let divMatchStrings = searchResult.match(/<div.*>.*<\/div>/g);
	if(divMatchStrings != undefined) divMatchStrings.forEach(matchString => 
		splitedSearchStrings.forEach(splitedSearchString => HignlightReplace(matchString,splitedSearchString)));
	let spanMatchStrings = searchResult.match(/<span.*>.*<\/span>/g);
	if(spanMatchStrings != undefined) spanMatchStrings.forEach(matchString => 
		splitedSearchStrings.forEach(splitedSearchString => HignlightReplace(matchString,splitedSearchString)));
	view.innerHTML = searchResult;
}

function HignlightReplace(matchString,searchString)
{
	let matchAttr = matchString.match(/>\s*.*\s*</g)[0];
	let matchAttrReplace = matchAttr.replaceAll(searchString,`<strong style="color: red">${searchString}</strong>`);
	let matchStringReplace = matchString.replace(matchAttr,matchAttrReplace);
	searchResult = searchResult.replace(matchString,matchStringReplace);
}

export {
	Init
}

export default{
	Init
}






