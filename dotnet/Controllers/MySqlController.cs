using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace dotnet
{
    [Route("MySql/")]
    [Controller]
    public class CookieController : Controller
    {
        [HttpGet]
        [Route("Hello")]
        public string Hello()
        {
            return "MySql Hello";
        }
        [HttpGet]
        [Route("sql")]
        public string SqlTest()
        {
            string result = "";
            string server = "192.168.1.104,3306";
            string database = "schema1";
            string uid = "root";
            string pwd = "123456";
            string connstr = $"Server={server};Database={database};Uid={uid};Pwd={pwd};";
            MySqlConnection mySqlConnection = new MySqlConnection(connstr);
            try
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = "select * from schema1.table1";
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while(mySqlDataReader.Read()) result += mySqlDataReader["name"];
                return result;
            }catch(Exception e)
            {
                mySqlConnection.Close();
                return e.ToString();
            }
            
        }
    }
}