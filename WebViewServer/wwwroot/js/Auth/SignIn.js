import { ShowAlert } from '../My.js';

function SignIn()
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

	$("#signin").on("click",() => {
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
		}).done(async (data) => {
			if(data == "注册成功") 
			{
				await ShowAlert("alert alert-success","😀",data);
				window.location.href = "/SelectCircle";
			}
			ShowAlert("alert alert-warning","😥",data);
		}).fail(() =>
			//alert("send failure");
			ShowAlert("alert alert-danger","😭","登录失败,请联系管理员")
		);
	});
}

export {
	SignIn
}
export default{
	SignIn
}