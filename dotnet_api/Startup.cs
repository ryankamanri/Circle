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
using dotnetApi.Model;
using dotnetApi.Services;
using dotnetApi.Services.Database;

namespace dotnetApi
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
            

            services.AddCors(options => 
            {
                options.AddPolicy("dotnet",builder => builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true));
            });

            services.AddScoped<User>();


            //增加单例服务,数据库访问
            services.AddSingleton(new SQL(options =>
            {
                options.Server = "127.0.0.1";
                options.Port = "3306";
                options.Database = "dotnet_ubuntu";
                options.Uid = "root";
                options.Pwd = "123456";
            }));

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

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            };

            webSocketOptions.AllowedOrigins.Any();

            app.UseWebSockets(webSocketOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
