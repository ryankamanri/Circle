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

        public class SqlConnectOptions
        {
            public string Server { get; set; }
            public string Port { get; set; }
            public string Database { get; set; }
            public string Uid { get; set; }
            public string Pwd { get; set; }

            public int NumberOfConnections { get; set; } = 3;

            public override string ToString()
            {
                return $"Server={Server};Port={Port};Database={Database};Uid={Uid};Pwd={Pwd};";
            }
        }


        private SqlConnectOptions options;

        public List<KeyValuePair<MySqlConnection,Mutex>> connectionPool;

        private int currentConnectionNumber;

        private const string defaultExpression = "select * from tags where ID = -1";

        /// <summary>
        /// 对数据库访问服务的初始化
        /// </summary>
        /// <param name="SetOptions">一个匿名函数,用于设置数据库连接字符串</param>
        public SQL(Action<SqlConnectOptions> SetOptions)
        {
            options = new SqlConnectOptions();
            SetOptions(options);

            try
            {

                //
                connectionPool = new List<KeyValuePair<MySqlConnection,Mutex>>();
                for(int i = 0;i < options.NumberOfConnections; i++)
                {
                    var conn = new MySqlConnection(options.ToString());
                    conn.Open();
                    connectionPool.Add(new KeyValuePair<MySqlConnection,Mutex>(conn,new Mutex(5)));
                }
                KeepSession();
            }
            catch (Exception e)
            {
                throw e;
            }

            
        }

        private void KeepSession()
        {
            new Task(() =>
            {
                while (true)
                {
                    foreach (var connectionAndMutex in connectionPool)
                    {
                        try
                        {
                            connectionAndMutex.Value.Wait().Wait();
                            using (MySqlCommand command = connectionAndMutex.Key.CreateCommand())
                            {
                                command.CommandText = defaultExpression;
                                command.ExecuteReader().Close();
                                connectionAndMutex.Value.Signal();
                            }
                        }
                        catch (Exception e)
                        {
                            connectionAndMutex.Value.Signal();
                            throw e;
                        }
                    }
                    System.Threading.Thread.Sleep(4 * 60 * 60 * 1000);
                }
            }).Start();

        }

        /// <summary>
        /// 连接池中分配空闲连接.
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<MySqlConnection,Mutex> AssignConnection()
        {
            for(int i = 0;i < options.NumberOfConnections; i++)
            {
                if(connectionPool[currentConnectionNumber].Value.mutex == false)
                {
                    Console.WriteLine($"Current Connection Number : {currentConnectionNumber}");
                    return connectionPool[currentConnectionNumber];
                }
                currentConnectionNumber++;
                currentConnectionNumber %= options.NumberOfConnections;
            }
            Console.WriteLine($"Current Connection Number : {currentConnectionNumber},All Connections Are Full Used");
            return connectionPool[currentConnectionNumber];
            
        }



        /// <summary>
        /// 执行查询语句
        /// 返回值MySqlDataReader可以经过模型中对应的实体的GetList方法处理成一个IList,
        /// </summary>
        /// <param name="expression">SQL表达式</param>
        /// <returns></returns>
        public async Task<KeyValuePair<MySqlDataReader,Mutex>> Query(string expression)
        {
            var connectionAndMutex = AssignConnection();
            try
            {
                await connectionAndMutex.Value.Wait();
                using (MySqlCommand command = connectionAndMutex.Key.CreateCommand())
                {
                    command.CommandText = expression;
                    return new KeyValuePair<MySqlDataReader,Mutex>(command.ExecuteReader(),connectionAndMutex.Value);
                }
            }
            catch (Exception e)
            {
                connectionAndMutex.Value.Signal();
                throw e;
            }


        }

        /// <summary>
        ///  执行非查询语句
        /// </summary>
        /// <param name="expression">SQL表达式</param>
        public async Task Execute(string expression)
        {
            var connectionAndMutex = AssignConnection();
            try
            {
                using (MySqlCommand command = connectionAndMutex.Key.CreateCommand())
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
