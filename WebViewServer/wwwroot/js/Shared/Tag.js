import { Mutex , Sleep, CopyElement } from '../My.js';

let mutex = new Mutex();
function Init(services)
{
	FlushDrugEvent();
	FlushDropEvent();
}

let dragTagID = "";

function FlushDrugEvent() {
	let tags = document.querySelectorAll(".tag");
	tags.forEach(tag => {
		tag.ondragstart = event => {
			event.dataTransfer.setData("id", event.target.id);
			dragTagID = event.target.id;
		}
	});
}

function FlushDropEvent() {
	let tagboxes = document.querySelectorAll(".tagbox");
	let tagNodes = document.querySelectorAll(".tagNode,.ceiledTagNode");
	let id;
	let moveTag, originTag;
	let isDropped = false;
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
			if (tag != null) tagbox.appendChild(tag);
			else tagbox.appendChild(document.querySelector("iframe.tag-tree").contentDocument.getElementById(id));
			isDropped = true;
		};
		
	});

	tagNodes.forEach(tagNode => {
		tagNode.ondragstart = async event => {
			
			event.stopPropagation();
			id = event.dataTransfer.getData("id");
			moveTag = document.getElementById(id);
			originTag = CopyElement(moveTag);
			originTag.id++;
			mutex.mutex = true;
			isDropped = false;
			do{
				await Sleep(100);
				if (tagNode.children[0] === undefined)//标签已被取走
					tagNode.insertBefore(originTag, tagNode.children[0]);
				originTag.addEventListener("dragstart", event => {
					event.dataTransfer.setData("id", event.target.id);
				});
			}while(!isDropped);
			
		};
	});
}

export {
	Init, FlushDrugEvent, FlushDropEvent
}

export default{
	Init,FlushDrugEvent,FlushDropEvent
}



