using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace dotnet.Services
{
    public class Scheme : IAuthenticationHandler
    {

        public AuthenticationScheme AScheme { get; private set; }
        protected HttpContext Context { get; private set; }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            AScheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 认证
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            return AuthenticateResult.NoResult();
        }

        /// <summary>
        /// 没有登录 要求 登录 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 没权限
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        public static string Serialize(AuthenticationTicket ticket)
        {

            //需要引入  Microsoft.AspNetCore.Authentication

            byte[] byteTicket = TicketSerializer.Default.Serialize(ticket);
            return System.Text.Encoding.Default.GetString(byteTicket);
        }

        public static AuthenticationTicket Deserialize(string content)
        {
            byte[] byteTicket = System.Text.Encoding.Default.GetBytes(content);
            return TicketSerializer.Default.Deserialize(byteTicket);
        }

    }
}
