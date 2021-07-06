using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json;
using dotnet.Services.Extensions;
using dotnet.Services.Database;



namespace dotnet.Model
{
    public class Tag : Entity<Tag>
    {
        
        public string _Tag { get; set; }

        private void Init()
        {
            TableName = "tags";

            Columns  = "tags.ID,tag";

            ColumnsWithoutID = "tag";
        }

        public Tag()
        {
            Init();
        }

        public Tag(long ID)
        {
            this.ID = ID;
            Init();
        }

        public Tag(string tag)
        {
            this._Tag = tag;
            Init();
        }
        public Tag(long ID,string tag)
        {
            this.ID = ID;
            this._Tag = tag;
            Init();
        }



        public override string ToString()
        {
            return $"{ID},'{_Tag}'";
        }

        public override string InsertString()
        {
            return $"'{_Tag}'";
        }

        public override string UpdateString()
        {
            return $"tag = '{_Tag}'";
        }

        public override string SelectString()
        {
            return $"tag = '{_Tag}'";
        }
        

        public override IList<Tag> GetList(MySqlDataReader msdr)
        {
            IList<Tag> Tags = new List<Tag>();
            while (msdr.Read())
            {
                Tags.Add(new Tag((long)msdr["ID"], (string)msdr["tag"]));
            }
            msdr.Close();
            return Tags;
        }

        public override IDictionary<Tag,dynamic> GetRelationDictionary(MySqlDataReader msdr)
        {
            IDictionary<Tag,dynamic> Tag_Relations = new Dictionary<Tag,dynamic>();
            while (msdr.Read())
            {

                Tag_Relations.Add(new Tag((long)msdr["ID"], (string)msdr["tag"]),
                JsonConvert.DeserializeObject<ExpandoObject>((string)msdr["relations"]));
            }
            msdr.Close();
            return Tag_Relations;
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