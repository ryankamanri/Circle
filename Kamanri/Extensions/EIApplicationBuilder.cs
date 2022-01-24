using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Kamanri.Middlewares;

namespace Kamanri.Extensions
{
    public static class EIApplicationBuilder
    {
        public static Microsoft.AspNetCore.Builder.IApplicationBuilder UseKamanriWebSocket(this Microsoft.AspNetCore.Builder.IApplicationBuilder app)
        {
            app.UseMiddleware<WebSocketMiddleware>();
            return app;
        }
    }
}
