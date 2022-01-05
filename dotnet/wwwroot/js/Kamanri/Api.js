function Api() {
    
    this.Get = (url, CallBack) => {
        $.ajax({
            url: url,
            type: "GET"
            
        }).done(data => {
            CallBack(data);
            
        }).fail((action,state,event) =>{
            console.error(action);
            console.error(state);
            console.error(event);
        });
    }

    this.Post = (url, reqData, CallBack) => {
        $.ajax({
            url: url,
            type: "POST",
            data: reqData
            
        }).done(data => {
            CallBack(data);
            
        }).fail((action,state,event) =>{
            console.error(action);
            console.error(state);
            console.error(event);
        });
    }
}

export {
    Api
}