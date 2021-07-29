import {parseFunc} from '../My.js'
import MainVue from '../MainVue.js';
let vue;
function SignIn()
{
    vue = MainVue.MainVue();

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
                parseFunc(vue.$data.store.func.ShowMessage)("alert alert-success","😀",data);
            else parseFunc(vue.$data.store.func.ShowMessage)("alert alert-warning","😥",data);
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
                await parseFunc(vue.$data.store.func.ShowMessage)("alert alert-success","😀",data);
                window.location.href = "/Home";
            }
            parseFunc(vue.$data.store.func.ShowMessage)("alert alert-warning","😥",data);
        }).fail(() =>
            //alert("send failure");
            parseFunc(vue.$data.store.func.ShowMessage)("alert alert-danger","😭","登录失败,请联系管理员")
        );
    });
}
export default{
    SignIn
}