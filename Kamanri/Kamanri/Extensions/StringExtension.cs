using System;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
namespace Kamanri.Extensions
{
    public static class StringExtension
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
                throw e;
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
                throw e;
            }
        }

        /// <summary>
        /// 将字符串转换为字节数组, 该数组将以 0 结尾
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string str)
        {
            
            byte[] originBytes = Encoding.UTF8.GetBytes(str);
            //byte[] sourceBytes = new byte[originBytes.Length + 1];
            //originBytes.CopyTo(sourceBytes, 0);
            return originBytes;
        }
    }
}