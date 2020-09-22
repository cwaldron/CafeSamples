using System;
using System.Security.Claims;
using CafeLib.Authorization.Tokens;
using GrpcServer.Authentication;
using GrpcServer.Extensions;
using GrpcServer.Services;
using GrpcServer.Support;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GrpcServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly AppSettings _appSettings;

        public AccountsController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
            _appSettings = config.GetAppSettings();
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _accountService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _accountService.GetAll();
            return Ok(users);
        }

        [HttpGet("token/{name}")]
        public IActionResult GetToken(string name)
        {
            //var token = GenerateJwtToken(name);
            var token = GenerateToken(name);
            return Ok(token);
        }

        private string GenerateToken(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Name is not specified.");
            }

            var claims = new ClaimCollection
            {
                {ClaimTypes.Name, name}
            };

            var tokenBuilder = new TokenBuilder()
                .AddClaims(claims)
                .AddSecret(_appSettings.Secret)
                .Expires(DateTime.Now.AddMinutes(60));

            return tokenBuilder.Build().ToString();
        }
    }
}