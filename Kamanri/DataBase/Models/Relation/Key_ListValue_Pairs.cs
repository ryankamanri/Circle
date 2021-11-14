using System.Collections.Generic;
using System.Linq;

namespace Kamanri.Database.Models.Relation
{
    /// <summary>
    /// 由KeyValuePair组成的list,同时具有list列表的所有功能,
    /// dictionary字典的根据键查找值功能 KeyIndex()
    /// </summary>
    /// <typeparam name="TKey">键</typeparam>
    /// <typeparam name="TValue">值</typeparam>
    public class Key_ListValue_Pairs<TKey,TValue> : List<KeyValuePair<TKey,IList<TValue>>>
    {

        public IList<TKey> Keys {get
        {
            var keys = new List<TKey>();
            foreach(var item in this)
            {
                keys.Add(item.Key);
            }
            return keys;
        }}
        /// <summary>
        /// 返回第一个key在list中的索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int KeyIndex(TKey key)
        {
            int count = this.Count;
            dynamic Key = key;
            for(int i = 0; i < count;i ++)
            {
                if(Key.Equals(this[i].Key)) return i;
            }
            return int.MinValue;
        }


        public Key_ListValue_Pairs<TKey, TValue> Union(Key_ListValue_Pairs<TKey, TValue> other)
        {
            if (this.Count == 0) return other;

            foreach (var thisItem in this)
            {
                dynamic ThisItem = thisItem;
                foreach (var otherItem in other)
                {
                    dynamic OtherItem = otherItem;
                    if (ThisItem.Key.Equals(OtherItem.Key))
                    {
                        thisItem.Value.Union(otherItem.Value);
                    }
                }
            }
            return this;
        }
    }
}