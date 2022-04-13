var Site = {
	//设置每个按钮的post请求
	Post : (url,reqData,CallBack,FailHandler) =>
	{
		var resData = "";
		$.ajax({
			url: url,
			type: "POST",
			data: reqData
			
		}).done(data => {
			CallBack(data);
			
		}).fail((action,state,event) =>{
			console.log(action);
			console.log(state);
			console.log(event);
			FailHandler(action,state,event);
		});
	},
	GetJSONObject : jsonStr =>
	{
		return JSON.parse(unescape(jsonStr.replace(/\"/g,"").replace(/\\u0022/g,'"').replace(/\\u/g, "%u")));
	}
}

function Sleep(value) {
	return new Promise(resolve => {
		setTimeout(() => resolve(value), value);
	});
}



function ParseElement(str) {
	var o=document.createElement("div");
	o.innerHTML=str;
	return o.childNodes[0];
}

function Stringlify(obj){
	var o=document.createElement("div");
	o.appendChild(obj);
	return o.innerHTML;
}

function ParseFunc(str)
{
	return new Function(`return ${str}`)();
}

function StrIncrement(strNum, inc) {
	let num = Number(strNum);
	num += inc;
	return String(num);
}

async function Animate(element, properties, duration="slow", easing="swing") {
	return new Promise(resolve => {
		$(element).animate(properties, duration, easing, value => resolve(value));
	});
	
}

function GetType(obj) {
	return Object.prototype.toString.call(obj)
}
function CopyElement(element) {
	let copyElement = document.createElement(element.tagName);
	copyElement.innerHTML = element.innerHTML;
	let attrCount = element.attributes.length;
	for (let i = 0; i < attrCount; i++) {
		copyElement.setAttribute(element.attributes[i].name, element.attributes[i].value);
	}
	return copyElement;
}
function GenerateIDString() {
	return `${Date.now()}+${(Math.random() * 10000)}`
}

function AddScript(mountElement, src=null, statement=null, isModule=false) {
	let script = document.createElement("script");
	if(src !== null){
		script.src = src;
	}
	else if(statement !== null){
		script.innerText = statement;
		if(isModule) script.type = "module";
	}
	return new Promise((resolve, reject) => {
		try {
			mountElement.appendChild(script);
			script.onerror = (e) => reject(e);
			script.onload = () => {
				resolve();
			}
		} catch (error) {
			reject(error);
		}
	});
	
}

export {
	Site, Sleep, ParseElement, Stringlify, ParseFunc, StrIncrement, Animate, GetType, CopyElement, GenerateIDString, AddScript
}