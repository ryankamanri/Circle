using System.Collections.Generic;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using WebViewServer.Models;
using WebViewServer.Models.Post;
using WebViewServer.Models.User;

namespace WebViewServer.Services
{
    class ViewService
    {
        private readonly PostService _postService;
        private readonly UserService _userService;

        private readonly User _user;
        public ViewService(PostService postService, UserService userService, User user)
        {
            _postService = postService;
            _userService = userService;
            _user = user;
        }
        public async Task<Form> GetPostViewModel(Post post)
		{
            UserInfo authorInfo = await _postService.SelectAuthorInfo(post);
            ICollection<Tag> tags = await _postService.SelectTags(post);

            bool isFocus = await _userService.IsUserRelationExist(_user, new User(authorInfo.ID), "Type", "Focus");
            var likeCollectCommentCount = await _postService.SelectLikeCollectCommentCount(post);
            var isLikeOrCollect = await _postService.SelectIsLikeOrCollect(post, _user);

			return new Form()
			{
				{"ID", post.ID},
				{"AuthorNickName", authorInfo.NickName},
				{"AuthorHeadImage", authorInfo.HeadImage},
				{"AuthorID", authorInfo.ID},
				{"IsFocus", isFocus.ToString()},
				{"Title", post.Title},
				{"Focus", post.Focus},
				{"Summary", post.Summary},
				{"Tags", tags.ToJson()},
				{"IsLike", isLikeOrCollect["IsLike"]},
				{"LikeCount", likeCollectCommentCount["LikeCount"]},
				{"IsCollect", isLikeOrCollect["IsCollect"]},
				{"CollectCount", likeCollectCommentCount["CollectCount"]},
				{"CommentCount", likeCollectCommentCount["CommentCount"]}
			};
		}
    }
}