using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace dotnet.Model
{
    public class ID_IDList : List<ID_ID>
    {
        public enum OutPutType { Key, Value }

        public List<long> Find(long input, OutPutType type)
        {
            bool IsKey = (type == OutPutType.Key);
            List<long> target = new List<long>();
            ID_ID id_id =
            IsKey ? new ID_ID(0, input) : new ID_ID(input, 0);//如果输出的是key,则比较value是否相同,否则比较key

            int index =
            IsKey ? BinarySearch(0, Count, id_id, new Model.ValueComparer()) : BinarySearch(0, Count, id_id, new Model.KeyComparer());

            if (index < 0) return target;
            target.Add(IsKey ? this[index].ID : this[index].ID_2);

            if (IsKey)
            {
                for (int left = index - 1; (left >= 0) && (this[left].ID_2 == input); left--)
                    target.Add(this[left].ID);

                for (int right = index + 1; (right < this.Count) && (this[right].ID_2 == input); right++)
                    target.Add(this[right].ID);
            }
            else
            {
                for (int left = index - 1; (left >= 0) && (this[left].ID == input); left--)
                    target.Add(this[left].ID_2);

                for (int right = index + 1; (right < this.Count) && (this[right].ID == input); right++)
                    target.Add(this[right].ID_2);
            }


            return target;
        }

        /// <summary>
        /// 查找所有关系的交集
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<long> FindAndIntersect(List<long> inputs, OutPutType type)
        {
            if (inputs.Count == 0) return new List<long>();
            IEnumerable<long> target = Find(inputs[0], type);
            foreach (var input in inputs)
            {
                target = target.Intersect(Find(input, type));
            }
            return target;
        }

        /// <summary>
        /// 查找所有关系的并集
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<long> FindAndUnion(List<long> inputs, OutPutType type)
        {
            if (inputs.Count == 0) return new List<long>();
            IEnumerable<long> target = Find(inputs[0], type);
            foreach (var input in inputs)
            {
                target = target.Union(Find(input, type));
            }
            return target;
        }

        /// <summary>
        /// 查找所有关系的并集,并使用字典统计每一个输出实体id与输入实体id的所有关系
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Key_ListValue_Pairs<long,long> FindUnionStatistics(List<long> inputs, OutPutType type)
        {
            //以某些标签能够匹配到的其他用户为例
            if (inputs.Count == 0) return new Key_ListValue_Pairs<long, long>();

            Key_ListValue_Pairs<long, long> dictionary = new Key_ListValue_Pairs<long, long>();
            foreach(var input in inputs)//每一个input是一个标签的id
            {
                List<long> targets = Find(input,type);//找到拥有该标签的用户
                foreach(var target in targets)//每一个target是一个用户的id
                {
                    int index = dictionary.KeyIndex(target);
                    
                    if(index != int.MinValue)//如果字典中存在该帖子,则添加拥有该帖子的用户
                        dictionary[index].Value.Add(input);

                    else//如果不存在,则新建一个拥有该帖子的用户list

                        dictionary.Insert(0,new KeyValuePair<long, List<long>>(target,new List<long>(){input}));
                }
            }
            return dictionary;
        }
    }

    class KeyComparer : IComparer<ID_ID>, IEqualityComparer<ID_ID>
    {
        public int Compare(ID_ID id_id1, ID_ID id_id2)
        {
            return (int)(id_id1.ID - id_id2.ID);
        }
        public bool Equals(ID_ID id_id1, ID_ID id_id2)
        {
            if (id_id1.ID == id_id2.ID) return true;
            else return false;
        }

        public int GetHashCode(ID_ID id_id)
        {
            return 0;
        }
    }

    class ValueComparer : IComparer<ID_ID>, IEqualityComparer<ID_ID>
    {
        public int Compare(ID_ID id_id1, ID_ID id_id2)
        {
            return (int)(id_id1.ID_2 - id_id2.ID_2);
        }

        public bool Equals(ID_ID id_id1, ID_ID id_id2)
        {
            if (id_id1.ID_2 == id_id2.ID_2) return true;
            else return false;
        }

        public int GetHashCode(ID_ID id_id)
        {
            return 0;
        }
    }
}