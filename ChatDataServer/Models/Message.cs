using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using MySql.Data.MySqlClient;
using Kamanri.Database.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using Kamanri.Database.Models.Attributes;

namespace ChatDataServer.Models
{

	public class Message : Entity<Message>, IComparer<Message>, IEqualityComparer<Message>
	{
		public static string SEND_USER_ID = "SendUserID";
		public static string RECEIVE_ID = "ReceiveID";
		public long SendUserID { get; set; }
		public long ReceiveID { get; set; }

		public bool IsGroup { get; set; } = false;
		public DateTime Time { get; set; }
		public string ContentType { get; set; }

		[CandidateKeyIgnore]
		[ParameterPlaceHolder("@Content")]
		public object Content { get; set; }


		public override string TableName { get; set; } = "messages";

		public Message() { }

		public Message(long ID) : base(ID) { }

		public Message(long sendUserID, long receiveID, bool isGroup, DateTime time, string MessageContentType, object content)
		{
			SendUserID = sendUserID;
			ReceiveID = receiveID;
			IsGroup = isGroup;
			Time = time;
			ContentType = MessageContentType;
			Content = content;
		}

		public Message(long ID, long sendUserID, long receiveID, bool isGroup, DateTime time, string MessageContentType, object content) : base(ID)
		{
			SendUserID = sendUserID;
			ReceiveID = receiveID;
			IsGroup = isGroup;
			Time = time;
			Time = time;
			ContentType = MessageContentType;
			Content = content;
		}


		public override ICollection<DbParameter> SetParameter(DbCommand command)
		{
			var param = command.CreateParameter();
			param.DbType = System.Data.DbType.Binary;
			param.ParameterName = "@Content";
			param.Value = Content;
			return new List<DbParameter>() { param };
		}

		public override Message GetEntityFromDataReader(DbDataReader ddr) =>
			new Message(
				(long)ddr["ID"],
				(long)ddr["SendUserID"],
				(long)ddr["ReceiveID"],
				(bool)ddr["IsGroup"],
				(DateTime)ddr["Time"],
				(string)ddr["ContentType"],
				(byte[])ddr["Content"]);

		public override Message GetEntity()
		{
			return this;
		}

		public int Compare(Message message_1, Message message_2) =>
			(message_1.Time - message_2.Time).Milliseconds;

		public bool Equals(Message x, Message y)
		{
			return x.ID == y.ID;
		}

		public int GetHashCode(Message obj)
		{
			return (int)obj.ID;
		}
	}


	public static class MessageContentType
	{
		public const string Text = "text/plain";
		public const string Gif = "image/gif";
		public const string Jpg = "image/jpeg";
		public const string Png = "image/png";

	}


}