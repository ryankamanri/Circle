using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using dotnet.Model;
using dotnet.Model.Relation;
using dotnet.Services.Database;
using dotnet.Services.Cookie;

namespace dotnet.Services
{
    public class UserService
    {
        private DataBaseContext _dbc;

        private ICookie _cookie;

        private TagService _tagService;

        private IDictionary<string,Object> entityDictionary;

        public IList<User> Users{get;private set;}


        public UserService(DataBaseContext dbc,ICookie cookie,TagService tagService)
        {
            _dbc = dbc;
            _cookie = cookie;
            _tagService = tagService;
            InitUsers().Wait();

            entityDictionary = new Dictionary<string,object>();
            entityDictionary.Add("Comment",new Comment());
            entityDictionary.Add("Post",new Post());
            entityDictionary.Add("PostInfo",new PostInfo());
            entityDictionary.Add("User",new User());
            entityDictionary.Add("UserInfo",new UserInfo());
            entityDictionary.Add("Tag",new Tag());
        }

        private async Task InitUsers()
        {
            Users = await _dbc.SelectAll<User>(new User());
        }


        public async Task<UserInfo> GetUserInfo(User user)
        {
            var userInfo = await _dbc.Select<UserInfo>(new UserInfo(user.ID));

            if(userInfo == default) return new UserInfo(user.ID);

            return userInfo;
        }

        /// <summary>
        /// 用户匹配标签
        /// </summary>
        /// <param name="type"></param>
        /// <param name="SetSelections"></param>
        /// <returns></returns>
        public async Task<IList<Tag>> SelectTag(User user,Action<dynamic> SetSelections)
        {
            return await _dbc.MappingSelect<User,Tag>(user,new Tag(),ID_IDList.OutPutType.Value,SetSelections);
        }

        /// <summary>
        /// 新增本用户标签关系
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="tagRelation"></param>
        /// <returns></returns>
        // public async Task AppendTagRelation(User user,Tag tag, string tagRelation)
        // {
        //     await _dbc.AppendRelation<User,Tag>(user,tag,"Type",tagRelation);

        // }

        public async Task<bool> AppendRelation(User user,string entityType,long ID,string relationName,string newRelation)
        {
            dynamic entity;
            
            entityDictionary.TryGetValue(entityType,out entity);

            if(entity == null) return false;

            entity.ID = Convert.ToInt64(ID);
            
            await _dbc.AppendRelation(user,entity,relationName,newRelation);

            return true;
        }

        /// <summary>
        /// 移除本用户标签关系
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="tagRelation"></param>
        /// <returns></returns>
        // public async Task RemoveTagRelation(User user,Tag tag,string tagRelation)
        // {
        //     await _dbc.RemoveRelation<User,Tag>(user,tag,"Type",tagRelation);
        // }
        public async Task<bool> RemoveRelation(User user,string entityType,long ID,string relationName,string oldRelation)
        {
            dynamic entity;
            
            entityDictionary.TryGetValue(entityType,out entity);

            if(entity == null) return false;

            entity.ID = Convert.ToInt64(ID);
            
            await _dbc.RemoveRelation(user,entity,relationName,oldRelation);

            return true;
        }

        /// <summary>
        /// 添加关注的人
        /// </summary>
        /// <param name="yourself"></param>
        /// <param name="focusUser"></param>
        /// <param name="relationName"></param>
        /// <param name="userRelation"></param>
        /// <returns></returns>
        public async Task AppendFocusUser(User yourself,User focusUser,string userRelation)
        {
            await _dbc.AppendRelation<User,User>(yourself,focusUser,"Focus",userRelation);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="yourself"></param>
        /// <param name="focusUser"></param>
        /// <param name="relationName"></param>
        /// <param name="userRelation"></param>
        /// <returns></returns>
        public async Task RemoveFocusUser(User yourself,User focusUser,string userRelation)
        {
            await _dbc.RemoveRelation<User,User>(yourself,focusUser,"Focus",userRelation);
        }

        public IList<Post> GetAllPosts() 
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Post>> MappingInterestedPosts(User user)
        {
            IList<Tag> interestedTags = await _dbc.MappingSelect<User,Tag>(user,new Tag(),ID_IDList.OutPutType.Value,
            selection => selection.Type = new List<string>(){"Interested"});
            Key_ListValue_Pairs<Post, KeyValuePair<Tag, dynamic>> interestedPosts = await _dbc.MappingUnionStatistics<Tag,Post>(interestedTags,new Post(),ID_IDList.OutPutType.Key);
            return interestedPosts.Keys;
        }

        public async Task<double> CarculateSimilarity(User user1,User user2,Action<dynamic> SetMyTagsType)
        {
            IList<Tag> tagsOfU1 = await SelectTag(user1,SetMyTagsType);
            IList<Tag> tagsOfU2 = await SelectTag(user2,selection => selection.Type = new List<string>{"Self"});

            double tagSimilarity = 0,tagSimilarityMax = 0,tagSimilaritySum = 0;
            double tagCrossJoin = tagsOfU1.Count * tagsOfU2.Count;
            foreach(var tagOfU1 in tagsOfU1)
            {
                tagSimilarityMax = 0;
                foreach(var tagOfU2 in tagsOfU2)
                {
                    tagSimilarity = _tagService.CalculateSimilarity(tagOfU1,tagOfU2);
                    tagSimilarityMax = Math.Max(tagSimilarity,tagSimilarityMax);
                    
                }
                tagSimilaritySum += tagSimilarityMax * tagsOfU2.Count;
            }

            return (tagSimilaritySum / tagCrossJoin) * ((double)Math.Min(tagsOfU1.Count,tagsOfU2.Count)/(double)Math.Max(tagsOfU1.Count,tagsOfU2.Count));
            
        }


    }
}