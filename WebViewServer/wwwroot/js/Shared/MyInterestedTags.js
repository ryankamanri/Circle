import Tag from "./Components/Tag.js";
import { ShowAlert } from '../../lib/Kamanri/Show.js';

let mySelfTags;
let myInterestedTags;
let api;

async function Init(services) {
	api = services.Api;
	await Tag.Init(services);
	
	const myTags = document.querySelector(".my-tags");
	Tag.FlushDrugEvent(myTags);
	Tag.FlushDropEvent(myTags);
	

    mySelfTags = document.querySelector("#mySelfTags");
	myInterestedTags = document.querySelector("#myInterestedTags");
	

	mySelfTags.addEventListener("dragstart", async event => {
		let resData = await ChangeTagRelation(event, "/Shared/RemoveRelation", "Type", "Self");
		ShowAlert("alert alert-success", resData, "");
	});
	mySelfTags.addEventListener("drop", async event => {
		let resData = await ChangeTagRelation(event, "/Shared/AppendRelation", "Type", "Self");
		ShowAlert("alert alert-success", resData, "");
	});

	myInterestedTags.addEventListener("dragstart", async event => {
		let resData = await ChangeTagRelation(event, "/Shared/RemoveRelation", "Type", "Interested");
		ShowAlert("alert alert-success", resData, "");
	});
	
	myInterestedTags.addEventListener("drop", async event => {
		let resData = await ChangeTagRelation(event, "/Shared/AppendRelation", "Type", "Interested");
		ShowAlert("alert alert-success", resData, "");
	});
}


async function ChangeTagRelation(event, url, relationName, relation)
{
	let DragTagCurrentID = event.dataTransfer.getData("id");
	let DragTag = document.getElementById(DragTagCurrentID);
	let DragTagID = DragTag.children[0].innerText;

	return await PostChangeRelation(url,DragTagID,"Tag",relationName,relation);
}

async function PostChangeRelation(url, ID, entityType, relationName, relation)
{
	return await api.Post(url, {
		ID: ID,
		entityType: entityType,
		relationName: relationName,
		relation: relation
	});

}

export{
	Init,ChangeTagRelation,PostChangeRelation
}
export default{
	Init,ChangeTagRelation,PostChangeRelation
}