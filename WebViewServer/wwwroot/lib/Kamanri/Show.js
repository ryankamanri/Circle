import { Animate, Sleep } from './Utils.js';

function Show() 
{
	
}

async function ShowLoad(mountElement, loadText, LoadedCallback) {


	let showLoadWindow = `
		<div class="show-load-window" >
            <div class="show-load" >
                <h5 class="show-load-text">Loading...</h5>
                <div class="progress show-load-progress">
                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"
                         style="width:100%"></div>
                </div>
            </div>
        </div>`;
	let childElementFragment = document.createDocumentFragment();
	childElementFragment.innerHTML = mountElement.innerHTML;
	mountElement.innerHTML = showLoadWindow;
	let showLoadTextElement = mountElement.querySelector(".show-load-window .show-load-text");
	showLoadTextElement.innerText = loadText;
	await Animate(mountElement, { opacity: 1 });
	await LoadedCallback(childElementFragment);
	await Animate(mountElement, { opacity: 0 });
	mountElement.innerHTML = childElementFragment.innerHTML;
	await Animate(mountElement, { opacity: 1 });
}

async function ShowAlert(bootstrapClass, emphasis, message, alertTime=1500)
{
	let messageElem = document.querySelector("body>#alert");
	if (messageElem == null ) {
		messageElem = document.createElement("div");
		messageElem.id = "alert";
		let style = messageElem.style;
		style.opacity = 0;
		style.position = "fixed";
		style.top = "10%";
		style.left = "50%";
		style.zIndex = 100;
		style.setProperty("transform", "translate(-50%, -50%)");

		document.querySelector("body").append(messageElem);
	}
	
	messageElem.className = bootstrapClass;
	messageElem.innerHTML = `<strong>${emphasis}</strong>${message}`;

	Animate(messageElem, {
		opacity : 1
	});

	await Sleep(alertTime);

	Animate(messageElem, {
		opacity : 0
	});
}

function ShowInput(textPlaceHolder = "���ۻ�ظ��û�...", buttonPlaceHolder = "����", delegateObject, CallBack) {
	let inputElem = document.querySelector("body>#input");
	if (inputElem == null) {
		inputElem = document.createElement("div");
		inputElem.id = "input";
		let style = inputElem.style;
		style.width = "50%";
		style.position = "fixed";
		style.left = "50%";
		style.background = "whitesmoke";
		style.transform = "translate(-50%, 0)";
		style.borderRadius = "10px";
		style.opacity = 0.9;
		style.boxShadow = "0px 0px 10px 0px";
		style.minWidth = "350px";
		inputElem.innerHTML = `
				<div id="close" class="btn btn-danger" style="
					width: 30px;
					height: 30px;
					float: right;
					cursor: pointer;
					text-align: center;
				">X</div>
				<form>
					<div class="input-group" style="padding: 3rem;">

						<div style="position: relative; width: 80%;">
							<input class="form-control" type="text" placeholder="${textPlaceHolder}">
						
						</div>

						<input type="button" id="send" class="btn btn-outline-info my-2 my-sm-0" value="${buttonPlaceHolder}">
						
					</div>
				</form>`;
		document.querySelector("body").append(inputElem);
	} else {
		inputElem.querySelector("input.form-control").setAttribute("placeholder", textPlaceHolder);
		inputElem.querySelector("#send").setAttribute("value", buttonPlaceHolder);
	}

	inputElem.querySelector("#close").onclick = () => HideInput();
	inputElem.querySelector("#send").onclick = () => {
		let text = inputElem.querySelector("input").value;
		CallBack(text, delegateObject);
		HideInput();
		inputElem.querySelector("input").value = "";
	};

	Animate(inputElem, {
		bottom: "-30%"
	}, "fast").then(Animate(inputElem, {
		bottom: "10%"
	}));
	
}

function HideInput() {
	let inputElem = document.querySelector("body>#input");
	inputElem.querySelector("#close").onclick = event => HideInput();
	Animate(inputElem, {
		bottom: "-30%"
	});
}
export {
	Show, ShowAlert, ShowLoad, ShowInput
}

export default{
	Show
}


