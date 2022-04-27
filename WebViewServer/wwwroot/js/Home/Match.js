import {ShowAlert, ModelView, Sleep, AddScript} from '../My.js';
import SearchUserInfo from "../Shared/Components/SearchUserInfo.js";

await AddScript(document.body, "/lib/matchButton/three.min.js");
await AddScript(document.body, "/lib/matchButton/gsap.min.js");




async function InitUserInfoView(services) {
	const mountElement = document.querySelector(".match-user");
    const formedPosts = `[{
        "HeadImage": "/StaticFiles/Images/HeadImage/syt5.com.1638601045.jpg",
        "NickName": "带感",
        "SchoolYear": "2019-01-01T00:00:00",
        "Speciality": "计算机科学与技术",
        "Tags": [{
            "_Tag": "电路原理",
            "ID": 9
        }, {
            "_Tag": "模拟电子技术",
            "ID": 10
        }, {
            "_Tag": "汇编语言",
            "ID": 13
        }],
        "Similarity": 0.7115074316232417,
        "Interesty": 0.4988876515698589,
        "ID": 9,
        "IsFocus": true
    }, {
        "HeadImage": "/StaticFiles/Images/HeadImage/syt5.com.1638601084.jpg",
        "NickName": "兄贵",
        "SchoolYear": "2019-01-01T00:00:00",
        "Speciality": "Computer Science & Technology",
        "Tags": [{
            "_Tag": "哲学",
            "ID": 4
        }, {
            "_Tag": "电路原理",
            "ID": 9
        }, {
            "_Tag": "人工智能",
            "ID": 11
        }],
        "Similarity": 0.7115074316232417,
        "Interesty": 0.4988876515698589,
        "ID": 3,
        "IsFocus": true
    }, {
        "HeadImage": "/StaticFiles/Images/HeadImage/syt5.com.1638601120.jpg",
        "NickName": "了无痕",
        "SchoolYear": "2019-01-01T00:00:00",
        "Speciality": "Computer Science & Technology",
        "Tags": [{
            "_Tag": "软件工程",
            "ID": 7
        }, {
            "_Tag": "数据库",
            "ID": 16
        }, {
            "_Tag": "Java",
            "ID": 35
        }],
        "Similarity": 0.7678538984566009,
        "Interesty": 0.7453559924999299,
        "ID": 4,
        "IsFocus": true
    }, {
        "HeadImage": "/StaticFiles/Images/HeadImage/syt5.com.1638601145.jpg",
        "NickName": "853909407",
        "SchoolYear": "2019-01-01T00:00:00",
        "Speciality": "Computer Science & Technology",
        "Tags": [{
            "_Tag": "第一个标签",
            "ID": 1
        }, {
            "_Tag": "软件工程",
            "ID": 7
        }],
        "Similarity": 0.637036868809194,
        "Interesty": 0.8660254037844386,
        "ID": 5,
        "IsFocus": true
    }, {
        "HeadImage": "/StaticFiles/Images/HeadImage/21E71DB5133C5A2C5257FFA8FDC510BB.jpg",
        "NickName": "Lucas",
        "SchoolYear": "2019-01-01T00:00:00",
        "Speciality": "Computer Science & Technology",
        "Tags": [{
            "_Tag": "计算机科学技术",
            "ID": 5
        }],
        "Similarity": 0.48412291827592707,
        "Interesty": 0.4841229182759271,
        "ID": 15,
        "IsFocus": true
    }]`
	const formedUserInfos = JSON.parse(formedPosts);
	const userModelList = new ModelView.ModelList(formedUserInfos);
	const userModelView = new ModelView.ModelView(userModelList, mountElement);

	await SearchUserInfo.Init(services);

	await userModelView.SetItemViewType(model => {

		return SearchUserInfo.SetItemViewType(model);
	}).SetItemTemplate(viewType => {

		return SearchUserInfo.SetItemTemplate(viewType);
	}).SetTemplateViewToModelBinder((view, model, viewType) => {

		SearchUserInfo.SetTemplateViewToModelBinder(view, model, viewType,view =>{
            // MatchKeyWords(view);
        });

	}).ShowAsync();

}

async function Init(services){
    await InitUserInfoView(services);
}
export default{
    Init
}