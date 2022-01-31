using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Kamanri.Self
{
	public static class Dynamic
	{
		/// <summary>
		/// 使用第一个参数作为动态对象覆盖第二个参数
		/// </summary>
		/// <param name="object_1"></param>
		/// <param name="selections"></param>
		public static void Cover(dynamic object_1, dynamic object_2)
		{
			if (object_1 == null || object_2 == null) return;
			if (!(typeof(IDictionary<string, object>).IsAssignableFrom(object_1.GetType() as Type)))
				throw new InvalidOperationException($"Cannot Iterate The Object 1 Which Have NOT Inherited The Interface `IDictionary<string, object>`");
			if (!(typeof(IDictionary<string, object>).IsAssignableFrom(object_2.GetType() as Type)))
				throw new InvalidOperationException($"Cannot Iterate The Object 2 Which Have NOT Inherited The Interface `IDictionary<string, object>`");
			foreach (var option in object_1)
			{
				(object_2 as IDictionary<string, object>)[option.Key] = JTokenToObject(option.Value);
			}
		}

		public static dynamic JsonToObject(string json)
		{
			try
			{
				var jToken = JToken.Parse(json);
				return JTokenToObject(jToken);
			}catch(Exception e)
			{
				throw new InvalidCastException($"Failed To Convert The Json To Object : {json}", e);
			}
			
		}

		private static dynamic JTokenToObject(JToken jToken)
		{
			var type = jToken.Type;
			if(type == JTokenType.Null ||
			 type == JTokenType.None ||
			 type == JTokenType.Undefined) return default;
			
			else if(type == JTokenType.Array)
			{
				var list = new List<object>();
				foreach(var jItem in jToken as JArray)
				{
					list.Add(JTokenToObject(jItem));
				}
				return list;
			}

			else if(type == JTokenType.Object)
			{
				var obj = new Dictionary<string, object>();
				foreach(var jItem in jToken as JObject)
				{
					obj.Add(jItem.Key, JTokenToObject(jItem.Value));
				}
				return obj;
			}

			else
			{
				if(type == JTokenType.Integer) return (int)jToken;
				if(type == JTokenType.Float) return (float)jToken;
				if(type == JTokenType.String) return (string)jToken;
				if(type == JTokenType.Boolean) return (bool)jToken;
				if(type == JTokenType.Date) return (DateTime)jToken;
				if(type == JTokenType.Bytes) return (byte[])jToken;
				
				return default;
			}
		}
	}
}