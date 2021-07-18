let nodes = document.querySelectorAll(".post-content>.content-item");
nodes.forEach(node => {

    node.onclick = event => {
        event.preventDefault();
        event.stopPropagation();
        ShowContent(event.target,event.target.id);
    }
});

function ShowContent(node,postID)
{
    $.ajax({
        url : "/Shared/ShowPostInfo",
        type : "POST",
        data : {
            postID : postID
        }
    }).done(resData => {
        let content = JSON.parse(resData).Content;
        let contentNode = document.createElement("div");
        contentNode.innerHTML = content;
        contentNode.className = "content-item col-md-12";

        node.parentElement.insertBefore(contentNode,node);

        node.onclick = event => {
            event.preventDefault();
            event.stopPropagation();
            HideContent(event.target,contentNode);
        }
        node.value = "收起全文";
    })
}




function HideContent(node)
{
    let contentNode = node.parentElement.querySelector(".content-item");
    node.parentElement.removeChild(contentNode);

    node.onclick = event => {
        event.preventDefault();
        event.stopPropagation();
        ShowContent(event.target,event.target.id);
    }
    node.value = "查看全文";
    console.log("hide content");
}


