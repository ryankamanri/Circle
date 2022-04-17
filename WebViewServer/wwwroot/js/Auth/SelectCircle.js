import { Sleep, ShowLoad } from '../My.js';

async function SelectCircle()
{
	let body = document.querySelector("body");
	if (document.readyState != "complete") {
		await ShowLoad(body, "加载中", async fragment => {
			while (document.readyState != "complete") await Sleep(500);
		});
	}
	
	$("#ExamCircle").on("click", () => {
		$.ajax({
			url: "/SelectCircleSubmit",
			type: "POST",
			data: {
				Circle: "考研圈"
			}
		}).done(data => {
			console.log(data);
			window.location.href = "/Home#Home/Posts";
		});
	});

	$("#EmploymentCircle").on("click", () => {
		$.ajax({
			url: "/SelectCircleSubmit",
			type: "POST",
			data: {
				Circle: "就业圈"
			}
		}).done(data => {
			console.log(data);
			window.location.href = "/Home#Home/Posts";
		});
	});
	

}

export {
	SelectCircle
}
export default{
	SelectCircle
}
