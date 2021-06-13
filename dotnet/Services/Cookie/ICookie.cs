using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dotnet.Services
{
    public interface ICookie
    {
        void SetCookie(HttpContext httpContext,ClaimsPrincipal claimsPrincipal);

        void DeleteCookie(HttpContext httpContext);

        ClaimsPrincipal GetCookie(HttpContext httpContext);
    }
}
