import { ParseFunc } from './Utils.js'

let obj;
let kamanriStorage;
let saver = {
	get : (obj,prop) => 
	{
		if(prop.toString().match(/Function_.*/g) == prop) return ParseFunc(obj[prop]);
		return obj[prop];
	},
	set : (obj,prop,value) => {
		if(typeof(value) == "function") obj[prop] = value.toString();
		else obj[prop] = value;
		Save(obj);
		return true;
	}
}



function Storage()
{
	kamanriStorage = Get();

	if(kamanriStorage == undefined) kamanriStorage = new Object();
	
	obj = new Proxy(kamanriStorage,saver);

	return obj;
}

function Get()
{
	return JSON.parse(window.localStorage.getItem("kamanriStorage"));
}

function Save(data)
{
	window.localStorage.setItem("kamanriStorage", JSON.stringify(data));
}

function Clear()
{
	obj = new Proxy(new Object(), saver);
	window.localStorage.removeItem("kamanriStorage");
}


export default{
	Storage, Clear
}