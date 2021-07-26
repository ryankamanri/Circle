import { Mutex } from '/js/My.js'

function Tag()
{
    
    FlushDrugEvent();
    FlushDropEvent();
}

function FlushDrugEvent() {
    let tags = document.querySelectorAll(".tag");
    tags.forEach(tag => {
        tag.ondragstart = event => {
            event.dataTransfer.setData("id", event.target.id);
        }
    });
}

function FlushDropEvent() {
    let tagboxes = document.querySelectorAll(".tagbox");
    let tagNodes = document.querySelectorAll(".tagNode,.ceiledTagNode");
    let id;
    let moveTag, originTag;
    let mutex = new Mutex();
    tagboxes.forEach(tagbox => {
        tagbox.ondragover = event => event.preventDefault();
        tagbox.ondrop = event => {
            id = event.dataTransfer.getData("id");
            tagbox.appendChild(document.getElementById(id));
            mutex.Signal();
        };
        tagNodes.forEach(tagNode => {
            tagNode.ondragstart = async event => {
                event.stopPropagation();
                id = event.dataTransfer.getData("id");
                moveTag = document.getElementById(id);
                originTag = moveTag.cloneNode();
                originTag.innerHTML = moveTag.innerHTML;
                originTag.id++;
                mutex.mutex = true;
                await mutex.Wait();
                //tagNode.appendChild(originTag);
                //tagNode.children[0].insertBefore(originTag,this);
                tagNode.insertBefore(originTag,tagNode.children[0]);
                originTag.addEventListener("dragstart", event => {
                    event.dataTransfer.setData("id", event.target.id);
                });


            };
        });
    });
}


export default{
    Tag,FlushDrugEvent,FlushDropEvent
}



