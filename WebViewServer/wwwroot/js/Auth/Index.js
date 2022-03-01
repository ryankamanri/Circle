import { Show, ShowAlert, ShowLoad } from "../Show.js"
import { ParseFunc, Sleep, Storage, Animate } from '../My.js'


let storage;

function Index()
{
	let app = document.querySelector("#app");
	if (document.readyState != "complete") {
		ShowLoad(app, "加载中...", async fragment => {
			while (document.readyState != "complete")
				await Sleep(500);
			ShowAlert("alert alert-info", "", "欢迎来到校友互助共享圈!");

		});
	}
	
	
}


export {
	Index
}


export default{
	Index
}