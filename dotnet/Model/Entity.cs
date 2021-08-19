using System.Threading.Tasks;
using System;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json;
using dotnet.Services.Self;

namespace dotnet.Model
{
    public abstract class Entity<TEntity>
    {
        
        /// <summary>
        /// 实体ID
        /// </summary>
        /// <value></value>
        public long ID { get; set; }

        public Entity(){}
        public Entity(long ID)
        {
            this.ID = ID;
        }

        /// <summary>
        /// 实体对应的数据库表名
        /// </summary>
        /// <value></value>
        public abstract string TableName {get; set;}


    }
}