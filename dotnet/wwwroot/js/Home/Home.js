import _Layout from '../Shared/_Layout.js'
import MainVue from '../MainVue.js'
import { parseFunc } from '../My.js'


function Home()
{
    
    let circleName = document.querySelector("a.navbar-brand").innerText;
    let vue = new MainVue.MainVue();
    let ShowMessage = vue.$data.store.Function_ShowMessage;
    if(window.location.pathname == "/Home")
        ShowMessage("alert alert-info",`欢迎来到 ${circleName} ,`,"点击左上角可切换圈子");
    
}


export default{
    Home
}


