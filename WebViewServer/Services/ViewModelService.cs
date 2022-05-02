using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Kamanri.Extensions;
using Kamanri.Http;
using Newtonsoft.Json.Linq;
using WebViewServer.Models;
using WebViewServer.Models.Post;
using WebViewServer.Models.User;

namespace WebViewServer.Services
{
    public class ViewModelService
    {
	    private Api _api;
        private readonly PostService _postService;
        private readonly UserService _userService;
        private readonly SearchService _searchService;

        private readonly User _user;
        public ViewModelService(SearchService searchService, Api api, PostService postService, UserService userService, User user)
        {
	        _api = api;
	        _searchService = searchService;
            _postService = postService;
            _userService = userService;
            _user = user;
        }

        #region Pages

        #region Home

        

        

        public async IAsyncEnumerable<Task<Form>> GetHomePostsViewModels(PostService.CircleType type)
        {
            var myInterestedPosts = await _userService.MappingPostsByTag(_user, selection => selection.Type = new List<string>() { "Interested" });
            var allPosts = await _postService.GetAllPost();
            allPosts.Remove(allPosts.FirstOrDefault(post => post.ID == -1));
            var myPosts = myInterestedPosts.Union(allPosts, new Post());
            var count = 0;
            foreach (var myPost in myPosts)
            {
	            var myPostType = await _postService.GetCircleType(myPost);
	            if(!type.Employment && myPostType.Employment) continue;
	            if(!type.Postgraduate && myPostType.Postgraduate) continue;
				if(count < 20) 
				{
					count++;
					yield return GetPostViewModel(myPost);
				}
				else yield break;
                	
            }
        }

		public async IAsyncEnumerable<Task<Form>> GetExtraPostsViewModels(PostService.CircleType type)
		{
			var myInterestedPosts = (await _userService.MappingPostsByTag(_user, selection => selection.Type = new List<string>() { "Interested" })).ToList();
            var allPosts = (await _postService.GetAllPost()).ToList();
            allPosts.Remove(allPosts.FirstOrDefault(post => post.ID == -1));
			for(var count = 0; count < 20; count++) {
				var rand = new Random().Next();
				if (rand % 3 == 0)
				{
					if (myInterestedPosts.Count == 0) continue;
					var myPost = myInterestedPosts[rand % myInterestedPosts.Count];
					var myPostType = await _postService.GetCircleType(myPost);
					if (!type.Employment && myPostType.Employment) continue;
					if (!type.Postgraduate && myPostType.Postgraduate) continue;
					yield return GetPostViewModel(myPost);
				} 
				else
				{
					if (allPosts.Count == 0) continue;
					var myPost = allPosts[rand % allPosts.Count];
					var myPostType = await _postService.GetCircleType(myPost);
					if(!type.Employment && myPostType.Employment) continue;
					if(!type.Postgraduate && myPostType.Postgraduate) continue;
					yield return GetPostViewModel(myPost);
				} 
			}
			
		}

        public async IAsyncEnumerable<Task<Form>> GetZonePostsViewModels()
        {
            IList<User> myFocusUsers = await _userService.SelectUserInitiative(_user,
        selections => selections.Type = new List<string>() { "Focus" });

            IEnumerable<Post> myFocusPosts = new List<Post>();
            foreach (var myFocusUser in myFocusUsers)
            {
                IList<Post> myFocusUserPosts = await _userService.SelectPost(myFocusUser, selection => { });
                myFocusPosts = myFocusPosts.Union(myFocusUserPosts, new Post());
            }

            var myFocusPostList = myFocusPosts.ToList();
			myFocusPostList.Sort((post_1, post_2) => DateTime.Compare(post_2.PostDateTime, post_1.PostDateTime));
            DateTime dateTemp = default;

            foreach (var myFocusPost in myFocusPostList)
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

        public async Task<IList<KeyValuePair<Comment, Form>>> GetNoticeViewModels()
        {
	        var result = new List<KeyValuePair<Comment, Form>>();
	        // Get My Post Comment
	        var myPosts =
		        await _userService.SelectPost(_user, selection => selection.Type = new List<string>() { "Owned" });
	        foreach (var myPost in myPosts)
	        {
		        var comments = await _postService.SelectComments(myPost);
		        foreach (var comment in comments)
		        {
			        var commentModel = await GetCommentViewModel(comment);
			        result.Add(commentModel);
		        }
	        }

	        // Get Reply My Comment
	        var myComments =
		        await _userService.SelectComment(_user, selection => selection.Type = new List<string>() { "Owned" });
	        foreach (var myComment in myComments)
	        {
		        var replyComments = await _postService.SelectReplyComments(myComment);
		        foreach (var replyComment in replyComments)
		        {
			        var commentModel = await GetCommentViewModel(replyComment);
			        result.Add(commentModel);
		        }
	        }
	        
	        result.Sort((comment_1, comment_2) => DateTime.Compare(comment_2.Key.CommentDateTime, comment_1.Key.CommentDateTime));

	        return result;
        }
        #endregion

        public async IAsyncEnumerable<Task<Form>> GetUserPagePostsViewModels()
        {

	        var myPostList = await _userService.SelectPost(_user, selections => { });
	        myPostList.ToList().Sort((post_1, post_2) => DateTime.Compare(post_2.PostDateTime, post_1.PostDateTime));
	        DateTime dateTemp = default;

	        foreach (var myPost in myPostList)
	        {
		        if (dateTemp.Year != myPost.PostDateTime.Year ||
		            dateTemp.Month != myPost.PostDateTime.Month ||
		            dateTemp.Day != myPost.PostDateTime.Day)
		        {
			        yield return Task.Run(() => new Form()
			        {
				        {"Time", myPost.PostDateTime.ToString("D",CultureInfo.CreateSpecificCulture("zh-CN"))}
			        });
			        dateTemp = myPost.PostDateTime;
		        }
		        yield return GetPostViewModel(myPost);
	        }
        }

        public async Task<Form> GetMatchModels()
        {
	        var matchUserInfo_Tag = await _api.Get<Form>($"/Match/Match?userID={_user.ID}");
	        var matchUserInfoModelList = new List<Form>();
	        foreach (var userInfoToken in (JArray)matchUserInfo_Tag["MatchUserInfoList"])
	        {
		        var userInfo = userInfoToken.ToJson().ToObject<UserInfo>();
		        var tags = await _userService.SelectTag(new User(userInfo.ID),
			        selection => selection.Type = new List<string>() { "Self" });
		        matchUserInfoModelList.Add(await GetSearchUserInfoViewModel(new KeyValuePair<UserInfo, IList<Tag>>(userInfo, tags)));
	        }
	        matchUserInfo_Tag["MatchUserInfoList"] = matchUserInfoModelList;
	        return matchUserInfo_Tag;
        }

        #endregion

        
        #region Models

        public async Task<Form> GetSearchResultViewModel(string searchString)
        {
	        var userInfo_tagLists = await _searchService.SearchUserInfoAndTags(searchString);
	        var posts = await _searchService.SearchPosts(searchString);
	        var userinfoModelList = new List<Form>();
	        foreach (var userInfo_tagList in userInfo_tagLists)
	        {
		        userinfoModelList.Add(await GetSearchUserInfoViewModel(userInfo_tagList));
	        }

	        var postModels = new List<Form>();
	        foreach (var post in posts)
	        {
		        postModels.Add(await GetPostViewModel(post));
	        }
	        return new Form()
	        {
		        {"UserInfos", userinfoModelList},
		        {"Posts", postModels}
	        };
        }

        public async Task<Form> GetSearchUserInfoViewModel(KeyValuePair<UserInfo,IList<Tag>> userInfo_tagList)
        {
	        IList<Tag> userTags = await _userService.SelectTag(new User(userInfo_tagList.Key.ID),relation => relation.Type = new List<string>{"Self"});
	        IEnumerable<Tag> showTags = userInfo_tagList.Value.Union(userTags,new Tag()).Take(Math.Min(4, userTags.Count));
	        bool isFocus = await _userService.IsUserRelationExist(_user,new User(userInfo_tagList.Key.ID),"Type","Focus");
	        double similarity = await _userService.CarculateSimilarityFix(_user,new User(userInfo_tagList.Key.ID),selection => selection.Type = new List<string>(){"Self"});
	        double interesty = await _userService.CarculateSimilarityFix(_user,new User(userInfo_tagList.Key.ID),selection => selection.Type = new List<string>(){"Interested"});

	        return new Form()
	        {
		        { "HeadImage", userInfo_tagList.Key.HeadImage },
		        { "NickName", userInfo_tagList.Key.NickName },
		        { "SchoolYear", userInfo_tagList.Key.SchoolYear },
		        { "Speciality", userInfo_tagList.Key.Speciality },
		        { "Tags", showTags },
		        { "Similarity", similarity },
		        { "Interesty", interesty },
		        { "ID", userInfo_tagList.Key.ID },
		        { "IsFocus", isFocus }
	        };
        }

        public async Task<KeyValuePair<Comment, Form>> GetCommentViewModel(Comment comment)
        {
	        return await _postService.SelectAFormedCommentAndUser(comment, _user);
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
                {"CommentCount", likeCollectCommentCount["CommentCount"]},
                {"PostDateTime", post.PostDateTime}
            };
        }

        #endregion

        
    }
}