using dotnet.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Services;
using dotnet.Services.Database;

namespace dotnet.Services
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

        public async Task<UserInfo> GetAuthorInfo(Post post)
        { 
            ICollection<User> authorCollection = (await _dbc.Mapping<Post,User>(post,new User(),Model.Relation.ID_IDList.OutPutType.Key)).Keys;
            IEnumerator<User> userEnumerator = authorCollection.GetEnumerator();
            if(!userEnumerator.MoveNext()) return new UserInfo();
            User user = userEnumerator.Current;
            return await _userService.GetUserInfo(user);

        }

        public async Task<ICollection<Tag>> GetTags(Post post)
        {
            return (await _dbc.Mapping<Post,Tag>(post,new Tag(),Model.Relation.ID_IDList.OutPutType.Value)).Keys;
        }

        public async Task<PostInfo> GetPostInfo(Post post)
        {
            return await _dbc.Select<PostInfo>(new PostInfo(post.ID));
        }
        
    }
}