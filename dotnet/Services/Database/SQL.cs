using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace dotnet.Services.Database
{
    /// <summary>
    /// 数据库访问类
    /// </summary>
    public class SQL
    {
        
        public class MySqlConnectOptions
        {
            public string Server { get; set; }
            public string Port { get; set; }
            public string Database { get; set; }
            public string Uid { get; set; }
            public string Pwd { get; set; }

            public override string ToString()
            {
                return $"Server={Server};Port={Port};Database={Database};Uid={Uid};Pwd={Pwd};";
            }
        }


        private MySqlConnectOptions _options;

        private MySqlConnection connection;

        
        

        /// <summary>
        /// 对数据库访问服务的初始化
        /// </summary>
        /// <param name="SetOptions">一个匿名函数,用于设置数据库连接字符串</param>
        public SQL(Action<MySqlConnectOptions> SetOptions)
        {
            _options = new MySqlConnectOptions();
            SetOptions(_options);

            try
            {
                connection = new MySqlConnection(_options.ToString());
                connection.Open();
                
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }


        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            try
            {
                connection.Close();

            }catch (Exception e)
            {
                throw e;
            }
            
        }

        /// <summary>
        /// 执行查询语句
        /// 返回值MySqlDataReader可以经过模型中对应的实体的GetList方法处理成一个IList,
        /// </summary>
        /// <param name="expression">SQL表达式</param>
        /// <returns></returns>
        public MySqlDataReader Query(string expression)
        {
            try
            {
                using MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"{expression}";
                return command.ExecuteReader();
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
            

        }

        /// <summary>
        ///  执行非查询语句
        /// </summary>
        /// <param name="expression">SQL表达式</param>
        public async Task Execute(string expression)
        {
            try
            {
                using MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"{expression}";
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }
    }
}
