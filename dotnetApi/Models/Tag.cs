
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Dynamic;
using System.Data.Common;
using Newtonsoft.Json;
using Kamanri.Database.Models;



namespace dotnetApi.Models
{
    public class Tag : Entity<Tag>,IEqualityComparer<Tag>
    {
        
        public string _Tag { get; set; }

        public override string TableName { get ; set ;} = "tags";

        public override string ColumnNamesString() => $"{TableName}.tag";


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



        public override string ToString()
        {
            return $"{ID},'{_Tag}'";
        }

        public override string InsertValuesString()
        {
            return $"'{_Tag}'";
        }

        public override string UpdateSetString()
        {
            return $"{TableName}.tag = '{_Tag}'";
        }

        public override string CandidateKeySelectionString()
        {
            return $"{TableName}.tag = '{_Tag}'";
        }
        

        public override Tag GetEntityFromDataReader(DbDataReader msdr)
        {
            return new Tag((long)msdr["ID"], (string)msdr["tag"]);
        }

        public override Tag GetEntity()
        {
            return this;
        }

        public bool Equals(Tag tag_1,Tag tag_2)
        {
            return (tag_1.ID == tag_2.ID);
        }

        public int GetHashCode(Tag tag)
        {
            return (int)tag.ID;
        }


        /// <summary>
        /// 获取该标签的连续子集
        /// </summary>
        /// <returns></returns>
        public List<string> GetContinuousSubset()
        {
            List<string> subset = new List<string>();
            string subItem,subItemUpper,subItemLower;
            //从长度为1开始截取
            for(int i = 1;i <= _Tag.Length;i++)
            {
                //起始位置为j
                for(int j = 0;j + i <= _Tag.Length;j++)
                {
                    subItem = _Tag.Substring(j,i);
                    subItemUpper = subItem.ToUpper();
                    subItemLower = subItem.ToLower();
                    
                    if(!subset.Contains(subItem))
                        subset.Add(subItem);
                    if(!subset.Contains(subItemUpper))
                        subset.Add(subItemUpper);
                    if(!subset.Contains(subItemLower))
                        subset.Add(subItemLower);
                }
            }
            return subset;
        }

    }
}