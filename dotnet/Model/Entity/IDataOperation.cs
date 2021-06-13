using System.Threading.Tasks;

namespace dotnet.Services
{
    interface IDataOperation
    {
        /// <summary>
        /// 将模型实体保存到数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task Save(SQL sql);

        /// <summary>
        /// 将模型实体从数据库移除
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task Remove(SQL sql);

        /// <summary>
        /// 更新模型实体
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task Modify(SQL sql);
    }
}