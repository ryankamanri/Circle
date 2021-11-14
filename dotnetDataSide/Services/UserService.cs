using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using dotnetDataSide.Models;
using Kamanri.Extensions;
namespace dotnetDataSide.Services
{
    public class UserService
    {
        public Dictionary<User, long> OnlineUsers { get; }
        public ILogger<UserService> _logger;
        public UserService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserService>();
            OnlineUsers = new Dictionary<User, long>();
        }

        /// <summary>
        /// 添加在线用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="serviceID"></param>
        public void AppendOnlineUser(int eventCode, User user, long serviceID)
        {
            var selectedUser = (from userItem in OnlineUsers.Keys 
            where userItem.ID == user.ID 
            select userItem).FirstOrDefault();
            if(selectedUser != default)
            {
                if(OnlineUsers[selectedUser] != serviceID)
                {
                    _logger.LogInformation(eventCode, $"Convert The Service ID Of ({user.ToJson()}) To {serviceID}");
                    OnlineUsers[selectedUser] = serviceID;
                }
            }
            else OnlineUsers.Add(user, serviceID);
            _logger.LogInformation(eventCode, $"Now OnlineUsers Count : {OnlineUsers.Count}");
        }

        public void RemoveOnlineUser(User user)
        {
            OnlineUsers.Remove(user);
        }

        public long SelectServerIDByUserID(long userID)
        {
            var selectedServerID = (from user_serverID in OnlineUsers
            where user_serverID.Key.ID == userID
            select user_serverID.Value).FirstOrDefault();
            return selectedServerID;
        }

    }
}