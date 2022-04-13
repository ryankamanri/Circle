import {Site} from '../My.js'
import { ShowAlert } from '../Show.js'

let btn,title,focus,tagIDs,summary,contentDocument;
let tagCollection;


function Init()
{

	btn = document.querySelector("input#submit");
	
	btn.onclick = () => SendPostSubmit();
}

function SendPostSubmit()
{
	tagIDs = [];
	title = document.querySelector("#title").value;
	focus = document.querySelector("#focus").value;
	
	contentDocument = document.querySelector(".ck-editor__main .ck-content");

	tagCollection = document.querySelector("#post-tags").querySelectorAll(".ID");
	tagCollection.forEach(tagID => {
		tagIDs.push(tagID.innerText);
	})
	Site.Post("/Home/SendPostSubmit",{
		Title : title,
		Focus : focus,
		Summary : contentDocument.innerText.substring(0,100),
		Content : contentDocument.innerHTML,
		TagIDs : JSON.stringify(tagIDs)
	},async resData => {
		console.log(resData);
		if(resData == "add succeed")
		{
			await ShowAlert("alert alert-success","😀","发布成功");
			window.location.href = "/Home#Home/Posts";
		} 
	},()=>{});
}

export {
	Init
}

export default{
	Init
}