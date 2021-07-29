import {PostChangeRelation} from './_Layout.js'
import {parseFunc} from '../My.js';

let contentItems, focusLabels ,thisVue;

function Post(vue) {

    thisVue = vue;

    contentItems = document.querySelectorAll(".post-content>.content-item");
    focusLabels = document.querySelectorAll("p>a.tag-label");

    if (contentItems.length > 0)
        contentItems.forEach(node => {
            node.onclick = event => {
                event.preventDefault();
                event.stopPropagation();
                ShowContent(event.target, event.target.id);
            }
        });
    if(focusLabels.length > 0)
        focusLabels.forEach(focusLabel => {
            JudgeFocus(focusLabel);
        })

}

function JudgeFocus(focusLabel)
{
    if(focusLabel.getAttribute("isFocus") == "False"){
        focusLabel.onclick = event => AppendFocus(event);
    }else{
        focusLabel.onclick = event => RemoveFocus(event);
        focusLabel.innerText = "已关注";
        focusLabel.style.color = "#25bb9b";
        focusLabel.style.border = "1px solid #25bb9b";
    }
}

function AppendFocus(event)
{
    let btn = event.target;
    event.stopPropagation();
    let keyID = btn.getAttribute("ID");
    PostChangeRelation("/Shared/AppendRelation",
    keyID,"User","Type","Focus",
    resData => {
        console.log(resData);
        btn.onclick = event => RemoveFocus(event);//这种必须写成匿名函数形式,直接写一个函数名会发生不可预知的执行情况
        btn.innerText = "已关注";
        btn.style.color = "#25bb9b";
        btn.style.border = "1px solid #25bb9b";
        parseFunc(thisVue.$data.store.func.ShowMessage)("alert alert-info","关注成功","");
        //ShowMessage("alert alert-info","关注成功","");
    },()=>{});
    
}

function RemoveFocus(event)
{
    let btn = event.target;
    event.stopPropagation();
    let keyID = event.target.getAttribute("ID");
    PostChangeRelation("/Shared/RemoveRelation",
    keyID,"User","Type","Focus",
    resData =>{
        console.log(resData);
        event.target.onclick = event => AppendFocus(event);
        btn.innerText = "+ 关注";
        btn.style.color = "";
        btn.style.border = "";
        parseFunc(thisVue.$data.store.func.ShowMessage)("alert alert-info","取关成功","");
        //ShowMessage("alert alert-info","取关成功","");
    },()=>{});
    
}



function ShowContent(node, postID) {
    $.ajax({
        url: "/Shared/ShowPostInfo",
        type: "POST",
        data: {
            postID: postID
        }
    }).done(resData => {
        let content = JSON.parse(resData).Content;
        let contentNode = document.createElement("div");
        contentNode.innerHTML = content;
        contentNode.className = "content-item col-md-12";

        node.parentElement.insertBefore(contentNode, node);

        node.onclick = event => {
            event.preventDefault();
            event.stopPropagation();
            HideContent(event.target, contentNode);
        }
        node.value = "收起全文";
    })
}




function HideContent(node) {
    let contentNode = node.parentElement.querySelector(".content-item");
    node.parentElement.removeChild(contentNode);

    node.onclick = event => {
        event.preventDefault();
        event.stopPropagation();
        ShowContent(event.target, event.target.id);
    }
    node.value = "查看全文";
    console.log("hide content");
}

export default{
    Post
}