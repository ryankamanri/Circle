import { Mutex , Sleep} from '/js/My.js'

let mutex = new Mutex();
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
    tagboxes.forEach(tagbox => {
        tagbox.ondragover = event => event.preventDefault();
        tagbox.ondrop = event => {
            id = event.dataTransfer.getData("id");
            let tag = document.getElementById(id);
            if (tag != null) tagbox.appendChild(tag);
            else tagbox.appendChild(document.querySelector("iframe.tag-tree").contentDocument.getElementById(id));

        };
        
    });

    tagNodes.forEach(tagNode => {
        tagNode.ondragstart = async event => {
            
            event.stopPropagation();
            id = event.dataTransfer.getData("id");
            moveTag = document.getElementById(id);
            originTag = moveTag.cloneNode();
            originTag.innerHTML = moveTag.innerHTML;
            originTag.id++;
            mutex.mutex = true;
            await Sleep(1000);
            if(tagNode.children[0] == undefined)//标签已被取走
                tagNode.insertBefore(originTag,tagNode.children[0]);
            originTag.addEventListener("dragstart", event => {
                event.dataTransfer.setData("id", event.target.id);
            });


        };
    });
}


export default{
    Tag,FlushDrugEvent,FlushDropEvent
}



