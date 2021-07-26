import {PostChangeRelation} from './_Layout.js'
let contentItems, focusLabels;
function Post() {

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
            focusLabel.onclick = event => AppendFocus(event);
        })

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
        // btn.classList.remove("btn-outline-info");
        // btn.classList.add("btn-info");
        // btn.value = "已关注";
        btn.innerText = "已关注";
        btn.style.color = "#25bb9b";
        btn.style.border = "1px solid #25bb9b";
    },()=>{});
    
}

function RemoveFocus(event)
{
    let btn = event.target;
    event.stopPropagation();
    let keyID = event.target.getAttribute("key");
    PostChangeRelation("/Shared/RemoveRelation",
    keyID,"User","Type","Focus",
    resData =>{
        console.log(resData);
        event.target.onclick = event => AppendFocus(event);
        // btn.classList.remove("btn-info");
        // btn.classList.add("btn-outline-info");
        // btn.value = "关注";
        btn.innerText = "+ 关注";
        btn.style.color = "";
        btn.style.border = "";
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