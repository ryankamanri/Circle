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