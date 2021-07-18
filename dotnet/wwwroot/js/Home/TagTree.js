

function FlushTagTree()
{
    let tagTreeNodes = document.querySelectorAll("#tagRoot .tagNode");
    tagTreeNodes.forEach(tagTreeNode => {
        tagTreeNode.ondblclick = event => {
            event.preventDefault();
            event.stopPropagation();
            FindChildTags(event.target.parentElement.parentElement,tagTreeNode);
        }
    })
}

function AppendChildTree(parentTag, childTags) 
{
    for (let i in childTags) {
        resultItem = document.createElement("div");
        resultItem.innerHTML = childTags[i];
        resultItem.setAttribute("class", "tagNode");
        parentTag.append(resultItem);
    }
}

function FindChildTags(parentTag,tagTreeNode)
{
    let tagID = tagTreeNode.querySelector(".ID").innerText;
    $.ajax({
        url : "/Home/FindChildTags",
        type : "POST",
        data : {
            tagID : tagID
        }
    }).done(resData => {
        AppendChildTree(parentTag,JSON.parse(resData));
        FlushTagTree();
        FlushDrugEvent();
    });
}

FlushTagTree();

