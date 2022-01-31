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
	public abstract class Entity<TEntity> : EntityObject, IEquatable<Entity<TEntity>>
	{

		public Entity()
		{
			EntityAttributesString = new EntityAttributesString<TEntity>(this);
		}
		public Entity(long ID) : base(ID)
		{
			EntityAttributesString = new EntityAttributesString<TEntity>(this);
		}

		/// <summary>
		/// 实体对应的数据库表名
		/// </summary>
		/// <value></value>
		[JsonIgnore]
		public abstract string TableName { get; set; }

		[JsonIgnore]
		public EntityAttributesString<TEntity> EntityAttributesString;

		/// <summary>
		/// 实体对应的数据库表所有列名
		/// </summary>
		/// <value></value>
		public string Columns()
		{
			EntityAttributesString.Build();
			return $"{TableName}.ID, {EntityAttributesString.ColumnNamesString}";
		}

		/// <summary>
		/// 从数据库容器中读取实体
		/// </summary>
		/// <param name="ddr"></param>
		/// <returns></returns>
		public abstract TEntity GetEntityFromDataReader(DbDataReader ddr);

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
		/// <param name="ddr"></param>
		/// <returns></returns>
		public async Task<IList<TEntity>> GetList(KeyValuePair<DbDataReader,Mutex> ddr_mutex)
		{
			IList<TEntity> entities = new List<TEntity>();
			while (ddr_mutex.Key.Read())
			{
				try
				{
					var entity = GetEntityFromDataReader(ddr_mutex.Key);
					(entity as Entity<TEntity>).ID = (long)ddr_mutex.Key["ID"];
					entities.Add(entity);
				}catch(Exception e)
				{
					throw new DataBaseModelException("Failed To Get Entity From Data Reader", e);
				}
				
			}
			await ddr_mutex.Key.CloseAsync();
			ddr_mutex.Value.Signal();
			return entities;
		}

		/// <summary>
		/// 读取从数据库中连接表获取的实体信息与对应关系,形成一个Dictionary列表
		/// </summary>
		/// <param name="ddr"></param>
		/// <returns></returns>
		public async Task<IDictionary<TEntity,dynamic>> GetRelationDictionary(KeyValuePair<DbDataReader,Mutex> ddr_mutex)
		{
			IDictionary<TEntity,dynamic> entity_Relations = new Dictionary<TEntity,dynamic>();
			while (ddr_mutex.Key.Read())
			{
				try
				{
					var entity = GetEntityFromDataReader(ddr_mutex.Key);
					(entity as Entity<TEntity>).ID = (long)ddr_mutex.Key["ID"];
					entity_Relations.Add(entity, 
					((string)ddr_mutex.Key["relations"]).ToObject<ExpandoObject>());
				}catch(Exception e)
				{
					throw new DataBaseModelException($"Failed To Get Entity From Data Reader Or Deserialize The ExpandoObject ddr_mutex.Key['relations'] {(string)ddr_mutex.Key["relations"]}", e);
				}
				
			}
			await ddr_mutex.Key.CloseAsync();
			ddr_mutex.Value.Signal();
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