using System.Collections.Generic;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Kamanri.Http;
using WebViewServer.Models;
using WebViewServer.Models.Post;
using WebViewServer.Models.User;

namespace WebViewServer.Services
{
    public class PostService
    {

        private Api _api;



        public PostService(Api api)
        {
            _api = api;
        }

        #region Get

        public async Task<IList<Post>> GetAllPost()
        {
            return await _api.Get<IList<Post>>("/Post/GetAllPost");
        }

        public async Task<PostInfo> GetPostInfo(Post post)
        {
            return await _api.Post<PostInfo>("/Post/GetPostInfo", new Form()
            {
                {"Post", post}
            });
        }

        #endregion

        #region Select


        public async Task<UserInfo> SelectAuthorInfo(Post post)
        {

            return await _api.Post<UserInfo>("/Post/SelectAuthorInfo", new Form()
            {
                {"Post", post}
            });
        }

        public async Task<ICollection<Tag>> SelectTags(Post post)
        {
            return await _api.Post<ICollection<Tag>>("/Post/SelectTags", new Form()
            {
                {"Post", post}
            });
        }

        #endregion

        /// <summary>
        /// 保存帖子
        /// </summary>
        /// <param name="author"></param>
        /// <param name="title"></param>
        /// <param name="focus"></param>
        /// <param name="content"></param>
        /// <param name="tagIDs"></param>
        /// <returns></returns>
        public async Task<bool> InsertPost(User author, string title, string focus, string summary, string content, string tagIDs)
        {
            var TagIDs = tagIDs.ToObject<IList<long>>();
            return await _api.Post<bool>("/Post/InsertPost", new Form()
            {
                {"Author", author},
                {"Title", title},
                {"Focus", focus},
                {"Summary", summary},
                {"Content", content},
                {"TagIDs", TagIDs}
            });
        }
    }
}