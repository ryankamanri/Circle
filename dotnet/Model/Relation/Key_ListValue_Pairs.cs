using System.Collections.Generic;
using System.Linq;

namespace dotnet.Model.Relation
{
    /// <summary>
    /// 由KeyValuePair组成的list,同时具有list列表的所有功能,
    /// dictionary字典的根据键查找值功能 KeyIndex()
    /// </summary>
    /// <typeparam name="TKey">键</typeparam>
    /// <typeparam name="TValue">值</typeparam>
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