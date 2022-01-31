using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Kamanri.Extensions;
using Kamanri.Database;
using Kamanri.WebSockets;
using ChatDataServer.Services;
using ChatDataServer.Middlewares;

namespace ChatDataServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();

			// var loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();

			//增加单例服务,数据库访问

			services.AddKamanriDataBase(options =>
			{
				options.Server = Configuration["SQL:Server"];
				options.Port = Configuration["SQL:Port"];
				options.Database = Configuration["SQL:Database"];
				options.Uid = Configuration["SQL:Uid"];
				options.Pwd = Configuration["SQL:Pwd"];

			}, options => new MySql.Data.MySqlClient.MySqlConnection(options));

			services.AddKamanriWebSocket().AddSingleton<OnMessageService>();

			services.AddSingleton<MessageService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			var webSocketOptions = new WebSocketOptions()
			{
				KeepAliveInterval = TimeSpan.FromSeconds(120)
			};

			webSocketOptions.AllowedOrigins.Any();

			app.UseWebSockets(webSocketOptions);

			app.UseKamanriWebSocket();

			app.UseMiddleware<OnMessageMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
