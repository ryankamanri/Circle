using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Kamanri.Self
{
    public static class JsonDynamic
    {
        /// <summary>
        /// 将字典中的项填充至selection中
        /// </summary>
        /// <param name="options"></param>
        /// <param name="selections"></param>
        public static void FillSelections(Dictionary<string, object> options,dynamic selections)
        {
            foreach(var option in options)
            {
                if(option.Value.GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                    ((IDictionary<string,object>)selections)[option.Key] = ((Newtonsoft.Json.Linq.JArray)option.Value).ToObject<IEnumerable<object>>();
                else ((IDictionary<string,object>)selections)[option.Key] = option.Value;
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
                throw new InvalidCastException($"Failed To Convert The Json To Object : {json} \n Caused By : ", e);
            }
            
        }

        public static dynamic JTokenToObject(JToken jToken)
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