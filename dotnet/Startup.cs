using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using dotnet.Services;
using dotnet.Services.Cookie;
using dotnet.Services.Database;

namespace dotnet
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //ConfigureServices方法添加需要的服务
        //添加的服务由服务器容器代理,可以通过构造函数中的参数获取实例或接口
        public void ConfigureServices(IServiceCollection services)
        {
            //增加控制器服务
            services.AddControllers();
            //增加控制器与视图(观察者模式)服务
            services.AddControllersWithViews();
            //增加(SystemLogIn)的认证服务,如果没有这个Cookie将无法进入主页面
            services.AddAuthentication("CookiesAuth").AddCookie("CookiesAuth",options => 
            {
                options.Cookie.Name = "SystemLogInCookie";
                options.LoginPath = "/LogIn";
            });

            //增加作用域Cookie服务
            services.AddScoped<ICookie, Cookie>();

            //增加字典服务,用于注册验证
            services.AddSingleton<Dictionary<string,string>>();

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

            //配置跨域访问
            services.AddCors(options => 
            {
                options.AddPolicy("any", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //Configure方法添加中间件,接受的http请求或做出的响应在获得服务之前将逐一通过这些中间件
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //使用路由
            app.UseRouting();
            //可访问静态文件,位于wwwroot/
            app.UseStaticFiles();
            //使用认证服务
            app.UseAuthentication();
            //使用授权服务
            app.UseAuthorization();
            //允许跨域访问
            app.UseCors("any");
            //定位到对应的服务
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();

                endpoints.MapDefaultControllerRoute();
                
            });
        }
    }
}
