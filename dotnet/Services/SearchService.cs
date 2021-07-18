using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using dotnet.Services.Database;
using dotnet.Model.Relation;
using dotnet.Model;
namespace dotnet.Services
{
    public class SearchService
    {
        private DataBaseContext _dbc;

        private UserService _userService;

        public SearchService(DataBaseContext dbc,UserService userService)
        {
            _dbc = dbc;
            _userService = userService;
        }

        public async Task<Key_ListValue_Pairs<UserInfo,Tag>> SearchUserInfoAndTags(string searchString)
        {
            if(searchString == "" || searchString == default) return new Key_ListValue_Pairs<UserInfo,Tag>();
            searchString = searchString.Replace("'","\'");

            IList<UserInfo> userInfosByBase = await _dbc.SelectCustom<UserInfo>(new UserInfo(),
            $"NickName like '%{searchString}%' or RealName like '%{searchString}%' or University like '%{searchString}%' or School like '%{searchString}%' or Speciality like '%{searchString}%' or Introduction like '%{searchString}%'");
            
            Key_ListValue_Pairs<User,Tag> tagsGroupByUser = await _dbc.MappingSelectUnionStatistics<Tag,User>(
                await SearchTag(searchString),new User(),Model.Relation.ID_IDList.OutPutType.Key,
                selection => selection.Type = new List<string>(){"Self"});

            Key_ListValue_Pairs<UserInfo,Tag> tagsGroupByUserInfo  = new Key_ListValue_Pairs<UserInfo,Tag>();
            Key_ListValue_Pairs<UserInfo,Tag> tagsGroupByUserInfoBase  = new Key_ListValue_Pairs<UserInfo,Tag>();
            foreach(var user_tags in tagsGroupByUser)
            {
                tagsGroupByUserInfo.Add(new KeyValuePair<UserInfo, List<Tag>>(await _userService.GetUserInfo(user_tags.Key),user_tags.Value));
            }

            foreach(var userInfo in userInfosByBase)
            {
                tagsGroupByUserInfoBase.Add(new KeyValuePair<UserInfo, List<Tag>>(userInfo,new List<Tag>()));
            }
            
            return tagsGroupByUserInfo.Union(tagsGroupByUserInfoBase);
            
        }

        public async Task<IList<Post>> SearchPost(string searchString)
        {
            if(searchString == "" || searchString == default) return new List<Post>();
            searchString = searchString.Replace("'","\'");
            return await _dbc.SelectCustom<Post>(new Post(),
            $"Title like '%{searchString}%' or Summary like '%{searchString}%' or Focus like '%{searchString}%'");
        }

        public async Task<IList<Tag>> SearchTag(string searchString)
        {
            if(searchString == "" || searchString == default) return new List<Tag>();
            searchString = searchString.Replace("'","\'");
            return await _dbc.SelectCustom<Tag>(new Tag(),$"tag like '%{searchString}%'");
        }
    }
}