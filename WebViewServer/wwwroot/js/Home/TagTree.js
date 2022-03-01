
import Tag from '../Shared/Tag.js'

let tagRoot;

function TagTree()
{
	Tag.Tag();

	FlushTagTree();

	tagRoot = document.querySelector("#tagRoot");
	tagRoot.onmousewheel = event => {
		event.preventDefault();
		event.stopPropagation();
		tagRoot.scrollLeft += event.deltaY;
	}
}

function FlushTagTree() {
	let tagTreeNodes = document.querySelectorAll("#tagRoot .ceiledTagNode");
	tagTreeNodes.forEach(tagTreeNode => {
		tagTreeNode.ondblclick = event => {
			event.preventDefault();
			event.stopPropagation();
			FindChildTags(event.target.parentElement.parentElement, tagTreeNode);
		}
	})
}


function AppendChildTree(parentTag, childTags) {
	for (let i in childTags) {
		let resultItem = document.createElement("div");
		resultItem.innerHTML = childTags[i];
		resultItem.setAttribute("class", "ceiledTagNode");
		parentTag.append(resultItem);
		resultItem.ondblclick = event => {
			event.preventDefault();
			event.stopPropagation();
			FindChildTags(event.target.parentElement.parentElement, resultItem);
		}
	}
}

function FindChildTags(parentTag, tagTreeNode) {
	let tagID = tagTreeNode.querySelector(".ID").innerText;
	$.ajax({
		url: "/Home/FindChildTags",
		type: "POST",
		data: {
			tagID: tagID
		}
	}).done(resData => {
		AppendChildTree(parentTag, JSON.parse(resData));
		//FlushTagTree(parentTag);
		parentTag.ondblclick = undefined;
		Tag.FlushDrugEvent();
		Tag.FlushDropEvent();
	});
}

export {
	TagTree, FlushTagTree, AppendChildTree, FindChildTags
}

export default{
	TagTree,FlushTagTree,AppendChildTree,FindChildTags
}



