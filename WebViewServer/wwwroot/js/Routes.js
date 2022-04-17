import { Router, Api, AddScript, Sleep, MyWebSocket } from "./My.js";
import Tag from "./Shared/Tag.js";
import Post from "./Shared/Post.js";
import Home from "./Home/Home.js";
import SendPost from "./Home/SendPost.js";
import PrivateChat from "./Home/PrivateChat.js";
import SearchResult from "./Home/SearchResult.js";
import TagTree from "./Home/TagTree.js";
import Zone from "./Home/Zone.js";

const baseTitle = document.title;
const mainMount = document.querySelector(".main-container main");

function SetSubtitle(subtitle) {
    document.title = `${subtitle} ${baseTitle}`;
}


async function Init(services) {

    await new Router().AddRoute("Home/Posts", async() => {
        SetSubtitle("帖子");
        await InitView(services);
        Home.Init(services);
        await InitBase(services);

    }).AddRoute("Home/SendPost", async() => {
        SetSubtitle("发帖");
        await InitView(services);
        await InitBase(services);
        SendPost.Init(services);
        await AddCKEditor();

    }).AddRoute("Home/Zone", async() => {
        SetSubtitle("动态");
        await InitView(services);
        Zone.Init(services);
        await InitBase(services);

    }).AddRoute("Match", async() => {
        SetSubtitle("匹配");
        await InitView(services);
        await InitBase(services);

    }).AddRoute("PrivateChat", async() => {
        SetSubtitle("私聊");
        await InitView(services);
        await InitBase(services);
        PrivateChat.Init(services);

    }).AddRoute("SearchResult", async() => {
        SetSubtitle("搜索结果");
        await InitView(services);
        await InitBase(services);
        SearchResult.Init(services);

    }).AddRoute("TagTree", async() => {
        SetSubtitle("标签树");
        await InitView(services);
        await InitBase(services);
        TagTree.Init(services);

    })
    .Execute();
}

async function InitView(services) {
    const viewStr = await services.Api.Get(`/Home/${window.location.hash.replace("#", "")}`);
    mainMount.innerHTML = viewStr;
}

async function InitBase(services){

    Tag.Init(services);
	// Post.Init(services);
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