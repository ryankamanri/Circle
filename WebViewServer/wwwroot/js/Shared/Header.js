import { Sleep } from '../My.js'
import Tag from '../Shared/Components/Tag.js'

function Init(services)
{
	let count = 0;//统计一段时间要发送的ajax请求个数
	let resultList;

	

	$(document).ready(() => {
		$("form #search").keyup(async() => { //这个事件一定要嵌套在ready()里面
			count++;
			await Sleep(500);
			count--;
			if(count > 0) return;
			SearchTag();
		});
		$("#logout").on("click", () => {
			$.ajax({
				url: "/LogOutSubmit",
				type: "POST"
			}).done(data => {
				if (data == "logout succeed")
					window.location.href = "/";
			});
		});
		$("form #searchButton").on("click",() => {
			let searchString = $("#search").val();
			window.location.href = `/Home#SearchResult?searchString=${encodeURIComponent(searchString)}`;
		})
		$("input#search").focus(() => {
			document.onkeypress = (event) => {
				
				if (event.code === "Enter") {
					event.preventDefault();
					event.stopPropagation();
					let searchString = $("#search").val();
					window.location.hash = `#SearchResult?searchString=${encodeURIComponent(searchString)}`;
				}
			}
		});
		$("input#search").blur(() => {
			document.onkeypress = null;
		});
	});

	function SearchTag() {
		let search = $("#search").val();
		if(search == "") return;
		console.log(search);
		$.ajax({
			url: "/Shared/Search",
			type: "POST",
			data: {
				"search": search
			}
		}).done((data) => {
			if (data == "bad request") console.log(data);
			if(data == "[]") return;
			resultList = JSON.parse(data);
			ClearResults();
			AppendResults(resultList);
			Tag.FlushDrugEvent(document.querySelector("header"));
		}).fail(() =>
			console.log("send failure")
		);
	}

	function AppendResults(obj)
	{
		let searchResult = document.querySelector("#searchResult");
		obj.forEach(item =>
		{
			let resultItem = document.createElement("li");
			resultItem.innerHTML = item;
			resultItem.setAttribute("class","self-dropdown-item");
			searchResult.append(resultItem);
		});
		
	}

	function ClearResults()
	{
		$("#searchResult").empty();
	}
}

export {
	Init
}
export default{
	Init
}