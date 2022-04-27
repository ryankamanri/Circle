function Router() {
    this._eventHandlers = {}
    this.AddRoute = (url, Delegate) => {
        if(!url.startsWith("#")) {
            url = `#${url}`;
        }
        this._eventHandlers[url] = Delegate;
        return this;
    }
    this.Execute = async() => {
        window.addEventListener("load", this._Delegate);
        window.addEventListener("hashchange", this._Delegate);
        await this._Delegate();
    }
    this._Delegate = async() => {
        let hash = window.location.hash.split("?")[0];
        if(this._eventHandlers[hash] === undefined) {
            console.error(`The Route Hash "${hash}" Does NOT Match Any Handler`);
            console.warn("Please Check Your Url Is Correct," +
                " Or Use \n\n" +
                "\tRouter.AddRoute(url: string, Delegate: () => {})" +
                "\n\n To Add A New Route");
            return;
        }
        await this._eventHandlers[hash]();
    }
}

export {
    Router
}

export default {
    Router
}