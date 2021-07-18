function FlushDrugEvent() {
    let tags = document.querySelectorAll(".tag");
    tags.forEach(tag => {
        tag.addEventListener("dragstart", event => {
            event.dataTransfer.setData("id", event.target.id);
        });
    });
}

function FlushDropEvent() {
    let tagboxes = document.querySelectorAll(".tagbox");
    let tagNodes = document.querySelectorAll(".tagNode,.ceiledTagNode");
    let id;
    let moveTag, originTag;
    let originNode;
    let mutex = new Mutex();
    tagboxes.forEach(tagbox => {
        tagbox.addEventListener("dragover", event => event.preventDefault());
        tagbox.addEventListener("drop", event => {
            id = event.dataTransfer.getData("id");
            tagbox.appendChild(document.getElementById(id));
            mutex.Signal();
        });
    tagNodes.forEach(tagNode => {
        tagNode.addEventListener("dragstart", async event => {
            
            id = event.dataTransfer.getData("id");
            moveTag = document.getElementById(id);
            originTag = moveTag.cloneNode();
            originTag.innerHTML = moveTag.innerHTML;
            originTag.id++;
            mutex.mutex = true;
            await mutex.Wait();
            //tagNode.innerHTML = tagNode.innerHTML.replace(/<div class="tagNode">\s*<\/div>/g,`<div class="tagNode">${stringlify(originTag)}</div>`);
            tagNode.appendChild(originTag);
            originTag.addEventListener("dragstart", event => {
                event.dataTransfer.setData("id", event.target.id);
            });
        });
    });
    });
}

FlushDrugEvent();
FlushDropEvent();

