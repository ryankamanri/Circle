using System.Threading.Tasks;
using System;
using System.Data.Common;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json;
using Kamanri.Self;
using Kamanri.Extensions;

namespace Kamanri.Database.Models
{
    /// <summary>
    /// 所有对应到数据库表实体的抽象实体类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [JsonObject(MemberSerialization.OptOut)]
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
        [JsonIgnore]
        public abstract string TableName{get; set;}

        /// <summary>
        /// 实体对应的数据库表所有列名
        /// </summary>
        /// <value></value>
        public string Columns() => $"{TableName}.ID, {ColumnNamesString()}";

        /// <summary>
        /// 实体对应的数据库表所有列名,不计入ID
        /// 格式 {TableName}.Property_1,{TableName}.Property_2
        /// </summary>
        /// <value></value>
        public abstract string ColumnNamesString();
       
        /// <summary>
        /// 插入数据库时需要的字符串,包括所有属性的值,以逗号分隔
        /// 格式 {Property_1},{Property_2}
        /// </summary>
        /// <returns></returns>
        public abstract string InsertValuesString();

        /// <summary>
        /// 更新数据库时需要的字符串,包括除ID外所有属性的列名和值,以逗号分隔
        /// 格式 {TableName}.Property_1 = {Property_1},{TableName}.Property_2 = {Property_2}
        /// </summary>
        /// <returns></returns>
        public abstract string UpdateSetString();

        /// <summary>
        /// 查询数据库时需要的字符串,包括除ID外任一候选码的列名和值,以and/or分隔
        /// 格式 {TableName}.Property_1 = {Property_1} and/or {TableName}.Property_2 = {Property_2}
        /// </summary>
        /// <returns></returns>
        public abstract string CandidateKeySelectionString();

        /// <summary>
        /// 从数据库容器中读取实体
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public abstract TEntity GetEntityFromDataReader(DbDataReader msdr);

        /// <summary>
        /// 从本实体抽象类的类实例中获取实体
        /// </summary>
        /// <returns></returns>
        public abstract TEntity GetEntity();

        /// <summary>
        /// 为SQL语句中的占位符设置参数(一般用于二进制数据)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual ICollection<DbParameter> SetParameter(DbCommand command) => default;




        /// <summary>
        /// 读取从数据库中获取的实体信息,形成一个list列表
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetList(KeyValuePair<DbDataReader,Mutex> msdr_mutex)
        {
            IList<TEntity> entities = new List<TEntity>();
            while (msdr_mutex.Key.Read())
            {
                try
                {
                    entities.Add(GetEntityFromDataReader(msdr_mutex.Key));
                }catch(Exception e)
                {
                    throw new DataBaseModelException("Failed To Get Entity From Data Reader", e);
                }
                
            }
            await msdr_mutex.Key.CloseAsync();
            msdr_mutex.Value.Signal();
            return entities;
        }

        /// <summary>
        /// 读取从数据库中连接表获取的实体信息与对应关系,形成一个Dictionary列表
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public async Task<IDictionary<TEntity,dynamic>> GetRelationDictionary(KeyValuePair<DbDataReader,Mutex> msdr_mutex)
        {
            IDictionary<TEntity,dynamic> entity_Relations = new Dictionary<TEntity,dynamic>();
            while (msdr_mutex.Key.Read())
            {
                try
                {
                    entity_Relations.Add(GetEntityFromDataReader(msdr_mutex.Key),
                    ((string)msdr_mutex.Key["relations"]).ToObject<ExpandoObject>());
                }catch(Exception e)
                {
                    throw new DataBaseModelException($"Failed To Get Entity From Data Reader Or Deserialize The ExpandoObject msdr_mutex.Key['relations'] {(string)msdr_mutex.Key["relations"]}", e);
                }
                
            }
            await msdr_mutex.Key.CloseAsync();
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