using EducationApp.BusinessLayer.Helpers;
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
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOptions<Config> _configOptions;

        public AccountController(IAccountService accountService, IOptions<Config> configOptions)
        {
            _accountService = accountService;
            _configOptions = configOptions;
        }


        public IActionResult RefreshToken()
        {
            return null;
        }
        [AllowAnonymous]
        [HttpPost, Route("request")]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token;
            if (_authService.IsAuthenticated(request, out token))
            {
                return Ok(token);
            }

            return BadRequest("Invalid Request");
        }
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserModel userModel)
        {
            var code = await _accountService.RegisterAsync(userModel.FirstName, userModel.LastName, userModel.Email, userModel.Password);

            EmailHelper emailHelper = new EmailHelper();

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = userModel.Id, code = code },
                protocol: HttpContext.Request.Scheme);

            //emailHelper.SendAsync()

            return Ok();
        }
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUserManagementService service, IOptions<TokenManagement> tokenManagement)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagement.Value;
        }
        public bool IsAuthenticated(TokenRequest request, out string token)
        {

            token = string.Empty;
            if (!_userManagementService.IsValidUser(request.Username, request.Password)) return false;

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, request.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;

        }
        //public async Task<IActionResult> Authenticate([FromBody] UserModel userModel)
        //{
        //    var user = _accountService.Authenticate(userModel.Email, userModel.Password);

        //    if(user == null)
        //    {
        //        return BadRequest();
        //    }

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_configOptions.Value.JwtKey);


        //}

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult<string> Post(IOptions<Config> appSettings, [FromBody]UserModel userModel)
        //{
        //    // 1. Проверяем данные пользователя из запроса.
        //    // ...

        //    // 2. Создаем утверждения для токена.
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(appSettings.Value.JwtKey);

        //    var accessClaims = new List<Claim>
        //    {
        //         new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //         new Claim(ClaimTypes.NameIdentifier, userModel.FirstName),
        //         new Claim(ClaimTypes.Role, userModel.Role),
        //         new Claim(ClaimTypes.Name, userModel.LastName),
        //    };
        //    var refreshClaims = new List<Claim>
        //    {
        //         new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //         //new Claim(ClaimTypes.NameIdentifier, user.Id),
        //    };

        //    var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

        //    // 3. Генерируем JWT.
        //    var token = new JwtSecurityToken(
        //        issuer: "DemoApp",
        //        audience: "DemoAppClient",
        //        claims: accessClaims,
        //        expires: DateTime.Now.AddMinutes(5),
        //        signingCredentials: signingCredentials
        //    );

        //    string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        //    return jwtToken;
        //}

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
}
