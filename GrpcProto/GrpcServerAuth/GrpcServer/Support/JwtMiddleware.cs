using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CafeLib.Authorization.Tokens;
using GrpcServer.Extensions;
using GrpcServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GrpcServer.Support
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        private readonly Dictionary<string, int> _idMap = new Dictionary<string, int>
        {
            {"ChrisSolutions", 1}
        };

        public JwtMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _appSettings = config.GetAppSettings();
        }

        public async Task Invoke(HttpContext context, IAccountService accountService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, accountService, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IAccountService accountService, string token)
        {
            var authToken = new Token(token);
            var result = authToken.TryValidate(_appSettings.Secret, out var response);
            if (result == false) return;  // Can throw exception here as well.
            var userId = _idMap[response.Token.Claims.Single(x => x.Key == ClaimTypes.Name).Value];

            // attach user to context on successful jwt validation
            context.Items["User"] = accountService.GetById(userId);
        }
    }
}