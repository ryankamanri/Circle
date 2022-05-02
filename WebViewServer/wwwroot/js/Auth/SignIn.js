
function GetAuthCode() {
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
		if(data == "验证码发送成功"){
			ShowAlert("alert alert-success","😀",data);

		}

		else ShowAlert("alert alert-warning","😥",data);
	});
}

async function SignIn() {
	return new Promise((resolve, reject) => {
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
			if(data == "注册成功")
			{
				resolve();
			}
			else ShowAlert("alert alert-warning","😥",data);
		}).fail(() => {
			ShowAlert("alert alert-danger","😭","注册失败,请联系管理员")
			reject();
		});
	})
	
}

function Summit() {
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
}

function Init()
{

	$("#getauthcode").on("click",() => 
	{
		GetAuthCode();
	});

	// const signin = document.querySelector("#signin");
	// signin.onclick = () => {
	// 	SignIn();
	// }

	$("#summit").on("click",() => {
		Summit();
	});
	return this;
	


}

async function ShowAlert(bootstrapClass, emphasis, message, alertTime=1500)
{
	let messageElem = document.querySelector("body>#alert");
	if (messageElem == null ) {
		messageElem = document.createElement("div");
		messageElem.id = "alert";
		let style = messageElem.style;
		style.opacity = 0;
		style.position = "fixed";
		style.top = "10%";
		style.left = "50%";
		style.zIndex = 100;
		style.setProperty("transform", "translate(-50%, -50%)");

		document.querySelector("body").append(messageElem);
	}

	messageElem.className = bootstrapClass;
	messageElem.innerHTML = `<strong>${emphasis}</strong>${message}`;

	Animate(messageElem, {
		opacity : 1
	});

	await Sleep(alertTime);

	Animate(messageElem, {
		opacity : 0
	});
}

function Sleep(value) {
	return new Promise(resolve => {
		setTimeout(() => resolve(value), value);
	});
}

async function Animate(element, properties, duration="slow", easing="swing") {
	return new Promise(resolve => {
		$(element).animate(properties, duration, easing, value => resolve(value));
	});

}

