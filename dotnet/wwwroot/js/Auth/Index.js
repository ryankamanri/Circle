import Show from "../Show.js"
import MainVue from '../MainVue.js'
import {parseFunc,Sleep} from '../My.js'

let messageComponent = {
    template : `
    <div :id="id" :class="class">{{message}}</div>`,
    props : ["id","class","message"]
}

let vue;

function Index()
{
    Show.Show();
    vue = MainVue.MainVue();

    vue.$data.store.components_message = messageComponent;
    // vue.$data.store.func = {
    //     ShowMessage : ShowMessage.toString()
    // };
    vue.$data.store.Function_ShowMessage = ShowMessage;
    //ShowMessage("alert alert-info","","欢迎来到校友互助共享圈!");
    vue.$data.store.Function_ShowMessage("alert alert-info","","欢迎来到校友互助共享圈!");
}

async function ShowMessage(bootstrapClass,emphasis,message)
{
    let messageElem = document.querySelector("body>.alert");
    if (messageElem == null ) {
        messageElem = document.createElement("div");
        
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
    
    $(messageElem).animate({
        opacity : 1
    });

    await new Promise(resolve => {
        setTimeout(() => resolve(), 2000);
    });

    $(messageElem).animate({
        opacity : 0
    });
}

export default{
    Index
}