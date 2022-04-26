using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Kamanri.Extensions;
using Kamanri.Http;
using WebViewServer.Models;
using WebViewServer.Models.Post;
using WebViewServer.Models.User;

namespace WebViewServer.Services
{
    public class ViewModelService
    {
        private readonly PostService _postService;
        private readonly UserService _userService;

        private readonly User _user;
        public ViewModelService(PostService postService, UserService userService, User user)
        {
            _postService = postService;
            _userService = userService;
            _user = user;
        }

        public async IAsyncEnumerable<Task<Form>> GetHomePostsViewModels()
        {
            var myInterestedPosts = await _userService.MappingPostsByTag(_user, selection => selection.Type = new List<string>() { "Interested" });
            var allPosts = await _postService.GetAllPost();
            var myPosts = myInterestedPosts.Union(allPosts, new Post());
			var count = 0;
            foreach (var myPost in myPosts)
            {
				if(count < 20) 
				{
					count++;
					yield return GetPostViewModel(myPost);
				}
				else yield break;
                	
            }
        }

		public async IAsyncEnumerable<Task<Form>> GetExtraPostsViewModels()
		{
			var myInterestedPosts = (await _userService.MappingPostsByTag(_user, selection => selection.Type = new List<string>() { "Interested" })).ToList();
            var allPosts = (await _postService.GetAllPost()).ToList();
			for(var count = 0; count < 20; count++) {
				var rand = new Random().Next();
				if (rand % 3 == 0)
				{
					if(myInterestedPosts.Count != 0)
						yield return GetPostViewModel(myInterestedPosts[rand%myInterestedPosts.Count]);
				} 
				else{
					if(allPosts.Count != 0)
						yield return GetPostViewModel(allPosts[rand%allPosts.Count]);
				} 
			}
			
		}

        public async IAsyncEnumerable<Task<Form>> GetZonePostsViewModels()
        {
            IList<User> myFocusUsers = await _userService.SelectUserInitiative(_user,
        selections => selections.Type = new List<string>() { "Focus" });

            List<Post> myFocusPosts = new List<Post>();
            foreach (var myFocusUser in myFocusUsers)
            {
                IList<Post> myFocusUserPosts = await _userService.SelectPost(myFocusUser, selection => { });
                foreach (var myFocusUserPost in myFocusUserPosts)
                {
                    myFocusPosts.Add(myFocusUserPost);
                }
            }

            myFocusPosts.Sort((post_1, post_2) => (post_2.PostDateTime - post_1.PostDateTime).Seconds);

            DateTime dateTemp = default;

            foreach (var myFocusPost in myFocusPosts)
            {
                if (dateTemp.Year != myFocusPost.PostDateTime.Year ||
                dateTemp.Month != myFocusPost.PostDateTime.Month ||
                dateTemp.Day != myFocusPost.PostDateTime.Day)
                {
					yield return Task.Run(() => new Form()
					{
						{"Time", myFocusPost.PostDateTime.ToString("D",CultureInfo.CreateSpecificCulture("zh-CN"))}
					});
                    dateTemp = myFocusPost.PostDateTime;
                }
                yield return GetPostViewModel(myFocusPost);
            }
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
                {"AuthorIsFocus", isFocus.ToString()},
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