using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using dotnetApi.Models;
using dotnetApi.Services;
using Kamanri.Database;

namespace dotnetApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILoggerFactory loggerFactory;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            
            services.AddSingleton<ILoggerFactory,LoggerFactory>();

            loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();

            services.AddCors(options => 
            {
                options.AddPolicy("dotnet",builder => 
                builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true));
            });

            services.AddScoped<User>();




            //增加单例服务,数据库访问
            services.AddSingleton(new SQL(options =>
            {
                options.Server = Configuration["SQL:Server"];
                options.Port = Configuration["SQL:Port"];
                options.Database = Configuration["SQL:Database"];
                options.Uid = Configuration["SQL:Uid"];
                options.Pwd = Configuration["SQL:Pwd"];

            }, options => new MySql.Data.MySqlClient.MySqlConnection(options)
            ,loggerFactory));

            //增加数据库上下文服务
            services.AddSingleton<DataBaseContext>();

            //增加标签索引,匹配服务
            services.AddSingleton<TagService>();

            //增加用户服务
            services.AddSingleton<UserService>();

            services.AddSingleton<SearchService>();

            services.AddSingleton<PostService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
