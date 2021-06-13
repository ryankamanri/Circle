using System;
using System.Text;
namespace dotnet.Services
{
    public static class Base64
    {
        /// <summary>
        /// Base64 UTF-8编码
        /// </summary>
        /// <param name="source">要编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            string result = "";
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }


        /// <summary>
        /// Base64 UTF-8解码
        /// </summary>
        /// <param name="source">要解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string DecodeBase64(string source)
        {
            string result = "";
            byte[] bytes = Convert.FromBase64String(source);
            try
            {
                result = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }
    }
}