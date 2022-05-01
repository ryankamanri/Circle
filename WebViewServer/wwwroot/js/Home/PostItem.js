import {Animate, ModelView} from "../My.js";
import Post from "../Shared/Components/Post/Post.js";
import Comment from "../Shared/Comment.js";

async function Init(services) {
    const postMount = document.querySelector(".post-mount");
    const form = window.location.hash.split("?")[1];
    const postForm = form.split("&")[0];
    const commentForm = form.split("&")[1];
    await InitPostView(services, postMount, postForm);
    await FocusComment(services, commentForm);
}

async function InitPostView(services, mountElement, form) {

    const formedPost = JSON.parse(await services.Api.Get(`/Home/PostItemModel?${form}`));
    const postModelList = new ModelView.ModelList([formedPost]);
    const postModelView = new ModelView.ModelView(postModelList, mountElement);

    await Post.Init(services);

    await postModelView.SetItemViewType(model => {

        return Post.SetItemViewType(model);
    }).SetItemTemplate(viewType => {

        return Post.SetItemTemplate(viewType);
    }).SetTemplateViewToModelBinder(async(view, model, viewType) => {

        await Post.SetTemplateViewToModelBinder(view, model, viewType);
        // await Post.ShowContent(view, model);
        const commentLabel = view.querySelector(".feed-item-bd .more>.comment");
        await Comment.Init(services, view.querySelector(".comment-mount"), commentLabel.getAttribute("postid"));
        await Comment.BuildAndShowModelView(commentLabel, true);
    }).Finally(async(view, model) => {
        await Post.ShowContent(view, model);
    }).ShowAsync();
    
    
}

async function FocusComment(services, commentForm) {
    if(commentForm === '') return;
    const key = commentForm.split("=")[0];
    if(key !== "CommentID") return;
    const value = commentForm.split("=")[1];
    const comment = document.querySelector(`div[comment-id='${value}']`);
    comment.focus();
    for(let i = 0; i < 2; i ++) {
        await Animate(comment, {opacity: 0}, "quick");
        await Animate(comment, {opacity: 1}, "quick");
    }
    
}

export default {
    Init
}