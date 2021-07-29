import Vue from '../lib/vue-dev/dist/vue.js'

let vue;
let mainVue_store;
let saver = {
    get : (obj,prop) => obj[prop],
    set : (obj,prop,value) => {
        obj[prop] = value;
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
    
    mainVue_store = JSON.parse(window.localStorage.getItem("mainVue_store"));
    if(mainVue_store == undefined) mainVue_store = new Object({
        components : {}
    });
    
    obj.data.store = new Proxy(mainVue_store,saver);

    vue = new Vue.Vue(obj);
    return vue;
}



function Save(data)
{
    window.localStorage.setItem("mainVue_store",JSON.stringify(data));
}

function Clear()
{
    vue.$data.store = new Proxy(new Object,saver);
    window.localStorage.removeItem("mainVue_store");
}

export{
    MainVue,Clear
}

export default{
    MainVue,Clear
}