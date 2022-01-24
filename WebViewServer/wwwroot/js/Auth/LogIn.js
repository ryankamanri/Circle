
import { ShowMessage } from '../Show.js';

function LogIn()
{
    $("#login").on("click",() => {
        let account = $("#account").val();
        let password = $("#password").val();
        $.ajax({
            url : "/LogInSubmit",
            type : "POST",
            data : {
                User : [
                    account,
                    password
                ]
            }
        }).done(async data => {
            if(data == "登录成功") 
            {   
                await ShowMessage("alert alert-success","😀",data);
                window.location.href = "/SelectCircle";
            }
            else{
                ShowMessage("alert alert-warning","😥",data)
            }
            
        }).fail(() =>{
            ShowMessage("alert alert-danger","😭","登录失败,请联系管理员");
        });
    });
    return this;
}

export default{
    LogIn
}

