using System.Collections.Generic;
using System.Net.WebSockets;
namespace dotnetPrivateChatApi.Services
{
    public class UserService
    {
        public Dictionary<long, WebSocket> onlineUsers;

        public UserService()
        {
            onlineUsers = new Dictionary<long, WebSocket>();
        }
    }
}