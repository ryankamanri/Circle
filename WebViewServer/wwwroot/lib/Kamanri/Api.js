import { AddScript } from "./Utils.js";

await AddScript(document.body, "/lib/jquery/dist/jquery.min.js");

function Api() {

	this.Get = url => {
		return new Promise((resolve, reject) => {
			$.ajax({
				url: url,
				type: "GET"

			}).done(data => {
				resolve(data);

			}).fail((action, state, event) => {
				console.error(action);
				console.error(state);
				console.error(event);
				reject(event);
			});
		});
		
	}

	this.Post = (url, reqData) => {
		return new Promise((resolve, reject) => {
			$.ajax({
				url: url,
				type: "POST",
				data: reqData

			}).done(data => {
				resolve(data);

			}).fail((action, state, event) => {
				console.error(action);
				console.error(state);
				console.error(event);
				console.warn(`When Post The Data ${JSON.stringify(reqData)}`);
				reject(event);
			});
		});
	}
}

export {
	Api
}