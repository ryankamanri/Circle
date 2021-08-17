using System.Threading.Tasks;
using System;
using System.Dynamic;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using dotnet.Services.Self;
using dotnet.Services.Database;

namespace dotnet.Model
{
    public abstract class Entity<TEntity> : IEquatable<Entity<TEntity>>
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

        /// <summary>
        /// 实体对应的数据库表所有列名
        /// </summary>
        /// <value></value>
        public string Columns => $"{TableName}.ID,{ColumnsWithoutID}";

        /// <summary>
        /// 实体对应的数据库表所有列名,不计入ID
        /// 格式 {TableName}.Property_1,{TableName}.Property_2
        /// </summary>
        /// <value></value>
        public abstract string ColumnsWithoutID {get;} 
       
        /// <summary>
        /// 插入数据库时需要的字符串,包括所有属性的值,以逗号分隔
        /// 格式 {Property_1},{Property_2}
        /// </summary>
        /// <returns></returns>
        public abstract string InsertString();

        /// <summary>
        /// 更新数据库时需要的字符串,包括除ID外所有属性的列名和值,以逗号分隔
        /// 格式 {TableName}.Property_1 = {Property_1},{TableName}.Property_2 = {Property_2}
        /// </summary>
        /// <returns></returns>
        public abstract string UpdateString();

        /// <summary>
        /// 查询数据库时需要的字符串,包括除ID外任一候选码的列名和值,以and/or分隔
        /// 格式 {TableName}.Property_1 = {Property_1} and/or {TableName}.Property_2 = {Property_2}
        /// </summary>
        /// <returns></returns>
        public abstract string SelectString();

        /// <summary>
        /// 从数据库容器中读取实体
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public abstract TEntity GetEntityFromDataReader(MySqlDataReader msdr);


        /// <summary>
        /// 读取从数据库中获取的实体信息,形成一个list列表
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public IList<TEntity> GetList(KeyValuePair<MySqlDataReader,Mutex> msdr_mutex)
        {
            IList<TEntity> entities = new List<TEntity>();
            while (msdr_mutex.Key.Read())
            {
                entities.Add(GetEntityFromDataReader(msdr_mutex.Key));
            }
            msdr_mutex.Key.Close();
            msdr_mutex.Value.Signal();
            return entities;
        }

        /// <summary>
        /// 读取从数据库中连接表获取的实体信息与对应关系,形成一个Dictionary列表
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public IDictionary<TEntity,dynamic> GetRelationDictionary(KeyValuePair<MySqlDataReader,Mutex> msdr_mutex)
        {
            IDictionary<TEntity,dynamic> entity_Relations = new Dictionary<TEntity,dynamic>();
            while (msdr_mutex.Key.Read())
            {
                entity_Relations.Add(GetEntityFromDataReader(msdr_mutex.Key),
                JsonConvert.DeserializeObject<ExpandoObject>((string)msdr_mutex.Key["relations"]));
            }
            msdr_mutex.Key.Close();
            msdr_mutex.Value.Signal();
            return entity_Relations;
        }


        public bool Equals(Entity<TEntity> other)
        {
            return (this.ID == other.ID);
        }


        public override int GetHashCode()
        {
            return (int)this.ID;
        }


    }
}