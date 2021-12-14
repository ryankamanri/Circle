import {parseFunc} from '../My.js'
import MainVue from '../MainVue.js';
let vue;
function SignIn()
{
    vue = MainVue.MainVue();
    let ShowMessage = vue.$data.store.Function_ShowMessage;
    $("#getauthcode").on("click",() => 
    {
        let account = $("#account").val();
        $.ajax({
            url : "/GetAuthCode",
            type : "POST",
            data : {
                User : [
                    account
                ]
            }
        }).done(data => {
            console.log(data);
            if(data == "验证码发送成功")
                ShowMessage("alert alert-success","😀",data);
            else ShowMessage("alert alert-warning","😥",data);
        });
    });

    $("#signin").on("click",() => {
        let account = $("#account").val();
        let password = $("#password").val();
        let authCode = $("#authcode").val();
        $.ajax({
            url : "/SignInSubmit",
            type : "POST",
            data : {
                User : [
                    account,
                    password,
                    authCode
                ]
            }
        }).done(async (data) => {
            if(data == "注册成功") 
            {
                await ShowMessage("alert alert-success","😀",data);
                window.location.href = "/SelectCircle";
            }
            ShowMessage("alert alert-warning","😥",data);
        }).fail(() =>
            //alert("send failure");
            ShowMessage("alert alert-danger","😭","登录失败,请联系管理员")
        );
    });
}
export default{
    SignIn
}