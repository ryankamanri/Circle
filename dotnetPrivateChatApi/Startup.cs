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
using dotnetPrivateChatApi.Services;
using Kamanri.Extensions;
using Kamanri.Database;
using Kamanri.WebSockets;

namespace dotnetPrivateChatApi
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

            

            //services.AddSingleton<ILoggerFactory,LoggerFactory>();

            loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();

            services.AddControllersWithViews();

            services.AddCors(options => 
            {
                options.AddPolicy("dotnet",builder => 
                builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true));
            });

            // var webSocketMessageService = new WebSocketMessageService(loggerFactory);

            // var webSocketClient = new WebSocketClient(Configuration["WebSocket:URL"], webSocketMessageService, loggerFactory);

            // services.AddWebSocket(webSocketMessageService, webSocketClient, loggerFactory);

            services.AddWebSocket(Configuration["WebSocket:URL"], 
            (wsm, wsClient) => services.AddSingleton(new OnMessageService(wsm, wsClient, loggerFactory)));

            // services.AddSingleton(new OnMessageService(services.BuildServiceProvider()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_privatechat_api v1"));
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
