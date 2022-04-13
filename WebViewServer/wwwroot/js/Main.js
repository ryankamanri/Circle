import Header from './Shared/Header.js';
import Tag from './Shared/Tag.js';
import Post from './Shared/Post.js';
import Sidebar from './Shared/Sidebar.js';
import MyInterestedTags from './Shared/MyInterestedTags.js';
import Routes from './Shared/Routes.js';



function Init()
{
	MyInterestedTags.Init();
	Header.Init();
	Tag.Init();
	Post.Init();
	Sidebar.Init();
	Routes.Init();
}


export{
	Init
}
export default{
	Init
}
