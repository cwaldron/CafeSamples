using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CafeLib.Aspnet.Identity;
using CafeLib.Identity.Sqlite.Data;
using CafeLib.Identity.Sqlite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace CafeLib.Identity.Sqlite.Controllers
{
    public class AccountController : ControllerBase
    {
        private IdentityStorage _storage;

        public AccountController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _storage = (IdentityStorage) serviceProvider.GetService<IdentityDatabase>().GetStorage();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel request, string returnUrl)
        {
            var login = new LoginModel {EmailAddress = request.EmailAddress, Password = request.Password, RememberMe = request.RememberMe};
            await Task.CompletedTask;

            var user = !string.IsNullOrWhiteSpace(login.UserName)
                ? await _storage.FindUserByUserName<IdentityUser>(login.UserName)
                : !string.IsNullOrWhiteSpace(login.EmailAddress)
                    ? await _storage.FindUserByEmailAddress<IdentityUser>(login.EmailAddress)
                    : throw new ArgumentException();

            var identity = new ClaimsIdentity(GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                principal, 
                new AuthenticationProperties
                {
                    IsPersistent = login.RememberMe,
                    ExpiresUtc = DateTime.UtcNow.AddDays(2) // todo: configure this value
                });

            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            //}

            return View(login);
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] JToken request, string returnUrl)
        {
            var registrationModel = request.ToObject<RegistrationModel>();
            await Task.CompletedTask;
            return View(registrationModel);
        }

        private static IEnumerable<Claim> GetUserClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            return claims;
        }
    }
}