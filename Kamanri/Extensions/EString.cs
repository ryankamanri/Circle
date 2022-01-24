using System;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
namespace Kamanri.Extensions
{
    public static class EString
    {
        /// <summary>
        /// 将某一Json字符串转换为 T 类型的对象实例
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }catch(Exception e)
            {
                throw new StringExtensionException($"Failed To Deserialized Json To Object : {json}", e);
            }
        }

        /// <summary>
        /// 将某一Json字符串转换为 T 类型的对象实例
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this StringValues json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }catch(Exception e)
            {
                throw new StringExtensionException($"Failed To Deserialized Json To Object : {json}", e);
            }
        }

        /// <summary>
        /// 将字符串转换为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string str)
        {
            try
            {
                byte[] originBytes = Encoding.UTF8.GetBytes(str);
                return originBytes;
            }catch(Exception e)
            {
                throw new StringExtensionException($"Failed To Deserialized String To ByteArray : {str}", e);
            }
            
        }
    }
}