using MySql.Data.MySqlClient;
using System.Collections.Generic;
using dotnet.Services;


namespace dotnet.Model
{
    public class Tag
    {
        public long ID { get; set; }
        public string _Tag { get; set; }

        public Tag(long ID,string tag)
        {
            this.ID = ID;
            this._Tag = tag;
        }

        public override string ToString()
        {
            return $"{ID}  {_Tag} ";
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
            return GetList(sql.Query($"select * from tags where ID = '{ID}'"))[0];
        }

        public static long FindID(SQL sql, string tag)
        {
            return GetList(sql.Query($"select * from tags where tag = '{tag}'"))[0].ID;
        }

        public static IList<Tag> Finds(SQL sql ,IEnumerable<long> IDs)
        {
            IList<Tag> Tags = new List<Tag>();
            foreach(var ID in IDs)
            {
                Tags.Add(Find(sql,ID));
            }
            return Tags;
        }

        public static List<long> FindIDs(SQL sql ,List<string> tags)
        {
            List<long> IDs = new List<long>();
            foreach(var tag in tags)
            {
                IDs.Add(FindID(sql,tag));
            }
            return IDs;
        }
    }
}