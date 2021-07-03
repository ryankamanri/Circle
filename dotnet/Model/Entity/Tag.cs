using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Services.Extensions;
using dotnet.Services.Database;



namespace dotnet.Model
{
    public class Tag : Entity<Tag>
    {
        public long ID { get; set; }
        public string _Tag { get; set; }

        public Tag(){}

        public Tag(string tag)
        {
            this.ID = RandomGenerator.GenerateID();
            this._Tag = tag;
        }
        public Tag(long ID,string tag)
        {
            this.ID = ID;
            this._Tag = tag;
        }

        public override string ToString()
        {
            return $"{{ \"ID\" : {ID} , \"Tag\" : \"{_Tag}\" }}";
        }


        public override async Task Insert(SQL sql)
        {
            await sql.Execute($"insert into tags (ID,tag) values({ID},'{_Tag}')");
        }

        public override async Task Delete(SQL sql)
        {
            await sql.Execute($"delete from tags where ID = {ID}");
        }

        public override async Task Update(SQL sql)
        {
            await sql.Execute($"update tags set tag = '{_Tag}' where ID = {ID}");
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

        public override Tag Select(SQL sql, long ID)
        {
            IList<Tag> result = GetList(sql.Query($"select * from tags where ID = '{ID}'"));
            if(result.Count == 0) return null;
            this.ID = result[0].ID;
            this._Tag = result[0]._Tag;
            return result[0];
        }

        public override long SelectID(SQL sql, Tag tag)
        {
            IList<Tag> result = GetList(sql.Query($"select * from tags where tag = '{tag._Tag}'"));
            if(result.Count == 0) return long.MinValue;
            this.ID = result[0].ID;
            return result[0].ID;
        }

        public override IList<Tag> Selects(SQL sql ,IEnumerable<long> IDs)
        {
            IList<Tag> Tags = new List<Tag>();
            Tag tag;
            foreach(var ID in IDs)
            {
                tag = Select(sql,ID);
                if(tag != null) Tags.Add(tag);
            }
            return Tags;
        }

        public override List<long> SelectIDs(SQL sql ,List<Tag> tags)
        {
            List<long> IDs = new List<long>();
            long ID;
            foreach(var tag in tags)
            {
                ID = SelectID(sql,tag);
                if(ID != long.MinValue) IDs.Add(SelectID(sql,tag));
            }
            return IDs;
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