using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using dotnetApi.Services.Self;
using dotnetApi.Services.Database;



namespace dotnetApi.Model.Relation
{
    /// <summary>
    /// 实体ID与实体ID的关系类,表示两个实体之间的关系
    /// </summary>
    public class ID_ID : IEquatable<ID_ID>
    {
        public long ID { get; set; }
        public long ID_2 { get; set; }

        public ExpandoObject Relations{ get; set;}

        public ID_ID()
        {
            Relations = new ExpandoObject();
        }

        public ID_ID(long ID,long ID_2,string relationsJSON)
        {
            this.ID = ID;
            this.ID_2 = ID_2;
            Relations = JsonConvert.DeserializeObject<ExpandoObject>(relationsJSON);
        }

        public ID_ID(long ID,long ID_2,Action<ExpandoObject> SetRelations)
        {
            this.ID = ID;
            this.ID_2 = ID_2;
            Relations = new ExpandoObject();
            SetRelations(Relations);
        }


        public override string ToString()
        {
            return $"{ID}  {ID_2} ";
        }

        
        public ID_IDList GetList(KeyValuePair<MySqlDataReader,Mutex> msdr_mutex)
        {
            ID_IDList ID_IDs = new ID_IDList();
            while (msdr_mutex.Key.Read())
            {
                ID_IDs.Add(new ID_ID((long)msdr_mutex.Key[0], (long)msdr_mutex.Key[1],(string)msdr_mutex.Key["relations"]));
            }
            msdr_mutex.Key.Close();
            msdr_mutex.Value.Signal();
            return ID_IDs;
        }

            // override object.Equals
        public bool Equals(ID_ID other)
        {
            return (this.ID == other.ID && this.ID_2 == other.ID_2);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            //throw new System.NotImplementedException();
            return base.GetHashCode();
        }
    }
}