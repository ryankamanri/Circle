import { Site, ShowAlert } from '../My.js';

let btn,title,focus,tagIDs,summary,contentDocument;
let tagCollection;
let _services;

async function Init(services)
{
	_services = services;

	await AddCKEditor();

	btn = document.querySelector("input#submit");
	
	btn.onclick = () => SendPostSubmit();
}

function SendPostSubmit()
{
	tagIDs = [];
	title = document.querySelector("#title").value;
	focus = document.querySelector("#focus").value;
	
	contentDocument = document.querySelector(".ck-editor__main .ck-content");

	tagCollection = document.querySelector("#post-tags").querySelectorAll(".ID");
	tagCollection.forEach(tagID => {
		tagIDs.push(tagID.innerText);
	})
	Site.Post("/Home/SendPostSubmit",{
		Title : title,
		Focus : focus,
		Summary : contentDocument.innerText.substring(0,100),
		Content : contentDocument.innerHTML,
		TagIDs : JSON.stringify(tagIDs)
	},async resData => {
		console.log(resData);
		if(resData == "add succeed")
		{
			await ShowAlert("alert alert-success","😀","发布成功");
			window.location.href = "/Home#Home/Posts";
		} 
	},()=>{});
}

async function AddCKEditor() {

	ClassicEditor
		.create(document.querySelector('.ckeditor'), {
			language: 'zh',
			mediaEmbed: {
				providers: [{
					name: 'myProvider',
					url: [
						/^.*/
					],
					html: match => {
						const input = match['input'];
						return(
							'<div style="position: relative; padding-bottom: 100%; height: 0; padding-bottom: 70%;">' +
							`<iframe src="${input}" ` +
							'style="position: absolute; width: 100%; height: 100%; top: 0; left: 0;" ' +
							'frameborder="0" allowtransparency="true" allow="encrypted-media">' +
							'</iframe>' +
							'</div>'
						)
					}
				}]
			}
		})
		.then(editor => {
			// 加载了适配器
			editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
				return new UploadAdapter(loader);
			};
			window.editor = editor;
		})
		.catch(error => {
			console.error(error);
		});

}

//重点代码 适配器
class UploadAdapter {
	constructor(loader) {
		this.loader = loader;
	}
	 upload() {
		return new Promise((resolve, reject) => {
		const data = new FormData();
					let file = [];
					//this.loader.file 这是一个Promise格式的本地文件流，一定要通过.then 进行获取，之前在各大博客查了很多文章都拿不到这个值，最后经过两个多小时的探索终于找到了是Promise问题。
					this.loader.file.then(res=>{
						file = res; //文件流 
						data.append('ImageFile', file); //传递给后端的参数，参数名按照后端需求进行填写
						$.ajax({
							url: '/Upload/PostImage', //后端的上传接口 
							type: 'POST',
							data: data,
							dataType: 'json',
							processData: false,
							contentType: false,
							success: resData => {
								console.log(resData);
								if(resData.Status === "Success"){
									const path = resData.Info;
									const url = `${window.location.origin}${path}`;
									resolve({
										default: url
									});
								}
								else reject (resData.Info);
								
							}
						});
					})
		});
	}
	abort() {
	}
}



export {
	Init
}

export default{
	Init
}