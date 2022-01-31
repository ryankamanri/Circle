using System;
using System.Collections.Generic;
using Kamanri.Database.Models;

namespace ChatServer.Models
{

	public class Message : EntityObject, IComparer<Message>, IEqualityComparer<Message>
	{
		public long SendUserID { get; set; }
		public long ReceiveID { get; set; }

		public bool IsGroup { get; set; } = false;
		public DateTime Time { get; set; }
		public string ContentType { get; set; }
		public object Content { get; set; }


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