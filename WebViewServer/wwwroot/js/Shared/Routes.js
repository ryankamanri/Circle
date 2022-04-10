import { Router, Api, AddScript, Sleep } from "../My.js";
import Tag from "./Tag.js";
import Post from "./Post.js";
import Home from "../Home/Home.js";
import SendPost from "../Home/SendPost.js";
import PrivateChat from "../Home/PrivateChat.js";
import SearchResult from "../Home/SearchResult.js";
import TagTree from "../Home/TagTree.js";

let ckeditor = null;

async function Routes() {
    let mainMount = document.querySelector(".main-container main");
    let api = new Api();

    new Router().AddRoute("Home/Posts", async() => {
        mainMount.innerHTML = await api.Get("/Home/Home/Posts");
        Tag.Tag();
	    Post.Post();
        Home.Home();
    }).AddRoute("Home/SendPost", async() => {
        mainMount.innerHTML = await api.Get("/Home/Home/SendPost");
        Tag.Tag();
	    Post.Post();
        Home.Home();
        SendPost.SendPost();
        await AddCKEditor(mainMount);
    }).AddRoute("Home/Zone", async() => {
        mainMount.innerHTML = await api.Get("/Home/Home/Zone");
        Tag.Tag();
	    Post.Post();
        Home.Home();
    }).AddRoute("Match", async() => {
        mainMount.innerHTML = await api.Get("/Home/Match");
        Tag.Tag();
	    Post.Post();
    }).AddRoute("PrivateChat", async() => {
        mainMount.innerHTML = await api.Get("/Home/PrivateChat");
        Tag.Tag();
	    Post.Post();
        PrivateChat.PrivateChat();
    }).AddRoute("SearchResult", async() => {
        mainMount.innerHTML = await api.Get(`/Home/${window.location.hash.replace("#", "")}`);
        Tag.Tag();
	    Post.Post();
        SearchResult.SearchResult();
    }).AddRoute("TagTree", async() => {
        mainMount.innerHTML = await api.Get("/Home/TagTree");
        Tag.Tag();
	    Post.Post();
        TagTree.TagTree();
    })
    .Execute();
}

function AddCKEditor(mainMount) {
    if (ckeditor !== null) {
        let ckeditorTextarea = document.querySelector(".ckeditor");
        ckeditorTextarea.parentNode.insertBefore(ckeditor, ckeditorTextarea);
        ckeditorTextarea.style.display = "none";
        return;
    }

    AddScript(mainMount, "/lib/ckeditor/ckeditor.js");

    setTimeout(async() => {
        while(document.querySelector("#cke_edit") === null){
            await Sleep(500);
        }
        ckeditor = document.querySelector("#cke_edit");
    }, 500);
    

}

export default {
    Routes
}