using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.PresentationLayer.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Post(IOptions<Config> appSettings, CredentialsView credentialsView, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            // 1. Проверяем данные пользователя из запроса.
            // ...

            // 2. Создаем утверждения для токена.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var accessClaims = new List<Claim>
            {
                 new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.NameIdentifier, credentialsView.Name),
                 new Claim(ClaimTypes.Role, credentialsView.Role),
                 new Claim(ClaimTypes.Name, credentialsView.Name),
            };
            var refreshClaims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var signingCredentials = new SigningCredentials(signingEncodingKey.GetKey(), signingEncodingKey.SigningAlgorithm);

            // 3. Генерируем JWT.
            var token = new JwtSecurityToken(
                issuer: "DemoApp",
                audience: "DemoAppClient",
                claims: accessClaims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: 
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
        public IActionResult RefreshToken()
        {
            return null;
        }

        [HttpGet("testget")]
        public IActionResult TestGet()
        {
            return Ok("Get");
        }
        [HttpPost("registr")]
        public async Task<IActionResult> Registr([FromBody] UserModel userModel)
        {
            await _accountService.RegisterAsync(userModel.FirstName, userModel.LastName, userModel.Email, userModel.Password);

            return Ok();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register()
        //{
            //if (ModelState.IsValid)
            //{
            //    var user = new ApplicationUser {  };
            //    var result = await UserManager<ApplicationUser>.CreateAsync(user, );
            //    if (result.Succeeded)
            //    {
            //        await SignInManager<ApplicationUser>.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        var callbackUrl = Url.Action("ConfirmEmail", "Account",
            //           new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //        await UserManager.SendEmailAsync(user.Id,
            //           "Confirm your account", "Please confirm your account by clicking <a href=\""
            //           + callbackUrl + "\">here</a>");

            //        return RedirectToAction("Index", "Home");
            //    }
            //    AddErrors(result);
            //}

            //// If we got this far, something failed, redisplay form
            //return View(model);
        //}
    }
    public class CredentialsView
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
