using System.Collections.Generic;


namespace Kamanri.Database.Models.Relation
{
    /// <summary>
    /// 实体之间关系类ID_ID的list集合
    /// </summary>
    public class ID_IDList : List<ID_ID>
    {
        public enum OutPutType { Key, Value }


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