import { ParseFunc } from '../My.js';
import { ShowAlert } from '../Show.js'

function Init()
{

	$("#getauthcode").on("click",() => 
	{
		let account = $("#account").val();
		$.ajax({
			url : "/GetAuthCode",
			type : "POST",
			data : {
				User : [
					account
				]
			}
		}).done(data => {
			console.log(data);
			if(data == "验证码发送成功")
				ShowAlert("alert alert-success","😀",data);
			else ShowAlert("alert alert-warning","😥",data);
		});
	});

	const signin = document.querySelector("#signin");
	signin.onclick = () => {
		console.log("click")
		let account = $("#account").val();
		let password = $("#password").val();
		let authCode = $("#authcode").val();
		$.ajax({
			url : "/SignInSubmit",
			type : "POST",
			data : {
				User : [
					account,
					password,
					authCode
				]
			}
		}).done((data) => {
			debugger
			if(data == "注册成功") 
			{
				// await ShowAlert("alert alert-success","😀",data);
				window.location.href = "/LogIn";

			}
			ShowAlert("alert alert-warning","😥",data);
		}).fail(() => {
			debugger
			ShowAlert("alert alert-danger","😭","注册失败,请联系管理员")
		});


		


	}

	$("#summit").on("click",() => {
        let nickname = $("#nickname").val();
        let realname = $("#realname").val();
        let university = $("#university").val();
        let school = $("#school").val();
		let speciality = $("#speciality").val();
		let schoolyear = $("#schoolyear").val();
		if(schoolyear === "")
		{
			ShowAlert("alert alert-warning","🤷‍♂️","请输入时间");
			return;
		}
			
		if(nickname === ""){
			ShowAlert("alert alert-warning","🤷‍♂️","请输入昵称");
			return;
		}
		if(university === ""){
			ShowAlert("alert alert-warning","🤷‍♂️","请输入学校");
			return;
		}
		if(school === ""){
			ShowAlert("alert alert-warning","🤷‍♂️","请输入学院");
			return;
		}
		if(speciality === ""){
			ShowAlert("alert alert-warning","🤷‍♂️","请输入专业");
			return;
		}
		$.ajax({
			url : "/Information_Summit",
			type : "POST",
			data : {
				UserInfo : [
					nickname,
					realname,
					university,
					school,
					speciality,
					schoolyear	
				]
			}
		}).done((data) => {
			if(data == "1") 
			{   
				ShowAlert("alert alert-success","😀","信息提交成功");
				window.location.href = "/Home#Home/Posts";
			}
			else{
				ShowAlert("alert alert-warning","😥",data)
			}
			
		}).fail(() =>{
			ShowAlert("alert alert-danger","😭","提交失败,请联系管理员");
		});
	});
	return this;
	

	// $("#signin").on("click",() => {
	// 	let account = $("#account").val();
	// 	let password = $("#password").val();
	// 	let authCode = $("#authcode").val();
	// 	$.ajax({
	// 		url : "/SignInSubmit",
	// 		type : "POST",
	// 		data : {
	// 			User : [
	// 				account,
	// 				password,
	// 				authCode
	// 			]
	// 		}
	// 	}).done(async (data) => {
	// 		if(data == "注册成功") 
	// 		{
	// 			await ShowAlert("alert alert-success","😀",data);
	// 			window.location.href = "/LogIn";
	// 		}
	// 		ShowAlert("alert alert-warning","😥",data);
	// 	}).fail(() =>
	// 		//alert("send failure");
	// 		ShowAlert("alert alert-danger","😭","登录失败,请联系管理员")
	// 	);
	// });
}

export {
	Init
}
export default{
	Init
}