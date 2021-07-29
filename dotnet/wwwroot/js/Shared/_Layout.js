import Header from './Header.js'
import Tag from './Tag.js'
import Post from './Post.js'
import MainVue from '../MainVue.js'



let mySelfTags;
let myInterestedTags;
let vue;


function _Layout()
{
    vue = MainVue.MainVue({
        el : "main"
    });

    mySelfTags = document.querySelector("#mySelfTags");
    myInterestedTags = document.querySelector("#myInterestedTags");

    mySelfTags.addEventListener("dragstart",event => {
        PostChange(event,"/Shared/RemoveRelation","Tag","Type","Self");
    });
    mySelfTags.addEventListener("drop", event => {
        PostChange(event,"/Shared/AppendRelation","Tag","Type","Self");
    });

    myInterestedTags.addEventListener("dragstart", event => {
        PostChange(event,"/Shared/RemoveRelation","Tag","Type","Interested");
    });
    
    myInterestedTags.addEventListener("drop", event => {
        PostChange(event,"/Shared/AppendRelation","Tag","Type","Interested");
    });

    // let drake = dragula(document.querySelectorAll(".tagbox,.tagNode"));
    
    Header.Header(vue);
    Tag.Tag(vue);
    Post.Post(vue);
}

function PostChange(event, url, entityType, relationName,relation) 
{
    let DragTagCurrentID = event.dataTransfer.getData("id");
    let DragTag = document.getElementById(DragTagCurrentID);
    let DragTagID = DragTag.children[0].innerText;

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
