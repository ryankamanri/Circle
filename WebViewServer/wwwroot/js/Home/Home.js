import { ParseFunc, Storage } from '../My.js';
import { ShowAlert } from '../Show.js';


function Init()
{
	
	let circleName = document.querySelector("a.navbar-brand").innerText;
	let storage = Storage.Storage();
	if(window.location.pathname == "/Home")
		ShowAlert("alert alert-info",`欢迎来到 ${circleName} ,`,"点击左上角可切换圈子");
	
}

export {
	Init
}

export default{
	Init
}


