using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using dotnet.Services.Self;



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