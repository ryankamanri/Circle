    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Kits
    function GetType(obj) {
        return Object.prototype.toString.call(obj)
    }
    function CopyElement(element) {
        let copyElement = document.createElement(element.tagName);
        copyElement.innerHTML = element.innerHTML;
        let attrCount = element.attributes.length;
        for(let i = 0; i < attrCount; i ++) {
            copyElement.setAttribute(element.attributes[i].name, element.attributes[i].value);
        }
        return copyElement;
    }
    function GenerateIDString() {
        return `${Date.now()}+${(Math.random() * 10000)}`
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function ModelList(modelArray) {
        // Type Check
        if (GetType(modelArray) != GetType(Array())) {
            console.error(`Expected ${GetType(Array())} As The Type Of Parameter 'modelArray' But Ordered ${GetType(modelArray)}`);
            return;
        }

        this._modelArray = modelArray;
        this.GetModelArray = () => this._modelArray;
        this._SetModelArray = value => this._modelArray = value;

        this.GetLength = () => this._modelArray.length;

        this._modelView = null;
        this.GetModelView = () => this._modelView;
        this._SetModelView = value => this._modelView = value;

        this.Append = (...items) => {
            let indexCount = this._modelArray.length;
            for(let item of items) {
                this._modelArray.push(item);
                if(this._modelView != undefined && this._modelView.GetModelList() == this) this._modelView.NotifyInsertedAt(indexCount, item);
                indexCount++;
            }
        }

        this.InsertAt = (index, ...items) => {
            let indexCount = index;
            for(let item of items) {
                this._modelArray.splice(indexCount, 0, item);
                if(this._modelView != undefined && this._modelView.GetModelList() == this) this._modelView.NotifyInsertedAt(indexCount, item);
                indexCount++;
            }
        }

        this.DeleteAt = (index, deleteCount) => {
            for(let indexCount = index; indexCount < index + deleteCount; indexCount++) {
                this._modelArray.splice(indexCount, 1);
                if(this._modelView != undefined && this._modelView.GetModelList() == this) this._modelView.NotifyDeletedAt(indexCount);
            }
        }

        this.Change = (index, ChangeDelegate) => {
            try {
                let model = this._modelArray[index];
                ChangeDelegate(model);
                if(this._modelView != undefined && this._modelView.GetModelList() == this) this._modelView.NotifyChangedAt(index);
            } catch (error) {
                console.error(`Failed To Execute Change Caused By : \n ${error.stack}`);
                console.warn(`Expected Delegate Function 'ChangeDelegage' Which Type : \n (model : object) => void`);
                return;
            }
        }

        this.Replace = (index, ReplaceDelegate) => {
            try {
                let model = this._modelArray[index];
                let replacedModel = ReplaceDelegate(model);
                if(this._modelView != undefined && this._modelView.GetModelList() == this) this._modelView.NotifyReplacedAt(index);
            } catch (error) {
                console.error(`Failed To Execute Replace Caused By : \n ${error.stack}`);
                console.warn(`Expected Delegate Function 'ReplaceDelegage' Which Type : \n (model : object) => replacedModel : object`);
                return;
            }
            
        }

        this.ForEach = ForEachElementDelegate => {
            this._modelArray.forEach((modelItem, index) => ForEachElementDelegate(modelItem, index));
        }

    }

    function ModelView(modelList, mountDivElement) {

        // Type Check
        if (GetType(modelList) != GetType(new ModelList([]))) {
            console.error(`Expected ${GetType(new ModelList([]))} As The Type Of Parameter 'modelList' But Ordered ${GetType(modelList)}`);
            return;
        }
        if (GetType(mountDivElement) != GetType(document.createElement('div'))) {
            console.error(`Expected ${GetType(document.createElement('div'))} As The Type Of Parameter 'mountDivElement' But Ordered ${GetType(mountDivElement)}`);
            return;
        }

        this._CheckTemplateIsValid = (viewTemplate) => {
            if (!('content' in viewTemplate) || !('childElementCount' in viewTemplate.content)) {
                console.error("Invalid Template : Property 'viewTemplate' May Not A Template");
                return false;
            }
            if (viewTemplate.content.childElementCount != 1) {
                console.error("Invalid Template : Template Can And Only Can Have 1 Root Element");
                return false;
            }
            return true;
        }

        // Double Way Binding
        this._modelList = modelList;
        this.GetModelList = () => this._modelList;
        this._SetModelList = value => this._modelList = value;
        this._modelList._SetModelView(this);

        this.RebindModelList = (bindModelList) => {
            if (GetType(bindModelList) != GetType(new ModelList([]))) {
                console.error(`Expected ${GetType(new ModelList([]))} As The Type Of Parameter 'modelList' But Ordered ${GetType(modelList)}`);
                return;
            }
            this._modelList = bindModelList;
            this._modelList._SetModelView(this);

        }

        this._mountElement = mountDivElement;
        this.GetMountElement = () => this._mountElement;
        this._SetMountElement = value => this._mountElement = value;

        // User Set Callback Functions Private
        this._ItemViewType = modelItem => 0; // (modelItem) -> viewType : int
        this._ItemTemplate = viewType => document.createElement("template"); // (viewType : int) -> Template
        this._TemplateViewToModelBinder = (view, modelItem, viewType) => { }; // (Template, modelItem, viewType) -> void
        this._ModelToTemplateViewBinder = (modelItem, view, viewType) => { };

        // User Set Callback Function Setters Public
        this.SetItemViewType = setter => {
            this._ItemViewType = setter;
            return this;

        }
        this.SetItemTemplate = setter => {
            this._ItemTemplate = setter;
            return this;

        }
        this.SetTemplateViewToModelBinder = setter => {
            this._TemplateViewToModelBinder = setter;
            return this;

        }
        this.SetModelToTemplateViewBinder = setter => {
            this._ModelToTemplateViewBinder = setter;
            return this;
        }

        this._GenerateAViewItem = (modelItem) => {
            let viewType;
            let template;
            let view;
            let viewItem;

            try {
                viewType = this._ItemViewType(modelItem);
                template = CopyElement(this._ItemTemplate(viewType));
                if (!this._CheckTemplateIsValid(template)) {
                    console.error("Invalid Template");
                    return;
                }
                view = template.content;
            } catch (error) {
                console.error(`Failed To Execute SetItemViewType Or SetItemTemplate Caused By : \n ${error.stack}`);
                console.warn(`Function 'SetItemViewType' Expected A Delegate Function Which Type : \n (modelItem : object) => viewType : int`);
                console.warn(`Function 'SetItemTemplate' Expected A Delegate Function Which Type : \n (viewType : int) => template : HTMLTemplateElement`);
                return;
            }
            
            // Set Item Key
            viewItem = view.firstElementChild;

            try {
                this._TemplateViewToModelBinder(viewItem, modelItem, viewType);
            } catch (error) {
                console.error(`Failed To Execute SetTemplateViewToModelBinder Caused By : \n ${error.stack}`);
                console.warn(`Expected A Delegate Function Which Type : \n (view : HTMLDivElement, modelItem : object, viewType : int) => void`);
                return;
            }

            
                
            viewItem.setAttribute("item_key", modelItem.itemKey);

            return viewItem;
        }

        this._CoverTemplateElement = (view) => {
            // let copyView = CopyElement(view);
            let template = document.createElement('template');
            template.content.appendChild(view);
            return template;
        }

        this.Show = () => {
            this._modelList.GetModelArray().forEach(modelItem => {

                let itemKey = GenerateIDString();
                modelItem.itemKey = itemKey;
                let viewItem = this._GenerateAViewItem(modelItem);
                
                //Append To MountElement
                this._mountElement.append(viewItem);

            });
        }

        this.Clean = () => {
            this._mountElement.innerHTML = "";
        }

        this.NotifyInsertedAt = (index, modelItem) => {

            if(this._mountElement.children[index] != undefined && 
                this._modelList.GetModelArray()[index].itemKey == this._mountElement.children[index].getAttribute("item_key")) return;
            let itemKey = GenerateIDString();
            modelItem.itemKey = itemKey;
            let viewItem = this._GenerateAViewItem(modelItem);
            if (this._mountElement.children.length == 0)
                this._mountElement.appendChild(viewItem);
            else this._mountElement.insertBefore(
                viewItem,
                this._mountElement.children[index]
            );
        }

        this.NotifyDeletedAt = (index) => {
            if(this._modelList.GetModelArray()[index] != undefined && 
                this._modelList.GetModelArray()[index].itemKey == this._mountElement.children[index].getAttribute("item_key")) return;
            this._mountElement.children[index].remove();
        }

        this.NotifyChangedAt = (index) => {
            let modelItem = this._modelList.GetModelArray()[index];
            let viewItem = this._mountElement.children[index];
            let viewType = this._ItemViewType(modelItem);
            this._TemplateViewToModelBinder(viewItem, modelItem, viewType);
        }

        this.NotifyReplacedAt = (index) => {
            let modelItem = this._modelList.GetModelArray()[index];
            let viewItem = this._GenerateAViewItem(modelItem);
            let viewType = this._ItemViewType(modelItem);
            this._TemplateViewToModelBinder(viewItem, modelItem, viewType);
            this._mountElement.children[index].remove();
            this._mountElement.insertBefore(
                viewItem, 
                this._mountElement.children[index]
            );
        }
        
    }


export default {
    ModelList, ModelView
}

export {
    ModelList, ModelView
}