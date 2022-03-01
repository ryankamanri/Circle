using System.Collections.Generic;
using System.IO;
using Kamanri.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using WebViewServer.Middlewares;
using WebViewServer.Models.User;
using WebViewServer.Services;
using WebViewServer.Services.Cookie;

namespace WebViewServer
{
	public class Startup
	{

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
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

			services.AddWebOptimizer(pipeline =>
			{
				//pipeline.MinifyJsFiles("/js/**/*");
				pipeline.MinifyCssFiles("/css/**/*");
				pipeline.AddCssBundle("/bundle.css", "/css/Global.css", "/css/Home/**/*", "/css/Shared/**/*");
			});
			//增加(SystemLogIn)的认证服务,如果没有这个Cookie将无法进入主页面
			services.AddAuthentication("CookiesAuth").AddCookie("CookiesAuth", options =>
			 {
				 options.Cookie.Name = "SystemLogInCookie";
				 options.LoginPath = "/LogIn";
			 });

			//配置跨域访问
			services.AddCors(options =>
			{
				options.AddPolicy("any", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			});

			//添加跨域api访问
			services.AddSingleton(new Api(Configuration["Api"]));



			services.AddScoped<User>();
			//增加Cookie服务
			services.AddSingleton<ICookie, Cookie>();

			//增加字典服务,用于注册验证,保存cookie
			services.AddSingleton<Dictionary<string, string>>();

			//增加标签索引,匹配服务
			services.AddSingleton<TagService>();

			//增加用户服务
			services.AddSingleton<UserService>();

			services.AddSingleton<SearchService>();

			services.AddSingleton<PostService>();


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
			//打包资源文件
			app.UseWebOptimizer();
			//可访问静态文件,位于wwwroot/
			app.UseStaticFiles();

			app.UseStaticFiles(new StaticFileOptions()
			{
				RequestPath = new PathString("/StaticFiles"),
				ServeUnknownFileTypes = true,
				FileProvider = new PhysicalFileProvider(
					Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "StaticFiles")),
			});
			//使用认证服务
			app.UseAuthentication();
			//使用授权服务
			app.UseAuthorization();
			//允许跨域访问
			app.UseCors("any");
			//获取用户登录信息
			app.UseMiddleware<UserMiddleware>();
			
			//定位到对应的服务
			app.UseEndpoints(endpoints =>
			{

				endpoints.MapControllers();

				endpoints.MapDefaultControllerRoute();

			});
		}
	}
}
