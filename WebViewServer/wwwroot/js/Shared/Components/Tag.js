import { Sleep, CopyElement, GenerateIDString } from "../../My.js";

const tagTemplate = document.createElement("template");
const tagNodeTemplate = document.createElement("template");

// 判断拖拽的标签有没有放置
let isDroppedTag = false;
let drugTagID = "";

function GetIsDroppedTag() {
    return isDroppedTag;
}

function SetIsDroppedTag(value) {
    isDroppedTag = value;
}

async function Init(services) {
    const tagModelStr = await services.Api.Get("/Shared/Components/Tag");
    if (tagTemplate.innerHTML === "") {
        tagTemplate.innerHTML = tagModelStr;
    }
    if(tagNodeTemplate.innerHTML === "") {
        const div = document.createElement("div");
        div.className = "tagNode";
        div.innerHTML = tagModelStr;
        tagNodeTemplate.content.appendChild(div);
    }
     
}

function SetItemTemplate(viewType) {
    switch (viewType) {
        case "tag":
            return tagTemplate;
        case "tagNode":
            return tagNodeTemplate;
        default:
            break;
    }
}

function SetTemplateViewToModelBinder(view, model, viewType) {
    let tagDiv = view;
    if(viewType === "tagNode") {
        tagDiv = view.querySelector(".tag");
    }
    tagDiv.setAttribute("id", GenerateIDString());
    tagDiv.setAttribute("title", model._Tag);
    const colorObj = RandomColor();
    tagDiv.style.background = `rgb(${colorObj.br},${colorObj.bg},${colorObj.bb})`;
    tagDiv.children[0].innerText = model.ID;
    tagDiv.children[1].innerText = model._Tag;
    tagDiv.children[1].style.color = `rgb(${colorObj.fr},${colorObj.fg},${colorObj.fb})`;

}

function FlushDrugEvent(view) {
	let tags = view.querySelectorAll(".tag");
	tags.forEach(tag => {
		tag.ondragstart = event => {
			event.dataTransfer.setData("id", event.target.id);
			console.log(event.target.id);
		}
	});
}

function FlushDropEvent(view) {
	let tagboxes = view.querySelectorAll(".tagbox");
	let tagNodes = view.querySelectorAll(".tagNode,.ceiledTagNode");
	let id;
	let moveTag, originTag;
	tagboxes.forEach(tagbox => {
		tagbox.ondragstart = async(event) => {
			
			id = event.dataTransfer.getData("id");
			moveTag = document.getElementById(id);
			await Sleep(1000);
			moveTag.style.display = "none";
		}
		tagbox.ondragover = event => {
			event.preventDefault();
		}
		tagbox.ondrop = event => {
			id = event.dataTransfer.getData("id");
			let tag = document.getElementById(id);
			
			if (tag != null){
				tag.style.display = '';
				tagbox.appendChild(tag);
			}
			else tagbox.appendChild(document.querySelector("iframe.tag-tree").contentDocument.getElementById(id));
			isDroppedTag = true;
		};
		
	});

	tagNodes.forEach(tagNode => {
		tagNode.ondragstart = async event => {
			
			event.stopPropagation();
			id = event.dataTransfer.getData("id");
			moveTag = document.getElementById(id);
			originTag = CopyElement(moveTag);
			originTag.id++;
			isDroppedTag = false;
			do{
				await Sleep(100);
				if (tagNode.children[0] === undefined)//标签已被取走
					tagNode.insertBefore(originTag, tagNode.children[0]);
				originTag.addEventListener("dragstart", event => {
					event.dataTransfer.setData("id", event.target.id);
				});
			}while(!isDroppedTag);
			
		};
	});
}

function RandomColor(){
    const r = Math.floor(Math.random() * 255);
    const g = Math.floor(Math.random() * 255);
    const b = Math.floor(Math.random() * 255);
    return {
        br : r,
        bg : g,
        bb : b,
        fr : (r + 128) % 256,
        fg : (g + 128) % 256,
        fb : (b + 128) % 256
    }

 }

export default {
    Init, GetIsDroppedTag, SetIsDroppedTag, SetItemTemplate, SetTemplateViewToModelBinder, FlushDropEvent, FlushDrugEvent
}