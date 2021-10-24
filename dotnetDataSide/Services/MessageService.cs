using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using dotnetDataSide.Model;
using Kamanri.Database;

namespace dotnetDataSide.Services
{
    public class MessageService
    {

        private DataBaseContext _dbc;

        private ILogger<MessageService> _logger;

        public ICollection<Message> messages;

        public MessageService(DataBaseContext dbc, ILoggerFactory loggerFactory)
        {
            _dbc = dbc;
            _logger = loggerFactory.CreateLogger<MessageService>();
            messages = new List<Message>();
        }



        public IEnumerable<Message> SelectTempMessagesOfAUser(int eventCode, User user)
        {
            var selectedMessages = from messageItem in messages
            where messageItem.ReceiveID == user.ID 
            select messageItem;
            _logger.LogInformation(eventCode, $"Selected {selectedMessages.Count()} Temporary Messages Of User {user.ToString()}");
            return selectedMessages;
        }

        public async Task<bool> AppendMessage(int eventCode, Message message)
        {
            messages.Add(message);
            await _dbc.Insert<Message>(message);
            _logger.LogInformation(eventCode, $"Append A Message, Which Type : {message.ContentType}");
            return true;
        }
    }
}