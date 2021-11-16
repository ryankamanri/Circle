namespace Kamanri.Database.Models
{
    /// <summary>
    /// 所有对应到视图实体的抽象实体类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityView
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        /// <value></value>
        public long ID { get; set; }

        public EntityView(){}
        public EntityView(long ID)
        {
            this.ID = ID;
        }
    }
}