using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Text;
using Kamanri.Extensions;
using Kamanri.Database.Models.Attributes;

namespace Kamanri.Database.Models
{
	/// <summary>
	/// 实体属性字符串
	/// 当实体类有属性发生改变时, 需要重新调用Build()
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public sealed class EntityAttributesString<TEntity>
	{
		private readonly Entity<TEntity> _entity;
		public EntityAttributesString(Entity<TEntity> entity)
		{
			_entity = entity;
		}
		/// <summary>
		/// 查询数据库时需要的字符串,包括除ID外任一候选码的列名和值,以and/or分隔
		/// 格式 {TableName}.Property_1 = {Property_1} and/or {TableName}.Property_2 = {Property_2}
		/// </summary>
		/// <returns></returns>
		public string CandidateKeySelectionString { get; private set; } = default;
		/// <summary>
		/// 实体对应的数据库表所有列名,不计入ID
		/// 格式 {TableName}.Property_1,{TableName}.Property_2
		/// </summary>
		/// <value></value>
		public string ColumnNamesString { get; private set; } = default;
		/// <summary>
		/// 插入数据库时需要的字符串,包括所有属性的值,以逗号分隔
		/// 格式 {Property_1},{Property_2}
		/// </summary>
		/// <returns></returns>
		public string InsertValuesString { get; private set; } = default;
		/// <summary>
		/// 更新数据库时需要的字符串,包括除ID外所有属性的列名和值,以逗号分隔
		/// 格式 {TableName}.Property_1 = {Property_1},{TableName}.Property_2 = {Property_2}
		/// </summary>
		/// <returns></returns>
		public string UpdateSetString { get; private set; } = default;

		public bool IsBuilded { get; private set; } = false;
		public EntityAttributesString<TEntity> Build()
		{
			if(IsBuilded) return this;

			var tableName = _entity.TableName;
			var properties = _entity.GetType().GetProperties();
			foreach(var property in properties)
			{
				if (property.Name == "TableName" || property.Name == "ID") continue;

				var columnName = property.Name;
				var columnNameAttribute = property.GetCustomAttribute(typeof(ColumnName), true);
				if(columnNameAttribute != null) columnName = (columnNameAttribute as ColumnName).Name;

				var isCandidateKeyIgnore = false;
				var CandidateKeyIgnoreAttribute = property.GetCustomAttribute(typeof(CandidateKeyIgnore), true);
				if(CandidateKeyIgnoreAttribute != null) isCandidateKeyIgnore = true;

				var placeHolder = _entity.Get(property.Name);
				if (placeHolder == null) placeHolder = "";
				if (placeHolder.GetType() == typeof(string)) placeHolder = $"'{(placeHolder as string).FitDBText()}'";
				if (placeHolder.GetType() == typeof(DateTime)) placeHolder = $"'{placeHolder}'";
				var placeHolderAttribute = property.GetCustomAttribute(typeof(ParameterPlaceHolder), true);
				if (placeHolderAttribute != null) placeHolder = (placeHolderAttribute as ParameterPlaceHolder).PlaceHolder;
				

				if (!isCandidateKeyIgnore)
				{
					if(CandidateKeySelectionString == default) CandidateKeySelectionString = $"{tableName}.{columnName} = {placeHolder}";
					else CandidateKeySelectionString += $" and {tableName}.{columnName} = {placeHolder}";
					
				}

				if(ColumnNamesString == default) ColumnNamesString = $"{tableName}.{columnName}";
				else ColumnNamesString += $", {tableName}.{columnName}";

				if (InsertValuesString == default) InsertValuesString = $"{placeHolder}";
				else InsertValuesString += $", {placeHolder}";

				if (UpdateSetString == default) UpdateSetString = $"{tableName}.{columnName} = {placeHolder}";
				else UpdateSetString += $", {tableName}.{columnName} = {placeHolder}";

			}
			IsBuilded = true;
			return this;
		}

		public void NotifyAttributedChanged()
		{
			IsBuilded = false;
		}
	}
}
