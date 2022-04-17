import { Router, Api, AddScript, Sleep, MyWebSocket } from "./My.js";
import Tag from "./Shared/Tag.js";
import Post from "./Shared/Post.js";
import Home from "./Home/Home.js";
import SendPost from "./Home/SendPost.js";
import PrivateChat from "./Home/PrivateChat.js";
import SearchResult from "./Home/SearchResult.js";
import TagTree from "./Home/TagTree.js";




async function Init(services) {

    let mainMount = document.querySelector(".main-container main");
    let api = services.Api;

    await new Router().AddRoute("Home/Posts", async() => {

        await InitBase(services, mainMount, api);
        Home.Init(services);

    }).AddRoute("Home/SendPost", async() => {

        await InitBase(services, mainMount, api);
        Home.Init(services);
        await AddCKEditor();
        SendPost.Init(services);

    }).AddRoute("Home/Zone", async() => {

        await InitBase(services, mainMount, api);
        Home.Init(services);

    }).AddRoute("Match", async() => {

        await InitBase(services, mainMount, api);

    }).AddRoute("PrivateChat", async() => {

        await InitBase(services, mainMount, api);
        PrivateChat.Init(services);

    }).AddRoute("SearchResult", async() => {

        await InitBase(services, mainMount, api);
        SearchResult.Init(services);

    }).AddRoute("TagTree", async() => {

        await InitBase(services, mainMount, api);
        TagTree.Init(services);

    })
    .Execute();
}

async function InitBase(services, mainMount, api){
    
    mainMount.innerHTML = await api.Get(`/Home/${window.location.hash.replace("#", "")}`);
    Tag.Init(services);
	Post.Init(services);
    PrivateChat.InitBase(services);

    services.MyWebSocket.AddEventHandler(MyWebSocket.WebSocketMessageEvent.OnServerConnect, async (wsMessages) => {
		console.log(wsMessages);
		console.log(`\n${JSON.stringify(wsMessages)}`);
		const assignID = new Number(wsMessages[0].Message);
        services.AssignID = assignID;
		if (services.UserInfo != undefined) {
			await new Promise(resolve => {
				services.MyWebSocket.SendMessages([
					new MyWebSocket.WebSocketMessage(
						MyWebSocket.WebSocketMessageEvent.OnClientConnect,
						MyWebSocket.WebSocketMessageType.Text,
						assignID.toString()
					),
					new MyWebSocket.WebSocketMessage(
						MyWebSocket.WebSocketMessageEvent.OnClientConnect,
						MyWebSocket.WebSocketMessageType.Text,
						String(services.UserInfo.ID)
					)
				]);
			});
		}
	});
}

async function AddCKEditor() {

    ClassicEditor
    .create( document.querySelector( '.ckeditor' ) )
    .catch( error => {
        console.error( error );
    } );

}

export default {
    Init
}