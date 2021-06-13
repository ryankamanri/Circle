using MySql.Data.MySqlClient;
using System.Collections.Generic;
using dotnet.Services;
using System.Threading.Tasks;


namespace dotnet.Model
{
    public class Tag : IDataOperation
    {
        public long ID { get; set; }
        public string _Tag { get; set; }

        public Tag(){}
        public Tag(long ID,string tag)
        {
            this.ID = ID;
            this._Tag = tag;
        }

        public override string ToString()
        {
            return $"{ID}  {_Tag} ";
        }


        public async Task Save(SQL sql)
        {
            await sql.Execute($"insert into tags (ID,tag) values({ID},'{_Tag}')");
        }

        public async Task Remove(SQL sql)
        {
            await sql.Execute($"delete from tags where ID = {ID}");
        }

        public async Task Modify(SQL sql)
        {
            await sql.Execute($"update tags set tag = '{_Tag}' where ID = {ID}");
        }

        public static IList<Tag> GetList(MySqlDataReader msdr)
        {
            IList<Tag> Tags = new List<Tag>();
            while (msdr.Read())
            {
                Tags.Add(new Tag((long)msdr["ID"], (string)msdr["tag"]));
            }
            msdr.Close();
            return Tags;
        }

        public static Tag Find(SQL sql, long ID)
        {
            IList<Tag> result = GetList(sql.Query($"select * from tags where ID = '{ID}'"));
            if(result.Count == 0) return null;
            return result[0];
        }

        public static long FindID(SQL sql, string tag)
        {
            IList<Tag> result = GetList(sql.Query($"select * from tags where tag = '{tag}'"));
            if(result.Count == 0) return long.MinValue;
            return result[0].ID;
        }

        public static IList<Tag> Finds(SQL sql ,IEnumerable<long> IDs)
        {
            IList<Tag> Tags = new List<Tag>();
            Tag tag;
            foreach(var ID in IDs)
            {
                tag = Find(sql,ID);
                if(tag != null) Tags.Add(tag);
            }
            return Tags;
        }

        public static List<long> FindIDs(SQL sql ,List<string> tags)
        {
            List<long> IDs = new List<long>();
            long ID;
            foreach(var tag in tags)
            {
                ID = FindID(sql,tag);
                if(ID != long.MinValue) IDs.Add(FindID(sql,tag));
            }
            return IDs;
        }
    }
}