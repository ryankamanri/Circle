import {Show,ShowMessage} from "../Show.js"
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
    Show();
    vue = MainVue.MainVue();

    vue.$data.store.components_message = messageComponent;

    vue.$data.store.Function_ShowMessage = ShowMessage;

    vue.$data.store.Function_ShowMessage("alert alert-info","","欢迎来到校友互助共享圈!");
}



export default{
    Index
}