import Header from './Shared/Header.js';
import Tag from './Shared/Components/Tag.js';
import Post from './Shared/Components/Post/Post.js';
import Sidebar from './Shared/Sidebar.js';
import MyInterestedTags from './Shared/MyInterestedTags.js';
import Routes from './Routes.js';
import { Api, Configuration, MyWebSocket, AddScript } from './My.js';


await AddScript(document.body, "/lib/bootstrap/dist/js/bootstrap.min.js");
await AddScript(document.body, "/lib/ckeditor5-build-classic/ckeditor.js");

const wsuri = await Configuration("WebSocketUri");

async function Init()
{
	const services = await InitServices();

	InitLayout(services);
	await Routes.Init(services);
	services.MyWebSocket.Open();
}

 async function InitLayout(services) {
	await MyInterestedTags.Init(services);
	Header.Init(services);
	await Tag.Init(services);
	await Post.Init(services);
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
