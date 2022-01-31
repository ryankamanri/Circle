using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Kamanri.Extensions;

namespace ChatDataServer.Services
{
	public class UserService
	{
		public Dictionary<long, long> OnlineUserID_ServerID { get; }
		public ILogger<UserService> _logger;
		public UserService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<UserService>();
			OnlineUserID_ServerID = new Dictionary<long, long>();
		}

		/// <summary>
		/// 添加在线用户
		/// </summary>
		/// <param name="user"></param>
		/// <param name="serviceID"></param>
		public void AppendOnlineUser(int eventCode, long userID, long serviceID)
		{
			var selectedUser = (from userItemID in OnlineUserID_ServerID.Keys
								where userItemID == userID
								select userItemID).FirstOrDefault();
			if (selectedUser != default)
			{
				if (OnlineUserID_ServerID[selectedUser] != serviceID)
				{
					_logger.LogInformation(eventCode, $"Convert The Service ID Of ({userID.ToJson()}) To {serviceID}");
					OnlineUserID_ServerID[selectedUser] = serviceID;
				}
			}
			else OnlineUserID_ServerID.Add(userID, serviceID);
			_logger.LogInformation(eventCode, $"Now OnlineUserID_ServerID Count : {OnlineUserID_ServerID.Count}");
		}

		public void RemoveOnlineUser(int eventCode, long userID, long serviceID)
		{
			var selectedUser = (from userID_serviceID in OnlineUserID_ServerID
								where userID_serviceID.Key == userID
								select userID_serviceID.Key).FirstOrDefault();
			if (selectedUser == default) _logger.LogError(eventCode, $"Cannot Remove The Inexistent User {{ ID = {userID} }}");
			else
			{
				if (OnlineUserID_ServerID[selectedUser] != serviceID) _logger.LogError(eventCode, $"The User {{ ID = {userID} }} Has Conflict At Service ID : Stored Is `{OnlineUserID_ServerID[selectedUser]}` But Ordered `{serviceID}`");
				OnlineUserID_ServerID.Remove(selectedUser);
				_logger.LogInformation(eventCode, $"Removed Online User {{ ID = {userID} }}");
			}
		}

		public long SelectServerIDByUserID(long userID)
		{
			var selectedServerID = (from user_serverID in OnlineUserID_ServerID
									where user_serverID.Key == userID
									select user_serverID.Value).FirstOrDefault();
			return selectedServerID;
		}

	}
}