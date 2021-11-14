using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Kamanri.Extensions
{
    public static class ObjectExtention
    {
        /// <summary>
        /// 以字符串作为属性名访问对象的某一属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(this object obj,string property)
        {
            try
            {
                return (T)obj.GetType().GetProperty(property).GetValue(obj);
            }catch(Exception e)
            {
                throw new ObjectExtensionException($"Failed To Get The Property {property} From {obj.ToString()}", e);
            }
            
        }

        /// <summary>
        /// 将某一对象转换为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }catch(Exception e)
            {
                throw new ObjectExtensionException($"Failed To Serialized The Object {obj.ToString()}", e);
            }
            
        }

    }
}