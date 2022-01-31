using System;
using System.Collections.Generic;
using System.Text;

namespace Kamanri.Database.Models.Attributes
{
	/// <summary>
	/// 表示SQL语句中该列的占位符
	/// </summary>
	public class ParameterPlaceHolder: Attribute
	{
		public string PlaceHolder { get; }
		public ParameterPlaceHolder(string placeHolder)
		{
			PlaceHolder = placeHolder;
		}
	}
}
