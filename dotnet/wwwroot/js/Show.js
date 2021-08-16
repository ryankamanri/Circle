

function Show() 
{
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

    function ShowNav() {
        $("header").animate({
            opacity: 1
        });
    }
}


async function ShowMessage(bootstrapClass,emphasis,message)
{
    let messageElem = document.querySelector("body>.alert");
    if (messageElem == null ) {
        messageElem = document.createElement("div");
        
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
    
    $(messageElem).animate({
        opacity : 1
    });

    await new Promise(resolve => {
        setTimeout(() => resolve(), 2000);
    });

    $(messageElem).animate({
        opacity : 0
    });
}
export{
    Show,ShowMessage
}

export default{
    Show
}


