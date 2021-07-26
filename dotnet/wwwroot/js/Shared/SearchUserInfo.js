import {Site} from '../My.js'
import {PostChangeRelation} from './_Layout.js'

let attentions;
let privateChats;
function SearchUserInfo()
{
    attentions = document.querySelectorAll(".attention");
    privateChats = document.querySelectorAll(".private-chat");

    if(attentions.length > 0)
        attentions.forEach(attention => {
            attention.onclick = event => AppendFocus(event);
        });

    if(privateChats.length > 0)
        privateChats.forEach(privateChat => {
            //privateChat.onclick = event => AppendFocus(event);
        });
    
    
}

function AppendFocus(event)
{
    let btn = event.target;
    event.stopPropagation();
    let keyID = btn.getAttribute("key");
    PostChangeRelation("/Shared/AppendRelation",
    keyID,"User","Type","Focus",
    resData => {
        console.log(resData);
        btn.onclick = event => RemoveFocus(event);//这种必须写成匿名函数形式,直接写一个函数名会发生不可预知的执行情况

        btn.classList.remove("btn-outline-info");
        btn.classList.add("btn-info");
        btn.value = "已关注";
    },()=>{});
    
}

function RemoveFocus(event,ChangeStyleAction)
{
    let btn = event.target;
    event.stopPropagation();
    let keyID = event.target.getAttribute("key");
    PostChangeRelation("/Shared/RemoveRelation",
    keyID,"User","Type","Focus",
    resData =>{
        console.log(resData);
        event.target.onclick = event => AppendFocus(event);
        btn.classList.remove("btn-info");
        btn.classList.add("btn-outline-info");
        btn.value = "关注";
    },()=>{});
    
}

export{
    SearchUserInfo,AppendFocus,RemoveFocus 
}

export default
{
    SearchUserInfo,AppendFocus,RemoveFocus    
}