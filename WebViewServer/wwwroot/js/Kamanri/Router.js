function Router() {
    this._eventHandlers = {}
    this.AddRoute = (url, Delegate) => {
        if(!url.startsWith("#")) {
            url = `#${url}`;
        }
        this._eventHandlers[url] = Delegate;
        return this;
    }
    this.Execute = () => {
        window.addEventListener("load", this._Delegate);
        window.addEventListener("hashchange", this._Delegate);
        this._Delegate();
    }
    this._Delegate = () => {
        let hash = window.location.hash.split("?")[0];
        this._eventHandlers[hash]();
    }
}

export {
    Router
}

export default {
    Router
}