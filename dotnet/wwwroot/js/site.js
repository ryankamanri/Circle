// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function ShowBody(event) {
    $("body").animate({
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



$(window).on("load",ShowBody);
$("nav").on("mouseover",ShowNav);
window.onmousewheel = event => ShowHeader(event);
