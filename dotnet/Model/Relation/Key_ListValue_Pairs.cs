using System.Collections.Generic;
using System.Linq;

namespace dotnet.Model
{
    public class Key_ListValue_Pairs<TKey,TValue> : List<KeyValuePair<TKey,List<TValue>>>
    {
        /// <summary>
        /// 返回第一个key在list中的索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int KeyIndex(TKey key)
        {
            int count = this.Count;
            for(int i = 0; i < count;i ++)
            {
                if(this[i].Key.Equals(key)) return i;
            }
            return int.MinValue;
        }
    }
}