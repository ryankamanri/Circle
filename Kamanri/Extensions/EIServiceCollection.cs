using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using Kamanri.Database;
using Kamanri.WebSockets;
namespace Kamanri.Extensions
{
    public static class EIServiceCollection
    {
        /// <summary>
        /// 添加实体访问数据库服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="SetOptions">设定数据库连接属性</param>
        /// <param name="SetConnection">设定数据库连接类型</param>
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddKamanriDataBase(this Microsoft.Extensions.DependencyInjection.IServiceCollection services,
        Action<SQL.SqlConnectOptions> SetOptions,
         Func<string, System.Data.Common.DbConnection> SetConnection)
        {
            services.AddSingleton(new SQL(
                SetOptions , 
                SetConnection,
                services.BuildServiceProvider().GetService<ILoggerFactory>()));

            services.AddSingleton<DataBaseContext>();
            return services;
        }

        /// <summary>
        /// 添加WebSocket服务, 可接受多个客户端的连接
        /// </summary>
        /// <param name="services"></param>
        /// <param name="SetEventHandlers">设置 WebSocketMessageService 和 WebSocketClient</param>

        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddKamanriWebSocket(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            services.AddSingleton<IWebSocketMessageService, WebSocketMessageService>()
                    .AddSingleton<IWebSocketSession, WebSocketSession>();

            return services;
        }


    }
}