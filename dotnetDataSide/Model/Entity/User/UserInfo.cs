using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Kamanri.Database.Model;

namespace dotnetDataSide.Model
{
    public class UserInfo : Entity<UserInfo>,IEqualityComparer<UserInfo>
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
        public string HeadImage{ get; set; }




        public override string TableName { get; set; } = "usersinfo";

        public override string ColumnsWithoutID() => 
        $"{TableName}.NickName,{TableName}.RealName,{TableName}.University,{TableName}.School,{TableName}.Speciality,{TableName}.SchoolYear,{TableName}.Introduction,{TableName}.HeadImage";




        public UserInfo(){}

        public UserInfo(long ID) : base(ID){}

        public UserInfo(string NickName,string RealName,string University,string School,string Speciality,DateTime SchoolYear,string Introduction,string HeadImage)
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

        public UserInfo(long ID,string NickName,string RealName,string University,string School,string Speciality,DateTime SchoolYear,string Introduction,string HeadImage) : base(ID)
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

        public override string InsertString()
        {
            return $"{ID},'{NickName}','{RealName}','{University}','{School}','{Speciality}','{SchoolYear.ToString()}','{Introduction}','{HeadImage}'";
        }

        public override string UpdateString()
        {
            return $"{TableName}.NickName = '{NickName}',{TableName}.RealName = '{RealName}',{TableName}.University = '{University}',{TableName}.School = '{School}',{TableName}.Speciality = '{Speciality}',{TableName}.SchoolYear = '{SchoolYear.ToString()}',{TableName}.Introduction = '{Introduction}',{TableName}.HeadImage = '{HeadImage}'";
        }

        public override string SelectString()
        {
            return $"{TableName}.NickName = '{NickName}' and {TableName}.RealName = '{RealName}' and {TableName}.University = '{University}' and {TableName}.School = '{School}' and {TableName}.Speciality = '{Speciality}' and {TableName}.SchoolYear = '{SchoolYear}'";
        }

        public override UserInfo GetEntityFromDataReader(DbDataReader msdr)
        {
            return new UserInfo((long)msdr["ID"],(string)msdr["NickName"],(string)msdr["RealName"],(string)msdr["University"],(string)msdr["school"],(string)msdr["Speciality"],DateTime.Parse((string)msdr["SchoolYear"]),(string)msdr["Introduction"],(string)msdr["HeadImage"]);
        }

        public bool Equals(UserInfo user_1,UserInfo user_2)
        {
            return user_1.ID == user_2.ID;
        }

        public int GetHashCode(UserInfo user)
        {
            return (int)user.ID;
        }


    }
}