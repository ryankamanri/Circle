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
    tagboxes.forEach(tagbox => {
        tagbox.addEventListener("dragover", event => event.preventDefault());
        tagbox.addEventListener("drop", event => {
            let id = event.dataTransfer.getData("id");
            tagbox.appendChild(document.getElementById(id));
        });
    });
}

FlushDrugEvent();
FlushDropEvent();

