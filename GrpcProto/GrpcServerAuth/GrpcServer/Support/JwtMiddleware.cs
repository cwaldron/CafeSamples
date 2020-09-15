using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CafeLib.Authorization.Tokens;
using GrpcServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GrpcServer.Support
{
    public class JwtMiddleware
    {
        private const string Secret = "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING";

        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        private readonly Dictionary<string, int> _idMap = new Dictionary<string, int>
        {
            {"ChrisSolutions", 1}
        };

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
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
            var result = authToken.TryValidate(Secret, out var response);
            if (result == false) return;  // Can throw exception here as well.
            var userId = _idMap[response.Token.Claims.Single(x => x.Key == ClaimTypes.Name).Value];

            // attach user to context on successful jwt validation
            context.Items["User"] = accountService.GetById(userId);
        }
    }
}