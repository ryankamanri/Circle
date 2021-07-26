

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
        }).done((data) => {
            if(data == "login succeed") window.location.href = "/SelectCircle";
            alert("send succeed\n" + data);
        }).fail(() =>
            alert("send failure")
        )
    });
    return this;
}

export default{
    LogIn
}

