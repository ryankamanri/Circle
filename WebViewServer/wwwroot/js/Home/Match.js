import {ShowAlert, ModelView, Sleep, AddScript, ShowLoad} from '../My.js';
import SearchUserInfo from "../Shared/Components/SearchUserInfo.js";

await AddScript(document.body, "/lib/matchButton/three.min.js");
await AddScript(document.body, "/lib/matchButton/gsap.min.js");




async function InitUserInfoView(services, mountElement) {
	
    const formedData = await services.Api.Get(`/Home/MatchModel`);
	const formedUserInfos = JSON.parse(formedData).MatchUserInfoList;
	const userModelList = new ModelView.ModelList(formedUserInfos);
	const userModelView = new ModelView.ModelView(userModelList, mountElement);

	await SearchUserInfo.Init(services);

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

async function Init(services){
	
}
export {
	InitUserInfoView
}

export default{
    Init
}