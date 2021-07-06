using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using dotnet.Services.Database;



namespace dotnet.Model.Relation
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

        
        public ID_IDList GetList(MySqlDataReader msdr)
        {
            ID_IDList ID_IDs = new ID_IDList();
            while (msdr.Read())
            {
                ID_IDs.Add(new ID_ID((long)msdr[0], (long)msdr[1],(string)msdr["relations"]));
            }
            msdr.Close();
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