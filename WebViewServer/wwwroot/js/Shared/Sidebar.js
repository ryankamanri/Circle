function Init(services) {
    let body = document.getElementsByTagName("body")[0];
    let aside = document.getElementsByTagName("aside")[0];
    let mainContainer = document.querySelector(".main-container");
    let btn = aside.querySelector(".btn");

    btn.onclick = () => {
        if (aside.clientWidth <= 40) {
            body.className = "";
            mainContainer.classList.add("left-moved");
        } else if (aside.clientWidth > 40) {
            console.log("side")
            body.className = "minibody";
            mainContainer.classList.remove("left-moved");
        }

    }
}

export default {
    Init
}