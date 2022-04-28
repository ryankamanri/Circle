
import { ShowAlert } from '../My.js';

function LogIn()
{
	$("#login").on("click",() => {
		let account = $("#account").val();
		let password = $("#password").val();
		$.ajax({
			url : "/LogInSubmit",
			type : "POST",
			data : {
				User : [
					account,
					password
				]
			}
		}).done(data => {
			if(data == "登录成功") 
			{   
				ShowAlert("alert alert-success","😀",data);
				window.location.href = "/SelectCircle";
			}
			else{
				ShowAlert("alert alert-warning","😥",data)
			}
			
		}).fail(() =>{
			ShowAlert("alert alert-danger","😭","登录失败,请联系管理员");
		});
	});
	return this;
}

export {
	LogIn
}

export default{
	LogIn
}

