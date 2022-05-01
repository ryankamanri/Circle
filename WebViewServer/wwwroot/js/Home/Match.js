import {ShowAlert, ModelView, Sleep, AddScript, ShowLoad} from '../My.js';
import SearchUserInfo from "../Shared/Components/SearchUserInfo.js";
import Tag from "../Shared/Components/Tag.js";

await AddScript(document.body, "/lib/matchButton/three.min.js");
await AddScript(document.body, "/lib/matchButton/gsap.min.js");

let matchData;

function GetMatchUserInfos() {
	return JSON.parse(matchData).MatchUserInfoList;
}

function GetMatchTags() {
	return JSON.parse(matchData).MatchTagList;
}


async function InitUserInfoView(services, mountElement) {
	
    
	const formedUserInfos = GetMatchUserInfos();
	const userModelList = new ModelView.ModelList(formedUserInfos);
	const userModelView = new ModelView.ModelView(userModelList, mountElement);

	await SearchUserInfo.Init(services);
	
	userModelView.Clean();

	await userModelView.SetItemViewType(model => {

		return SearchUserInfo.SetItemViewType(model);
	}).SetItemTemplate(viewType => {

		return SearchUserInfo.SetItemTemplate(viewType);
	}).SetTemplateViewToModelBinder((view, model, viewType) => {

		SearchUserInfo.SetTemplateViewToModelBinder(view, model, viewType,view =>{
            // MatchKeyWords(view);
        });

	}).ShowAsync();

}

async function InitTagView(services, tagMount) {
	
	const tagList = GetMatchTags();
	const tagModelList = new ModelView.ModelList(tagList);
	const tagModelView = new ModelView.ModelView(tagModelList, tagMount);

	tagModelView.Clean();
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

	Tag.FlushDropEvent(tagMount);
	Tag.FlushDrugEvent(tagMount);
}

async function Init(services){
	matchData = await services.Api.Get(`/Home/MatchModel`);
}


export default{
    Init, InitUserInfoView, InitTagView, GetMatchUserInfos, GetMatchTags
}