using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using MySql.Data.MySqlClient;
using Kamanri.Database.Models;

namespace dotnetPrivateChatApi.Models
{

    public class Message : Entity<Message>, IComparer<Message>
    {
        public long SendUserID { get; set; }
        public long ReceiveID { get; set; }

        public bool IsGroup { get; set; } = false;
        public DateTime Time { get; set; }
        public string ContentType { get; set; }
        public object Content;


        public override string TableName { get ; set ; } = "message";
        
        public Message(){}

        public Message(long ID) : base(ID){}

        public Message(long sendUserID, long receiveID, bool isGroup, DateTime time, string MessageContentType, object content)
        {
            SendUserID = sendUserID;
            ReceiveID = receiveID;
            IsGroup = isGroup;
            Time = time;
            ContentType = MessageContentType;
            Content = content;
        }

        public Message(long ID ,long sendUserID, long receiveID, bool isGroup, DateTime time, string MessageContentType, object content) : base(ID)
        {
            SendUserID = sendUserID;
            ReceiveID = receiveID;
            IsGroup = isGroup;
            Time = time;
            Time = time;
            ContentType = MessageContentType;
            Content = content;
        }

        public override string ColumnNamesString() => 
        $"{TableName}.SendUserID,{TableName}.ReceiveID, {TableName}.IsGroup, {TableName}.Time,{TableName}.ContentType,{TableName}.Content";

        public override string InsertValuesString() =>
        $"{SendUserID}, {ReceiveID}, {IsGroup}, '{Time.ToString()}', '{ContentType}', @Content";


        public override string UpdateSetString() => throw new NotImplementedException();


        public override string CandidateKeySelectionString() =>
        $"{TableName}.SendUserID = {SendUserID} and {TableName}.ReceiveID = {ReceiveID} and {TableName}.Time = {Time.ToString()}";


        public override ICollection<DbParameter> SetParameter(DbCommand command)
        {
            var param = command.CreateParameter();
            param.DbType = System.Data.DbType.Binary;
            param.ParameterName = "@Content";
            param.Value = Content;
            return new List<DbParameter>(){param};
        }

        public override Message GetEntityFromDataReader(DbDataReader msdr) => 
            new Message(
                (long)msdr["ID"],
                (long)msdr["SendUserID"],
                (long)msdr["ReceiveID"],
                (bool)msdr["IsGroup"],
                (DateTime)msdr["Time"],
                (string)msdr["ContentType"],
                (byte[])msdr["Content"]);

        public override Message GetEntity()
        {
            return this;
        }

        public int Compare(Message message_1, Message message_2) => 
            (message_1.Time - message_2.Time).Milliseconds;
    }


    public static class MessageContentType
    {
        public const string Text = "text/plain";
        public const string Gif = "image/gif";
        public const string Jpg = "image/jpeg";
        public const string Png = "image/png";

    }


}