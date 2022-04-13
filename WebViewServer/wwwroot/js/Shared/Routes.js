import { Router, Api, AddScript, Sleep } from "../My.js";
import Tag from "./Tag.js";
import Post from "./Post.js";
import Home from "../Home/Home.js";
import SendPost from "../Home/SendPost.js";
import PrivateChat from "../Home/PrivateChat.js";
import SearchResult from "../Home/SearchResult.js";
import TagTree from "../Home/TagTree.js";




async function Init() {

    let mainMount = document.querySelector(".main-container main");
    let api = new Api();

    new Router().AddRoute("Home/Posts", async() => {

        await InitBase(mainMount, api);
        Home.Init();

    }).AddRoute("Home/SendPost", async() => {

        await InitBase(mainMount, api);
        Home.Init();
        await AddCKEditor(mainMount);
        SendPost.Init();

    }).AddRoute("Home/Zone", async() => {

        await InitBase(mainMount, api);
        Home.Init();

    }).AddRoute("Match", async() => {

        await InitBase(mainMount, api);

    }).AddRoute("PrivateChat", async() => {

        await InitBase(mainMount, api);
        PrivateChat.Init();

    }).AddRoute("SearchResult", async() => {

        await InitBase(mainMount, api);
        SearchResult.Init();

    }).AddRoute("TagTree", async() => {

        await InitBase(mainMount, api);
        TagTree.Init();

    })
    .Execute();
}

async function InitBase(mainMount, api){
    
    mainMount.innerHTML = await api.Get(`/Home/${window.location.hash.replace("#", "")}`);
    Tag.Init();
	Post.Init();
}

async function AddCKEditor(mainMount) {

    // await AddScript(mainMount, "/lib/ckeditor5-build-classic/ckeditor.js");

    ClassicEditor
    .create( document.querySelector( '.ckeditor' ) )
    .catch( error => {
        console.error( error );
    } );

}

export default {
    Init
}