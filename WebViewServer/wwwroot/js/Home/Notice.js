import {ShowLoad} from "../../lib/Kamanri/Show.js";
import {ModelView} from "../My.js";
import Post from "../Shared/Components/Post/Post.js";
import FirstLevelComment from "../Shared/Components/Post/FirstLevelComment.js";
import SecondLevelComment from "../Shared/Components/Post/SecondLevelComment.js";
import {ShowInputWindow} from "../Shared/Comment.js";


const ModelType = {
    Post: 1,
    PostTime: 2,
    FirstLevelComment: 3,
    SecondLevelComment: 4
}

async function Init(services) {
    const mountElement = document.querySelector(".post-mount");
    await ShowLoad(document.querySelector(".show-fragment"), "看看有什么消息呢...", async(fragment) => {
        await InitView(services, mountElement);
    });

}

async function InitView(services, mountElement) {
    const formedPosts = JSON.parse(await services.Api.Post("/Home/Home/NoticeModel"));
    const modelList = new ModelView.ModelList(formedPosts);
    const modelView = new ModelView.ModelView(modelList, mountElement);

    await Post.Init(services);

    await modelView.SetItemViewType(model => {

        if (model.Key.CommentID === -1)
            return ModelType.FirstLevelComment;
        return ModelType.SecondLevelComment;
        
    }).SetItemTemplate(viewType => {
        switch (viewType) {
            case ModelType.Post:
            case ModelType.PostTime:
                return Post.SetItemTemplate(viewType);
            case ModelType.FirstLevelComment:
                return FirstLevelComment.SetItemTemplate(FirstLevelComment.ViewType.SingleItem);
            case ModelType.SecondLevelComment:
                return SecondLevelComment.SetItemTemplate(SecondLevelComment.ViewType.SingleItem);
        }
    }).SetTemplateViewToModelBinder((view, model, viewType) => {
        let commentLabel = view.querySelector(".comment");
        switch (viewType) {
            case ModelType.Post:
            case ModelType.PostTime:
                Post.SetTemplateViewToModelBinder(view, model, viewType);
                break;
            case ModelType.FirstLevelComment:
                FirstLevelComment.SetTemplateViewToModelBinder(view, model, viewType);
                commentLabel.addEventListener("click", event => {
                    ShowInputWindow(commentLabel);
                });
                break;
            case ModelType.SecondLevelComment:
                SecondLevelComment.SetTemplateViewToModelBinder(view, model, viewType);
                commentLabel.addEventListener("click", event => {
                    ShowInputWindow(commentLabel);
                });
                break;
        }

    }).ShowAsync();
}

export default {
    Init
}