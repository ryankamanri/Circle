using System.Collections.Generic;
using System.Data.Common;
using Kamanri.Database.Models;
using Kamanri.Database.Models.Attributes;

namespace MLServer.Models.BasicModels
{
	public class Tag : Entity<Tag>, IEqualityComparer<Tag>
	{
		[ColumnName("tag")]
		public string _Tag { get; set; }

		public override string TableName { get; set; } = "tags";


		public Tag()
		{

		}

		public Tag(long ID) : base(ID)
		{

		}

		public Tag(string tag)
		{
			_Tag = tag;

		}
		public Tag(long ID, string tag) : base(ID)
		{
			_Tag = tag;
		}



		public override string ToString()
		{
			return $"{ID},'{_Tag}'";
		}


		public override Tag GetEntityFromDataReader(DbDataReader ddr)
		{
			return new Tag((long)ddr["ID"], (string)ddr["tag"]);
		}

		public override Tag GetEntity()
		{
			return this;
		}

		public bool Equals(Tag tag_1, Tag tag_2)
		{
			return tag_1.ID == tag_2.ID;
		}

		public int GetHashCode(Tag tag)
		{
			return (int)tag.ID;
		}


		/// <summary>
		/// 获取该标签的连续子集
		/// </summary>
		/// <returns></returns>
		public List<string> GetContinuousSubset()
		{
			List<string> subset = new List<string>();
			string subItem, subItemUpper, subItemLower;
			//从长度为1开始截取
			for (int i = 1; i <= _Tag.Length; i++)
			{
				//起始位置为j
				for (int j = 0; j + i <= _Tag.Length; j++)
				{
					subItem = _Tag.Substring(j, i);
					subItemUpper = subItem.ToUpper();
					subItemLower = subItem.ToLower();

					if (!subset.Contains(subItem))
						subset.Add(subItem);
					if (!subset.Contains(subItemUpper))
						subset.Add(subItemUpper);
					if (!subset.Contains(subItemLower))
						subset.Add(subItemLower);
				}
			}
			return subset;
		}

	}
}