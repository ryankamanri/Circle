
import Tag from '../Shared/Components/Tag.js'

let tagRoot;

async function Init(services)
{
	await Tag.Init(services);

	FlushTagTree();

	tagRoot = document.querySelector("#tagRoot");
	Tag.FlushDropEvent(tagRoot);
	Tag.FlushDrugEvent(tagRoot);
	tagRoot.onmousewheel = event => {
		event.preventDefault();
		event.stopPropagation();
		tagRoot.scrollLeft += event.deltaY;
	}
}

function FlushTagTree() {
	let tagTreeNodes = document.querySelectorAll("#tagRoot .ceiledTagNode");
	tagTreeNodes.forEach(tagTreeNode => {
		tagTreeNode.onclick = event => {
			event.preventDefault();
			event.stopPropagation();
			FindChildTags(event.target.parentElement.parentElement, tagTreeNode);
		}
	})
}


function AppendChildTree(parentTag, childTags) {
	childTags.forEach( childTag => {
		let resultItem = document.createElement("div");
		resultItem.innerHTML = childTag;
		resultItem.setAttribute("class", "ceiledTagNode");
		parentTag.append(resultItem);
		resultItem.onclick = event => {
			event.preventDefault();
			event.stopPropagation();
			FindChildTags(event.target.parentElement.parentElement, resultItem);
		}
	});
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
		parentTag.onclick = undefined;
		Tag.FlushDrugEvent(document.querySelector("#tagRoot"));
		Tag.FlushDropEvent(document.querySelector("#tagRoot"));
	});
}

export {
	Init, FlushTagTree, AppendChildTree, FindChildTags
}

export default{
	Init,FlushTagTree,AppendChildTree,FindChildTags
}



