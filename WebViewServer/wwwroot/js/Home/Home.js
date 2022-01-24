import _Layout from '../Shared/_Layout.js';
import { parseFunc, Storage } from '../My.js';
import { ShowMessage } from '../Show.js';


function Home()
{
    
    let circleName = document.querySelector("a.navbar-brand").innerText;
    let storage = Storage.Storage();
    if(window.location.pathname == "/Home")
        ShowMessage("alert alert-info",`欢迎来到 ${circleName} ,`,"点击左上角可切换圈子");
    
}


export default{
    Home
}


