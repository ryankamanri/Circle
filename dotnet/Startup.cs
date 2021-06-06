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
                options.LoginPath = "/";
            });

            services.AddScoped<ICookie, Cookie>();

            //增加单例服务,数据库访问
            services.AddSingleton(new SQL(options =>
            {
                options.Server = "47.108.205.96";
                options.Port = "4306";
                options.Database = "schema1";
                options.Uid = "root";
                options.Pwd = "123456";
            }));
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
            //访问静态文件,位于wwwroot/
            app.UseStaticFiles();
            //使用认证服务
            app.UseAuthentication();
            //使用授权服务
            app.UseAuthorization();
            //定位到对应的服务
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();

                endpoints.MapDefaultControllerRoute();
                
            });
        }
    }
}
