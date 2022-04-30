function Init(services) {
    const logoutBtn = document.querySelector(".logout");
    logoutBtn.onclick = async() => {
        const data = await services.Api.Post("/LogOutSubmit");
        if (data === "logout succeed")
            window.location.href = "/";
    }
    
}

export default {
    Init
}