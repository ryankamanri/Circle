import { ShowAlert } from '../Show.js';

function Init()
{
	$("#summit").on("click",() => {
        let nickname = $("#nickname").val();
        let realname = $("#realname").val();
        let university = $("#university").val();
        let school = $("#school").val();
		let speciality = $("#speciality").val();
		let schoolyear = $("#schoolyear").val();
		$.ajax({
			url : "/Information_Submit",
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
		}).done( data => {
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
}

export {
	Init
}

// export default{
	// Init
// }
