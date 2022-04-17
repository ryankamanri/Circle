import Header from './Shared/Header.js';
import Tag from './Shared/Tag.js';
import Post from './Shared/Post.js';
import Sidebar from './Shared/Sidebar.js';
import MyInterestedTags from './Shared/MyInterestedTags.js';
import Routes from './Routes.js';
import { Api, Configuration, MyWebSocket } from './My.js';

const wsuri = await Configuration("WebSocketUri");

async function Init()
{
	const services = await InitServices();

	InitLayout(services);

	await Routes.Init(services);
	services.MyWebSocket.Open();
}

function InitLayout(services) {
	MyInterestedTags.Init(services);
	Header.Init(services);
	Tag.Init(services);
	Post.Init(services);
	Sidebar.Init(services);
}

async function InitServices() {
	const api = new Api();
	const userInfoStr = await api.Post("/Api/User/GetSelfInfo", {});
	return {
		Api: api,
		MyWebSocket: new MyWebSocket.MyWebSocket(wsuri),
		UserInfo: JSON.parse(userInfoStr)
	}
}

export{
	Init
}
export default{
	Init
}
