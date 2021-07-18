

let myInterestedTags = document.querySelector("#myInterestedTags");


myInterestedTags.addEventListener("dragstart", event => {
    PostChange(event,"/Home/RemoveTag","Interested");
});

myInterestedTags.addEventListener("drop", event => {
    PostChange(event,"/Home/AddTag","Interested");
});




function PostChange(event,url,relation)
{
    let DragTagCurrentID = event.dataTransfer.getData("id");
    let DragTag = document.getElementById(DragTagCurrentID);
    let DragTagID = DragTag.childNodes[1].innerText;
    $.ajax({
        url : url,
        type : "POST",
        data : {
            tagID : DragTagID,
            tagRelation : relation
        }
    }).done(resData => {
        console.log(resData);
    });
}