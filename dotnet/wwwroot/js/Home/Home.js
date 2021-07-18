let mySelfTags = document.querySelector("#mySelfTags");

mySelfTags.addEventListener("dragstart", event => {
    PostChange(event,"/Home/RemoveTag","Self");
});

mySelfTags.addEventListener("drop", event => {
    PostChange(event,"/Home/AddTag","Self");
});



