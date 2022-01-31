namespace Kamanri.Database.Models
{
	/// <summary>
	/// 所有实体的抽象实体类
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public abstract class EntityObject
	{
		/// <summary>
		/// 实体ID
		/// </summary>
		/// <value></value>
		public long ID { get; set; }

		public EntityObject(){}
		public EntityObject(long ID)
		{
			this.ID = ID;
		}
	}
}