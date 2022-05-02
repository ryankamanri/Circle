import Tag from "./Tag.js";
import {ModelView, ShowAlert} from "../../My.js";
import {PostChangeRelation} from "../MyInterestedTags.js";
import PrivateChat from "../../Home/PrivateChat.js";
let template = document.createElement('template');

let _services;

async function Init(services) {
    await Tag.Init(services);
    template.innerHTML = await services.Api.Get("/Shared/Components/SearchUserInfo");
    _services = services;
}

function SetItemViewType(model) {
    return 1;
}


function SetItemTemplate(viewType) {
    return template;
}

async function SetTemplateViewToModelBinder(view, model, viewType, CallBack) {
    const headImage = view.querySelector(".headimage img");
    headImage.setAttribute("src", model.HeadImage);

    const nickName = view.querySelector(".nickname");
    nickName.innerHTML = model.NickName;

    const year_speciality = view.querySelector(".year-speciality");
    year_speciality.innerHTML = `${new Date(model.SchoolYear).getFullYear()} 级 ${model.Speciality}`;

    const tagMount = view.querySelector(".tags");

    // tags.innerHTML = model.Tags;
    const tagList = model.Tags;
    const tagModelList = new ModelView.ModelList(tagList);
    const tagModelView = new ModelView.ModelView(tagModelList, tagMount);

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

    const similarity = view.querySelector(".similarity");
    similarity.innerHTML = `与我的相似度 : ${(model.Similarity * 100) - (model.Similarity * 100 % 1)} %`;
    const interesty = view.querySelector(".interesty");
    interesty.innerHTML = `我的感兴趣度 : ${(model.Interesty * 100) - (model.Interesty * 100 % 1)} %`;

    const attention = view.querySelector(".attention");
    attention.setAttribute("ID", model.ID);
    attention.setAttribute("isFocus", model.IsFocus);
    

    const privateChat = view.querySelector(".private-chat");
    privateChat.setAttribute("ID", model.ID);

    if(CallBack !== undefined)
        CallBack(view, model, viewType);
    
    // set events
    JudgeFocus(view.querySelector(".attention"));
    view.querySelector(".private-chat").onclick = async() => {
        await PrivateChat.ChatWithAUser(_services, model.ID);
        window.location.hash = "#PrivateChat";
    }
    Tag.FlushDropEvent(view, model);
    Tag.FlushDrugEvent(view, model);
}

function JudgeFocus(btn)
{
    if(btn.getAttribute("isFocus") === "false"){
        btn.onclick = async event => await AppendFocus(event);
    }else{
        btn.onclick = async event => await RemoveFocus(event);
        btn.classList.remove("btn-outline-info");
        btn.classList.add("btn-info");
        btn.value = "已关注";
    }
}

async function AppendFocus(event)
{
    let btn = event.target;
    event.stopPropagation();
    let keyID = btn.getAttribute("ID");
    let resData = await PostChangeRelation("/Shared/AppendRelation", keyID,"User","Type","Focus");
    console.log(resData);
    btn.onclick = async event => await RemoveFocus(event);//这种必须写成匿名函数形式,直接写一个函数名会发生不可预知的执行情况

    btn.classList.remove("btn-outline-info");
    btn.classList.add("btn-info");
    btn.value = "已关注";
    ShowAlert("alert alert-info", "关注成功", "");
}

async function RemoveFocus(event,ChangeStyleAction)
{
    let btn = event.target;
    event.stopPropagation();
    let keyID = event.target.getAttribute("ID");
    let resData = await PostChangeRelation("/Shared/RemoveRelation", keyID,"User","Type","Focus");
    console.log(resData);
    event.target.onclick = async event => await AppendFocus(event);
    btn.classList.remove("btn-info");
    btn.classList.add("btn-outline-info");
    btn.value = "关注";
    ShowAlert("alert alert-info", "取关成功", "");
}

export default {
    Init, SetItemViewType, SetItemTemplate, SetTemplateViewToModelBinder
}