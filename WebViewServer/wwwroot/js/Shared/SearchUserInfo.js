import { Site, ParseFunc} from '../My.js'
import { PostChangeRelation } from './_Layout.js'
import { ShowAlert } from '../Show.js'

let attentions;
let privateChats;

function SearchUserInfo()
{


	attentions = document.querySelectorAll(".attention");
	privateChats = document.querySelectorAll(".private-chat");

	if(attentions.length > 0)
		attentions.forEach(attention => {
			JudgeFocus(attention);
		});

	if(privateChats.length > 0)
		privateChats.forEach(privateChat => {
			//privateChat.onclick = event => AppendFocus(event);
		});
	
	
}

function JudgeFocus(btn)
{
	if(btn.getAttribute("isFocus") == "False"){
		btn.onclick = async event => await AppendFocus(event);
	}else{
		btn.onclick = async event => await RemoveFocus(event);
		btn.classList.remove("btn-outline-info");
		btn.classList.add("btn-info");
		btn.value = "已关注";
	}
}

async function AppendFocus(event)
{
	let btn = event.target;
	event.stopPropagation();
	let keyID = btn.getAttribute("ID");
	let resData = await PostChangeRelation("/Shared/AppendRelation", keyID,"User","Type","Focus");
	console.log(resData);
	btn.onclick = async event => await RemoveFocus(event);//这种必须写成匿名函数形式,直接写一个函数名会发生不可预知的执行情况

	btn.classList.remove("btn-outline-info");
	btn.classList.add("btn-info");
	btn.value = "已关注";
	ShowAlert("alert alert-info", "关注成功", "");
}

async function RemoveFocus(event,ChangeStyleAction)
{
	let btn = event.target;
	event.stopPropagation();
	let keyID = event.target.getAttribute("ID");
	let resData = await PostChangeRelation("/Shared/RemoveRelation", keyID,"User","Type","Focus");
	console.log(resData);
	event.target.onclick = async event => await AppendFocus(event);
	btn.classList.remove("btn-info");
	btn.classList.add("btn-outline-info");
	btn.value = "关注";
	ShowAlert("alert alert-info", "取关成功", "");
}

export{
	SearchUserInfo,AppendFocus,RemoveFocus 
}

export default
{
	SearchUserInfo,AppendFocus,RemoveFocus	
}