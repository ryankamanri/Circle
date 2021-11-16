using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Kamanri.Middlewares;

namespace Kamanri.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseKamanriWebSocket(this IApplicationBuilder app)
        {
            app.UseMiddleware<WebSocketMiddleware>();
            return app;
        }
    }
}
