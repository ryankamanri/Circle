import {Router, Api, AddScript, Sleep, MyWebSocket, ShowLoad} from "./My.js";
import Header from './Shared/Header.js';
import Tag from "./Shared/Components/Tag.js";
import Home from "./Home/Home.js";
import SendPost from "./Home/SendPost.js";
import PrivateChat from "./Home/PrivateChat.js";
import SearchResult from "./Home/SearchResult.js";
import TagTree from "./Home/TagTree.js";
import Zone from "./Home/Zone.js";
import UserPage from "./Home/UserPage.js"
import Match from "./Home/Match.js"
import MatchButton from "./Home/MatchButton.js";
import AccountInfo from "./Home/AccountInfo.js";
import Setting from "./Home/Setting.js";
import PostItem from "./Home/PostItem.js";
import Notice from "./Home/Notice.js";

const mainMount = document.querySelector(".main-container main");

// set title
let _baseTitle = "圈";
let _subtitle = "";

function SetSubtitle(subtitle) {
    _subtitle = subtitle;
    document.title = `${_subtitle} - ${_baseTitle}`;
}
function SetBaseTitle(baseTitle) {
    _baseTitle = baseTitle;
    document.title = `${_subtitle} - ${_baseTitle}`;
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
        await SendPost.Init(services);
        await TagTree.Init(services);

    }).AddRoute("Home/Zone", async() => {
        SetSubtitle("动态");
        await InitView(services);
        await Zone.Init(services);
        await InitBase(services);

    }).AddRoute("Home/Notice", async() => {
        SetSubtitle("通知");
        await InitView(services);
        await InitBase(services);
        await Notice.Init(services);
    }).AddRoute("Match", async() => {
        SetSubtitle("匹配");
        await InitView(services);
        await InitBase(services);
        await Match.Init(services);
        MatchButton.Init(services);

    }).AddRoute("PrivateChat", async() => {
        SetSubtitle("私聊");
        await InitView(services);
        await InitBase(services);
        await PrivateChat.Init(services);

    }).AddRoute("SearchResult", async() => {
        SetSubtitle("搜索结果");
        await InitView(services);
        await InitBase(services);
        await SearchResult.Init(services);

    }).AddRoute("TagTree", async() => {
        SetSubtitle("标签树");
        await InitView(services);
        await InitBase(services);
        await TagTree.Init(services);

    }).AddRoute("UserPage", async() => {
        SetSubtitle("用户主页");
        await InitView(services);
        await InitBase(services);
        await UserPage.Init(services);

    }).AddRoute("Setting", async() => {
        SetSubtitle("设置");
        await InitView(services);
        await InitBase(services);
        Setting.Init(services);

    }).AddRoute("AccountInfo", async() => {
        SetSubtitle("账号信息");
        await InitView(services);
        await InitBase(services);
        await AccountInfo.Init(services);
        
    }).AddRoute("Password", async() => {
        SetSubtitle("Password");
        await InitView(services);
        await InitBase(services);
    }).AddRoute("PostItem", async() => {
        SetSubtitle("帖子页");
        await InitView(services);
        await InitBase(services);
        await PostItem.Init(services);
    }).Execute();
}

async function InitView(services) {
    const viewStr = await services.Api.Get(`/Home/${window.location.hash.replace("#", "")}`);
    mainMount.innerHTML = "";
    await ShowLoad(mainMount, "页面加载中...", fragment => {
        const div = document.createElement("div");
        div.innerHTML = viewStr;
        fragment.appendChild(div);
    });
}

async function InitBase(services){
    Header.Init(services);
    await Tag.Init(services);
    await PrivateChat.InitBase(services);

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

export {
    SetBaseTitle, SetSubtitle
}

export default {
    Init
}