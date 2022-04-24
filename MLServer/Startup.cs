using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kamanri.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MLServer.Middlewares;
using MLServer.Services;

namespace MLServer
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
			services.AddSingleton<IMLService, MLService>();
			services.AddSingleton<IUserService, UserService>();
			services.AddKamanriDataBase(options =>
			{
				options.Server = Configuration["SQL:Server"];
				options.Port = Configuration["SQL:Port"];
				options.Database = Configuration["SQL:Database"];
				options.Uid = Configuration["SQL:Uid"];
				options.Pwd = Configuration["SQL:Pwd"];
			}, option => new MySql.Data.MySqlClient.MySqlConnection(option));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseMiddleware<MLServiceMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
