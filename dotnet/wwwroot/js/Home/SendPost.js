import {Site} from '../My.js'
import MainVue from '../MainVue.js'
import {parseFunc} from '../My.js'
let btn,title,focus,tagIDs,summary,contentDocument;
let tagCollection;
let vue;

function SendPost()
{
    vue = MainVue.MainVue();

    btn = document.querySelector("input#submit");
    
    btn.onclick = () => SendPostSubmit();
}

function SendPostSubmit()
{
    tagIDs = [];
    title = document.querySelector("#title").value;
    focus = document.querySelector("#focus").value;
    
    contentDocument = document.querySelector("iframe").contentDocument.querySelector("body");

    tagCollection = document.querySelector("#post-tags").querySelectorAll(".ID");
    tagCollection.forEach(tagID => {
        tagIDs.push(tagID.innerText);
    })
    Site.Post("/Home/SendPostSubmit",{
        Title : title,
        Focus : focus,
        Summary : contentDocument.innerText.substring(0,40),
        Content : contentDocument.innerHTML,
        TagIDs : JSON.stringify(tagIDs)
    },async resData => {
        console.log(resData);
        if(resData == "add succeed")
        {
            await parseFunc(vue.$data.store.func.ShowMessage)("alert alert-success","😀","发布成功");
            window.location.href = "/Home";
        } 
    },()=>{});
}

export default{
    SendPost
}