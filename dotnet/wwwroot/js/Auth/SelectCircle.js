$("#ExamCircle").on("click",() => {
    $.ajax({
        url : "/SelectCircleSubmit",
        type : "POST",
        data : {
            Circle : "考研圈"
        }
    }).done(data =>
        {
            console.log(data);
            window.location.href = "/Home";
        } );
});

$("#EmploymentCircle").on("click",() => {
    $.ajax({
        url : "/SelectCircleSubmit",
        type : "POST",
        data : {
            Circle : "就业圈"
        }
    }).done(data =>
        {
            console.log(data);
            window.location.href = "/Home";
        } );
});