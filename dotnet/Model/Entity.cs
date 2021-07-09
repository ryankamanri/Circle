using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using dotnet.Services.Database;

namespace dotnet.Model
{
    public abstract class Entity<T>
    {
        
        /// <summary>
        /// 实体ID
        /// </summary>
        /// <value></value>
        public long ID { get; set; }

        /// <summary>
        /// 实体对应的数据库表名
        /// </summary>
        /// <value></value>
        public string TableName {get;protected set;}

        /// <summary>
        /// 实体对应的数据库表所有列名
        /// </summary>
        /// <value></value>
        public string Columns{get; protected set;}

        /// <summary>
        /// 实体对应的数据库表所有列名,不计入ID
        /// </summary>
        /// <value></value>
        public string ColumnsWithoutID {get; protected set;}
       
        /// <summary>
        /// 插入数据库时需要的字符串,包括除ID外所有属性的值,以逗号分隔
        /// </summary>
        /// <returns></returns>
        public abstract string InsertString();

        /// <summary>
        /// 更新数据库时需要的字符串,包括除ID外所有属性的列名和值,以逗号分隔
        /// </summary>
        /// <returns></returns>
        public abstract string UpdateString();

        /// <summary>
        /// 查询数据库时需要的字符串,包括除ID外所有属性的列名和值,以and/or分隔
        /// </summary>
        /// <returns></returns>
        public abstract string SelectString();


        /// <summary>
        /// 读取从数据库中获取的实体信息,形成一个list列表
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public abstract IList<T> GetList(MySqlDataReader msdr);

        public abstract IDictionary<T,dynamic> GetRelationDictionary(MySqlDataReader msdr);


        public override bool Equals(object obj)
        {
            dynamic Obj = obj;
            return this.ID == Obj.ID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }
}