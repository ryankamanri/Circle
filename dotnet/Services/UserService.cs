using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using dotnet.Model;
using dotnet.Services.Database;
using dotnet.Services.Cookie;

namespace dotnet.Services
{
    class UserService
    {
        private DataBaseContext _dbc;

        private ICookie _cookie;

        public User user {get; set;}

        public UserService(DataBaseContext dbc,ICookie cookie)
        {
            _dbc = dbc;
            
        }


        public IList<Post> GetAllPosts() 
        {
            throw new NotImplementedException();
        }


    }
}