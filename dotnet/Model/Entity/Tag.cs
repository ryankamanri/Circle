
using System.Collections.Generic;




namespace dotnet.Model
{
    public class Tag : Entity<Tag>,IEqualityComparer<Tag>
    {
        
        public string _Tag { get; set; }

        public override string TableName { get ; set ;} = "tags";



        public Tag()
        {

        }

        public Tag(long ID) : base(ID)
        {

        }

        public Tag(string tag)
        {
            this._Tag = tag;

        }
        public Tag(long ID,string tag) : base(ID)
        {
            this._Tag = tag;
        }



   
        public bool Equals(Tag tag_1,Tag tag_2)
        {
            return (tag_1.ID == tag_2.ID);
        }

        public int GetHashCode(Tag tag)
        {
            return (int)tag.ID;
        }



    }
}