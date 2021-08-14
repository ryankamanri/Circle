import Vue from '../lib/vue-dev/dist/vue.js'
import {parseFunc} from './My.js'

let vue;
let mainVue_store;
let saver = {
    get : (obj,prop) => 
    {
        if(prop.toString().match(/Function_.*/g) == prop) return parseFunc(obj[prop]);
        return obj[prop];
    },
    set : (obj,prop,value) => {
        if(typeof(value) == "function") obj[prop] = value.toString();
        else obj[prop] = value;
        Save(obj);
        return true;
    }
}



function MainVue(obj)
{
    if(obj == undefined)
        obj = new Object({
            data : {
                store : {}
            }
        });

    if(obj.data == undefined)
        obj.data = new Object({
            store : {}
        })
    
    mainVue_store = Get();

    if(mainVue_store == undefined) mainVue_store = new Object();
    
    obj.data.store = new Proxy(mainVue_store,saver);

    vue = new Vue.Vue(obj);
    window.vue = vue;
    return vue;
}

function Get()
{
    return JSON.parse(window.localStorage.getItem("mainVue_store"));
}

function Save(data)
{
    window.localStorage.setItem("mainVue_store",JSON.stringify(data));
}

function Clear()
{
    vue.$data.store = new Proxy(new Object(),saver);
    window.localStorage.removeItem("mainVue_store");
}

export{
    MainVue,Clear
}

export default{
    MainVue,Clear
}