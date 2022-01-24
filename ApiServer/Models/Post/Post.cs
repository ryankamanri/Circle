using System;
using System.Data.Common;
using System.Collections.Generic;
using Kamanri.Database.Models;

namespace ApiServer.Models.Post
{
    public class Post : Entity<Post>, IEqualityComparer<Post>
    {
        /// <summary>
        /// 标题
        /// </summary>
        /// <value>string</value>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        /// <value>string</value>
        public string Summary { get; set; }

        /// <summary>
        /// 聚焦
        /// </summary>
        /// <value>string</value>
        public string Focus { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        /// <value>DateTime</value>
        public DateTime PostDateTime { get; set; }



        public override string TableName { get; set; } = "posts";
        public override string ColumnNamesString() => $"{TableName}.Title,{TableName}.Summary,{TableName}.Focus,{TableName}.PostDateTime";

        public Post()
        {

        }

        public Post(long ID) : base(ID)
        {

        }

        public Post(string Title, string Summary, string Focus, DateTime PostDateTime)
        {
            this.Title = Title;
            this.Summary = Summary;
            this.Focus = Focus;
            this.PostDateTime = PostDateTime;
        }
        public Post(long ID, string Title, string Summary, string Focus, DateTime PostDateTime) : base(ID)
        {
            this.Title = Title;
            this.Summary = Summary;
            this.Focus = Focus;
            this.PostDateTime = PostDateTime;
        }

        public override string InsertValuesString()
        {
            return $"'{Title}','{Summary}','{Focus}','{PostDateTime.ToString()}'";
        }

        public override string UpdateSetString()
        {
            return $"{TableName}.Title = '{Title}',{TableName}.Summary = '{Summary}',{TableName}.Focus = '{Focus}',{TableName}.PostDateTime = '{PostDateTime.ToString()}'";
        }

        public override string CandidateKeySelectionString()
        {
            return $"{TableName}.Title = '{Title}' and {TableName}.Summary = '{Summary}' and {TableName}.Focus = '{Focus}' and {TableName}.PostDateTime = '{PostDateTime.ToString()}'";
        }


        public override Post GetEntityFromDataReader(DbDataReader msdr)
        {
            return new Post((long)msdr["ID"], (string)msdr["Title"], (string)msdr["Summary"], (string)msdr["Focus"], DateTime.Parse((string)msdr["PostDateTime"]));
        }

        public override Post GetEntity()
        {
            return this;
        }

        public bool Equals(Post post_1, Post post_2)
        {
            return post_1.ID == post_2.ID;
        }

        public int GetHashCode(Post post)
        {
            return (int)post.ID;
        }


    }
}
