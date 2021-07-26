import Header from './Header.js'
import Tag from './Tag.js'
import Post from './Post.js'



let myInterestedTags;


function _Layout()
{
    myInterestedTags = document.querySelector("#myInterestedTags");


    myInterestedTags.addEventListener("dragstart", event => {
        PostChange(event,"/Shared/RemoveRelation","Tag","Type","Interested");
    });
    
    myInterestedTags.addEventListener("drop", event => {
        PostChange(event,"/Shared/AppendRelation","Tag","Type","Interested");
    });

    let drake = dragula(document.querySelectorAll(".tagbox,.tagNode"));
    
    Header.Header();
    Tag.Tag();
    Post.Post();
}

function PostChange(event, url, entityType, relationName,relation) 
{
    let DragTagCurrentID = event.dataTransfer.getData("id");
    let DragTag = document.getElementById(DragTagCurrentID);
    let DragTagID = DragTag.childNodes[1].innerText;

    PostChangeRelation(url,DragTagID,entityType,relationName,relation,resData =>{
        console.log(resData);
    },(action,state,event) => {});
}

function PostChangeRelation(url,ID,entityType,relationName,relation,CallBack,FailHandler)
{
    $.ajax({
        url: url,
        type: "POST",
        data: {
            ID: ID,
            entityType: entityType,
            relationName: relationName,
            relation: relation
        }
    }).done(resData => {
        CallBack(resData);
    }).fail((action,state,event) =>{
        console.log(action);
        console.log(state);
        console.log(event);
        FailHandler(action,state,event);
    });
}
export{
    _Layout,PostChange,PostChangeRelation
}
export default{
    _Layout,PostChange,PostChangeRelation
}
