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



function parseElement(str) {
	var o=document.createElement("div");
	o.innerHTML=str;
	return o.childNodes[0];
}

function stringlify(obj){
	var o=document.createElement("div");
	o.appendChild(obj);
	return o.innerHTML;
}

function parseFunc(str)
{
    return new Function(`return ${str}`)();
}

export{
    Site, Sleep, parseElement, stringlify, parseFunc
}