using System;
using System.Collections.Generic;
using System.Data.Common;
using Kamanri.Database.Models;

namespace ChatServer.Models.User
{
	public class UserInfo : Entity<UserInfo>, IEqualityComparer<UserInfo>
	{
		/// <summary>
		/// 昵称
		/// </summary>
		/// <value>string</value>
		public string NickName { get; set; }

		/// <summary>
		/// 真实姓名
		/// </summary>
		/// <value>string</value>
		public string RealName { get; set; }


		/// <summary>
		/// 学校
		/// </summary>
		/// <value>string</value>
		public string University { get; set; }

		/// <summary>
		/// 学院
		/// </summary>
		/// <value>string</value>
		public string School { get; set; }

		/// <summary>
		/// 专业
		/// </summary>
		/// <value>string</value>
		public string Speciality { get; set; }

		/// <summary>
		/// 入学年份
		/// </summary>
		/// <value></value>
		public DateTime SchoolYear { get; set; }

		/// <summary>
		/// 个人简介
		/// </summary>
		/// <value></value>
		public string Introduction { get; set; }

		/// <summary>
		/// 头像(以base64编码保存)
		/// </summary>
		/// <value></value>
		public string HeadImage { get; set; }


		public override string TableName { get; set; } = "usersinfo";


		public UserInfo() { }

		public UserInfo(long ID) : base(ID) { }

		public UserInfo(string NickName, string RealName, string University, string School, string Speciality, DateTime SchoolYear, string Introduction, string HeadImage)
		{
			this.NickName = NickName;
			this.RealName = RealName;
			this.University = University;
			this.School = School;
			this.Speciality = Speciality;
			this.SchoolYear = SchoolYear;
			this.Introduction = Introduction;
			this.HeadImage = HeadImage;
		}

		public UserInfo(long ID, string NickName, string RealName, string University, string School, string Speciality, DateTime SchoolYear, string Introduction, string HeadImage) : base(ID)
		{
			this.NickName = NickName;
			this.RealName = RealName;
			this.University = University;
			this.School = School;
			this.Speciality = Speciality;
			this.SchoolYear = SchoolYear;
			this.Introduction = Introduction;
			this.HeadImage = HeadImage;
		}

		public override UserInfo GetEntityFromDataReader(DbDataReader ddr)
		{
			return new UserInfo((long)ddr["ID"], (string)ddr["NickName"], (string)ddr["RealName"], (string)ddr["University"], (string)ddr["school"], (string)ddr["Speciality"], DateTime.Parse((string)ddr["SchoolYear"]), (string)ddr["Introduction"], (string)ddr["HeadImage"]);
		}

		public override UserInfo GetEntity() => this;

		public bool Equals(UserInfo user_1, UserInfo user_2)
		{
			return user_1.ID == user_2.ID;
		}

		public int GetHashCode(UserInfo user)
		{
			return (int)user.ID;
		}


	}
}