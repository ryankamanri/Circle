using System;
using System.Collections.Generic;
using System.Text;

namespace Kamanri.Database.Models.Attributes
{
	/// <summary>
	/// 设置数据库实体列名, 如不设置默认为属性名
	/// </summary>
	public class ColumnName: Attribute
	{
		public string Name { get; }
		public ColumnName(string name)
		{
			Name = name;
		}
	}
}
