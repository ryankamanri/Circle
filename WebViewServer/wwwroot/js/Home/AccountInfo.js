import {ShowAlert} from "../My.js";


async function Init(services) {
    const submitButton = document.querySelector("form.account-info .submit");
    submitButton.onclick = async() => {
        const form = document.querySelector("form.account-info");
        const formData = new FormData(form);
        const response = await services.Api.Post("/Home/AccountInfoSubmit", formData, false);
        console.log(response);
        await ShowAlert("alert alert-success", "😀", "修改成功");
        window.location.reload();
    }
    
}
export default {
    Init
}