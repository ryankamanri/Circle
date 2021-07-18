//$(window).on("load",ShowBody);
//$("nav").on("mouseover",ShowNav);
//window.onmousewheel = event => ShowHeader(event);

let showLoad = document.getElementById("showLoad");
let body = document.getElementById("body");
body.style.display = "none";
body.style.opacity = 0;

document.addEventListener("readystatechange", function (event) {
    if (document.readyState == "complete") {
        showLoad.style.display = "none";
        body.style.display = "block";
        ShowBody();
    }
});


function ShowBody() {
    $("#body").animate({
        opacity: 1
    });

    console.log("onload");
}

function ShowHeader(event) {
    if (event.deltaY > 0) {

        $("header").animate({
            opacity: 0
        });
    } else if (event.deltaY < 0) {

        $("header").animate({
            opacity: 1
        });
    }
}

function ShowNav()
{
    $("header").animate({
        opacity: 1
    });
}
