using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using dotnet.Model.Relation;
using dotnet.Services.Extensions;
using dotnet.Model;


namespace dotnet.Services.Database
{

    /// <summary>
    /// 数据库上下文类,用于暂存数据库的部分数据  
    /// 所有对数据库关系的操作基于这个类进行
    /// </summary>
    public class DataBaseContext
    {
        private SQL _sql;

        private User user;

        private Post post;

        private Tag tag;

        private KeyComparer keyComparer;

        private ValueComparer valueComparer;

        public Dictionary<string,List<Tag>> TagIndex {get;private set;}

        /// <summary>
        /// 按照key/value排序好的关系表
        /// </summary>
        /// <value></value>

        public ID_IDList SortedUsers_Posts {get;private set;}

        public ID_IDList SortedUsers_Tags {get;private set;}

        public ID_IDList SortedPosts_Tags {get;private set;}

        public ID_IDList Users_SortedPosts {get;private set;}

        public ID_IDList Users_SortedTags {get;private set;}

        public ID_IDList Posts_SortedTags {get;private set;}


        
        public DataBaseContext(SQL sql)
        {
            _sql = sql;

            user = new User();
            post = new Post();
            tag = new Tag();

            keyComparer = new KeyComparer();
            valueComparer = new ValueComparer();
            
            TagIndex = new Dictionary<string, List<Tag>>();
            InitTagIndex();

            SortedUsers_Posts = ID_ID.GetList( _sql.Query("select * from schema1.users_posts"));
            SortedUsers_Tags = ID_ID.GetList(_sql.Query("select * from schema1.users_tags"));
            SortedPosts_Tags = ID_ID.GetList(_sql.Query("select * from schema1.posts_tags"));
            Users_SortedPosts = ID_ID.GetList( _sql.Query("select * from schema1.users_posts"));
            Users_SortedTags = ID_ID.GetList(_sql.Query("select * from schema1.users_tags"));
            Posts_SortedTags = ID_ID.GetList(_sql.Query("select * from schema1.posts_tags"));

            

            SortedUsers_Posts.Sort(new Comparison<ID_ID>(keyComparer.Compare));
            SortedUsers_Tags.Sort(new Comparison<ID_ID>(keyComparer.Compare));
            SortedPosts_Tags.Sort(new Comparison<ID_ID>(keyComparer.Compare));

            Users_SortedPosts.Sort(new Comparison<ID_ID>(valueComparer.Compare));
            Users_SortedTags.Sort(new Comparison<ID_ID>(valueComparer.Compare));
            Posts_SortedTags.Sort(new Comparison<ID_ID>(valueComparer.Compare));
        }

        /// <summary>
        /// 两个实体之间建立关系
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public async Task Connect<T1,T2>(T1 model1,T2 model2)
        {
            dynamic m1 = model1,m2 = model2;

            Type t1Type = m1.GetType(),
            t2Type = m2.GetType(),
            userType = user.GetType(),
            postType = post.GetType(),
            tagType = tag.GetType();

            ID_ID newConnection = new ID_ID(m1.ID,m2.ID);

            if(t1Type == userType)
            {
                if(t2Type == postType)
                {
                    if(Users_SortedPosts.Contains(newConnection)) return;
                    Users_SortedPosts.Add(newConnection);
                    Users_SortedPosts.Sort(new Comparison<ID_ID>(valueComparer.Compare));
                    SortedUsers_Posts.Add(newConnection);
                    SortedUsers_Posts.Sort(new Comparison<ID_ID>(keyComparer.Compare));
                    await _sql.Execute($"insert into users_posts values({newConnection.ID},{newConnection.ID_2})");
                }
                else if(t2Type == tagType)
                {
                    if(Users_SortedTags.Contains(newConnection)) return;
                    Users_SortedTags.Add(newConnection);
                    Users_SortedTags.Sort(new Comparison<ID_ID>(valueComparer.Compare));
                    SortedUsers_Tags.Add(newConnection);
                    SortedUsers_Tags.Sort(new Comparison<ID_ID>(keyComparer.Compare));
                    await _sql.Execute($"insert into users_tags values({newConnection.ID},{newConnection.ID_2})");
                }
            }

            else if(t1Type == postType && t2Type == tagType)
            {
                if(Posts_SortedTags.Contains(newConnection)) return;
                Posts_SortedTags.Add(newConnection);
                Posts_SortedTags.Sort(new Comparison<ID_ID>(valueComparer.Compare));
                SortedPosts_Tags.Add(newConnection);
                SortedPosts_Tags.Sort(new Comparison<ID_ID>(keyComparer.Compare));
                await _sql.Execute($"insert into posts_tags values({newConnection.ID},{newConnection.ID_2})");
            }
            
            
        }


        /// <summary>
        /// 两个实体之间解除关系
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public async Task DisConnect<T1,T2>(T1 model1,T2 model2)
        {
            dynamic m1 = model1,m2 = model2;

            Type t1Type = m1.GetType(),
            t2Type = m2.GetType(),
            userType = user.GetType(),
            postType = post.GetType(),
            tagType = tag.GetType();

            ID_ID newConnection = new ID_ID(m1.ID,m2.ID);

            if(t1Type == userType)
            {
                if(t2Type == postType)
                {
                    if(!Users_SortedPosts.Contains(newConnection)) return;
                    Users_SortedPosts.Remove(newConnection);
                    //Users_SortedPosts.Sort(new Comparison<ID_ID>(valueComparer.Compare));
                    SortedUsers_Posts.Remove(newConnection);
                    //SortedUsers_Posts.Sort(new Comparison<ID_ID>(keyComparer.Compare));
                    await _sql.Execute($"delete from users_posts where usersID = {newConnection.ID} and postsID = {newConnection.ID_2}");
                }
                else if(t2Type == tagType)
                {
                    if(!Users_SortedTags.Contains(newConnection)) return;
                    Users_SortedTags.Remove(newConnection);
                    //Users_SortedTags.Sort(new Comparison<ID_ID>(valueComparer.Compare));
                    SortedUsers_Tags.Remove(newConnection);
                    //SortedUsers_Tags.Sort(new Comparison<ID_ID>(keyComparer.Compare));
                    await _sql.Execute($"delete from users_tags where usersID = {newConnection.ID} and tagsID = {newConnection.ID_2}");
                }
            }

            else if(t1Type == postType && t2Type == tagType)
            {
                if(!Posts_SortedTags.Contains(newConnection)) return;
                Posts_SortedTags.Remove(newConnection);
                //Posts_SortedTags.Sort(new Comparison<ID_ID>(valueComparer.Compare));
                SortedPosts_Tags.Remove(newConnection);
                //SortedPosts_Tags.Sort(new Comparison<ID_ID>(keyComparer.Compare));
                await _sql.Execute($"delete from posts_tags where postsID = {newConnection.ID} and tagsID = {newConnection.ID_2}");
            }
            
            
        }

        /// <summary>
        /// 标签索引初始化
        /// </summary>
        private void InitTagIndex()
        {
            IList<Tag> tags = tag.GetList(_sql.Query("select * from tags"));
            foreach(var tag in tags)
            {
                List<string> subset = tag.GetContinuousSubset();
                foreach(var subItem in subset)
                {
                    if(TagIndex.ContainsKey(subItem))
                        TagIndex[subItem].Add(tag);
                    
                    else TagIndex.Add(subItem,new List<Tag>(){tag});
                }
            }
        }

    }

}
