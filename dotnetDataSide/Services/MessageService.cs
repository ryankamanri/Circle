using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using dotnetDataSide.Models;
using Kamanri.Database;

namespace dotnetDataSide.Services
{
    public class MessageService
    {

        private DataBaseContext _dbc;

        private ILogger<MessageService> _logger;

        public IDictionary<long, IList<Message>> receiveID_messages;

        public MessageService(DataBaseContext dbc, ILoggerFactory loggerFactory)
        {
            _dbc = dbc;
            _logger = loggerFactory.CreateLogger<MessageService>();
            receiveID_messages = new Dictionary<long, IList<Message>>();
        }



        public IList<Message> SelectTempMessagesOfAUser(int eventCode, long userID)
        {
            IList<Message> selectedMessages;
            if (!receiveID_messages.TryGetValue(userID, out selectedMessages))
            {
                selectedMessages = new List<Message>();
            }
            _logger.LogInformation(eventCode, $"Selected {selectedMessages.Count()} Temporary Messages Of User {userID.ToString()}");

            receiveID_messages[userID] = new List<Message>();
            return selectedMessages;
        }

        public async Task<IEnumerable<Message>> SelectPreviousMessagesOfClientUser(int eventCode, long clientUserID, long requestUserID )
        {
            var receivedMessages = await _dbc.SelectCustom<Message>($"{Message.RECEIVE_ID} = {clientUserID} and {Message.SEND_USER_ID} = {requestUserID}");
            var sendedMessages = await _dbc.SelectCustom<Message>($"{Message.SEND_USER_ID} = {clientUserID} and {Message.RECEIVE_ID} = {requestUserID}");
            var allMessages = receivedMessages.Union(sendedMessages, new Message());
            _logger.LogInformation(eventCode, $"Select {allMessages.Count()} Messages Of User {clientUserID} With Request User {requestUserID}");
            return allMessages;
            
        }

        public bool AppendTemporaryMessage(int eventCode, Message message)
        {
            IList<Message> selectedMessages;
            if (!receiveID_messages.TryGetValue(message.ReceiveID, out selectedMessages))
                receiveID_messages[message.ReceiveID] = new List<Message>();
            receiveID_messages[message.ReceiveID].Add(message);
            _logger.LogInformation(eventCode, $"Append A Temporary Message, Which Type : {message.ContentType}");
            return true;
        }

        public async Task<bool> AppendConstantMessage(int eventCode, Message message)
        {
            
            await _dbc.Insert(message);
            _logger.LogInformation(eventCode, $"Append A Constant Message, Which Type : {message.ContentType}");
            return true;
        }
    }
}