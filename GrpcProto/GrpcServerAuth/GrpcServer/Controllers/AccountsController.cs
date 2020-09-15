using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CafeLib.Authorization.Tokens;
using GrpcServer.Authentication;
using GrpcServer.Services;
using GrpcServer.Support;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GrpcServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private const string Secret = "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING";

        private readonly IAccountService _accountService;
        private readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

        //private readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
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

        private static string GenerateToken(string name)
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
                .AddSecret(Secret)
                .Expires(DateTime.Now.AddMinutes(60));

            return tokenBuilder.Build().ToString();
        }
    }
}