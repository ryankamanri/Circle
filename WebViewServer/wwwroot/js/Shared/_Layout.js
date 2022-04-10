import Header from './Header.js';
import Tag from './Tag.js';
import Post from './Post.js';
import Sidebar from './Sidebar.js';
import { ShowAlert } from '../Show.js';
import Routes from './Routes.js';
import { Api } from '../My.js';





let mySelfTags;
let myInterestedTags;
let api = new Api();


function _Layout()
{


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

	
	Header.Header();
	Tag.Tag();
	Post.Post();
	Sidebar.Sidebar();
	Routes.Routes();
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
	_Layout,ChangeTagRelation,PostChangeRelation
}
export default{
	_Layout,ChangeTagRelation,PostChangeRelation
}
