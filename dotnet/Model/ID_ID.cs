using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace dotnet.Model
{
    public class ID_ID
    {
        public long ID { get; set; }
        public long ID_2 { get; set; }

        public ID_ID(long ID,long ID_2)
        {
            this.ID = ID;
            this.ID_2 = ID_2;
        }

        public override string ToString()
        {
            return $"{ID}  {ID_2} ";
        }

        public static ID_IDList GetList(MySqlDataReader msdr)
        {
            ID_IDList ID_IDs = new ID_IDList();
            while (msdr.Read())
            {
                ID_IDs.Add(new ID_ID((long)msdr[0], (long)msdr[1]));
            }
            msdr.Close();
            return ID_IDs;
        }
    }
}