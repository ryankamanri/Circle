using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using dotnet.Services.Database;

namespace dotnet.Model
{
    public abstract class Entity<T>
    {
        /// <summary>
        /// 将模型实体保存到数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract Task Insert(SQL sql);

        /// <summary>
        /// 将模型实体从数据库移除
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract Task Delete(SQL sql);

        /// <summary>
        /// 更新模型实体
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract Task Update(SQL sql);

        /// <summary>
        /// 读取从数据库中获取的实体信息,形成一个list列表
        /// </summary>
        /// <param name="msdr"></param>
        /// <returns></returns>
        public abstract IList<T> GetList(MySqlDataReader msdr);

        /// <summary>
        /// 根据ID选择实体,并保存在实例中
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract T Select(SQL sql,long id);

        /// <summary>
        /// 根据实体中的候选码选择ID,并保存在实例中
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract long SelectID(SQL sql,T entity);

        /// <summary>
        /// 根据提供的ID列表选择实体列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public abstract IList<T> Selects(SQL sql,IEnumerable<long> IDs);

        /// <summary>
        /// 根据提供的实体列表选择ID列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public abstract List<long> SelectIDs(SQL sql,List<T> entities);
    }
}