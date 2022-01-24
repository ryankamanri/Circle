using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Kamanri.Self;
using Microsoft.Extensions.Logging;
namespace ChatServer.Services
{
    public class UserService
    {

        public Dictionary<long, long> onlineUserID_WebSocketIDs { get; set; }

        public Dictionary<long, Mutex> onlineUserID_WebSocketIDsMutex { get; set; }

        public ILogger<UserService> _logger;

        public UserService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserService>();
            onlineUserID_WebSocketIDs = new Dictionary<long, long>();
            onlineUserID_WebSocketIDsMutex = new Dictionary<long, Mutex>();
        }

        public long GetWebSocketIDByOnlineUserID(long eventID, long onlineUserID)
        {
            long webSocketID;
            if (!onlineUserID_WebSocketIDs.TryGetValue(onlineUserID, out webSocketID))
                return default;
            return onlineUserID_WebSocketIDs[onlineUserID];
        }

        public long GetOnlineUserIDByWebSocketID(long eventID, long webSocketID)
        {
            return (from onlineUserID_WebSocketID in onlineUserID_WebSocketIDs
                    where onlineUserID_WebSocketID.Value == webSocketID
                    select onlineUserID_WebSocketID.Key).FirstOrDefault();
        }
    }
}