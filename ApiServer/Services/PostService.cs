
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Kamanri.Database;
using Kamanri.Database.Models.Relation;
using ApiServer.Models.User;
using ApiServer.Models;
using ApiServer.Models.Post;

namespace ApiServer.Services
{
    public class PostService
    {
        private DataBaseContext _dbc;

        private UserService _userService;


        public PostService(DataBaseContext dbc, UserService userService)
        {
            _dbc = dbc;
            _userService = userService;
        }

        #region Get

        public async Task<IList<Post>> GetAllPost()
        {
            return await _dbc.SelectAll<Post>();
        }

        public async Task<PostInfo> GetPostInfo(Post post)
        {
            return await _dbc.Select(new PostInfo(post.ID));
        }

        #endregion

        #region Select


        public async Task<UserInfo> SelectAuthorInfo(Post post)
        {
            // 这儿要修改成根据relation = {Type:["Owned"]}来寻找作者
            ICollection<User> authorCollection = (await _dbc.Mapping<Post, User>(post, ID_IDList.OutPutType.Key)).Keys;
            IEnumerator<User> userEnumerator = authorCollection.GetEnumerator();
            if (!userEnumerator.MoveNext()) return new UserInfo();
            User user = userEnumerator.Current;
            return await _userService.GetUserInfo(user);

        }

        public async Task<ICollection<Tag>> SelectTags(Post post)
        {
            return (await _dbc.Mapping<Post, Tag>(post, ID_IDList.OutPutType.Value)).Keys;
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
        public async Task<bool> InsertPost(User author, string title, string focus, string summary, string content, IList<long> tagIDs)
        {
            title = title.Replace("\'", "\\\'");
            focus = focus.Replace("\'", "\\\'");
            summary = summary.Replace("\'", "\\\'");
            content = content.Replace("\'", "\\\'");
            //1. 保存帖子信息
            // string
            Post post = new Post(title, summary, focus, DateTime.Now);
            await _dbc.Insert(post);
            //2. 获取帖子ID in 数据库
            long ID = await _dbc.SelectID(post);
            //3. 保存帖子内容
            PostInfo postInfo = new PostInfo(ID, content);
            await _dbc.InsertWithID(postInfo);
            //4. 保存帖子与标签关系
            foreach (var tagID in tagIDs)
            {
                // await _dbc.Connect<Post,Tag>(post,new Tag(tagID), relations => relations.Type = "Owned");
                await _dbc.AppendRelation(post, new Tag(tagID), "Type", "Owned"); // 设计为添加一个Type关系, 而不是直接建立关系
            }

            //5. 保存帖子与作者关系

            // await _dbc.Connect<User,Post>(author,post,relations => relations.Type = "Owned");
            await _dbc.AppendRelation(author, post, "Type", "Owned");
            return true;

        }
    }
}