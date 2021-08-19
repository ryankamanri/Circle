
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using dotnetApi.Model;
using dotnetApi.Services;
using dotnetApi.Services.Database;

namespace dotnetApi.Services
{
    public class PostService
    {
        private DataBaseContext _dbc;

        private UserService _userService;

        
        public PostService(DataBaseContext dbc,UserService userService)
        {
            _dbc = dbc;
            _userService = userService;
        }

        #region Get

        public async Task<IList<Post>> GetAllPost()
        {
            return await _dbc.SelectAll<Post>(new Post());
        }

        public async Task<PostInfo> GetPostInfo(Post post)
        {
            return await _dbc.Select<PostInfo>(new PostInfo(post.ID));
        }
        
        #endregion

        #region Select
            
        
        public async Task<UserInfo> SelectAuthorInfo(Post post)
        { 
            ICollection<User> authorCollection = (await _dbc.Mapping<Post,User>(post,new User(),Model.Relation.ID_IDList.OutPutType.Key)).Keys;
            IEnumerator<User> userEnumerator = authorCollection.GetEnumerator();
            if(!userEnumerator.MoveNext()) return new UserInfo();
            User user = userEnumerator.Current;
            return await _userService.GetUserInfo(user);

        }

        public async Task<ICollection<Tag>> SelectTags(Post post)
        {
            return (await _dbc.Mapping<Post,Tag>(post,new Tag(),Model.Relation.ID_IDList.OutPutType.Value)).Keys;
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
        public async Task<bool> InsertPost(User author,string title,string focus,string summary, string content,IList<long> tagIDs)
        {
            title = title.Replace("\'","\\\'");
            focus = focus.Replace("\'","\\\'");
            summary = summary.Replace("\'","\\\'");
            content = content.Replace("\'","\\\'");
            //1. 保存帖子信息
            // string
            Post post = new Post(title,summary,focus,DateTime.Now);
            await _dbc.Insert<Post>(post);
            //2. 获取帖子ID in 数据库
            long ID = await _dbc.SelectID<Post>(post);
            //3. 保存帖子内容
            PostInfo postInfo = new PostInfo(ID,content);
            await _dbc.InsertWithID<PostInfo>(postInfo);
            //4. 保存帖子与标签关系
            foreach(var tagID in tagIDs)
            {
                await _dbc.Connect<Post,Tag>(post,new Tag(tagID), relations => relations.Type = "Owned");
            }
            
            //5. 保存帖子与作者关系

            await _dbc.Connect<User,Post>(author,post,relations => relations.Type = "Owned");

            return true;
            
        }
    }
}