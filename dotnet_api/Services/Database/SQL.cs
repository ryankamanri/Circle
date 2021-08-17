using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using dotnetApi.Services.Self;

namespace dotnetApi.Services.Database
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


        private MySqlConnectOptions options;

        private MySqlConnection connection;

        public Mutex dataReaderMutex;




        /// <summary>
        /// 对数据库访问服务的初始化
        /// </summary>
        /// <param name="SetOptions">一个匿名函数,用于设置数据库连接字符串</param>
        public SQL(Action<MySqlConnectOptions> SetOptions)
        {
            options = new MySqlConnectOptions();
            SetOptions(options);

            try
            {
                connection = new MySqlConnection(options.ToString());
                connection.Open();
                KeepSession();
                dataReaderMutex = new Mutex();
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }

            
        }

        private void KeepSession()
        {
            Task sendMessageAtInterval = new Task(() => 
            {
                while(true)
                {
                    var result = Query("select * from tags where ID = -1").Result;
                    result.Key.Close();
                    result.Value.Signal();
                    System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                }
            });
            sendMessageAtInterval.Start();
            
            
        }


        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            try
            {
                connection.Close();

            }
            catch (Exception e)
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
        public async Task<KeyValuePair<MySqlDataReader,Mutex>> Query(string expression)
        {
            try
            {
                if(connection.State == System.Data.ConnectionState.Closed) connection.Open();
                await dataReaderMutex.Wait();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = expression;
                    return new KeyValuePair<MySqlDataReader,Mutex>(command.ExecuteReader(),dataReaderMutex);
                }
            }
            catch (Exception e)
            {
                dataReaderMutex.Signal();
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
                if(connection.State == System.Data.ConnectionState.Closed) connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = expression;
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
