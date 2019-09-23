using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.PresentationLayer.Common;
using EducationApp.PresentationLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOptions<Config> _configOptions;
        private EmailHelper _emailHelper;

        public AccountController(IAccountService accountService, IOptions<Config> configOptions, EmailHelper emailHelper)
        {
            _accountService = accountService;
            _configOptions = configOptions;
            _emailHelper = emailHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok();
        }

        public IActionResult RefreshToken()
        {
            return null;
        }

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromBody] UserModel userModel)
        {
            var user = await _accountService.RegisterAsync(userModel.FirstName, userModel.LastName, userModel.Email, userModel.Password);

            var code = await _accountService.GetConfirmToken(user);

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code = code },
                protocol: HttpContext.Request.Scheme);

            await _emailHelper.SendAsync(user, callbackUrl);

            return Ok();
        }

        [HttpPost("authorization")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Authorization([FromBody] UserModel userModel)
        {
            if (userModel.Email == null || userModel.Password == null)
            {
                return BadRequest();
            }
            var user = await _accountService.Authorization(userModel.Email, userModel.Password);

            var accessClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
            };

            var refreshClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
                       
            return JwtHelper.GenerateAccessToken(accessClaims, _configOptions);

            return JwtHelper.GenerateRefreshToken(refreshClaims, _configOptions);


        }

        [HttpGet("ConfirmEmail")]
        public async Task ConfirmEmail()
        {

        }
    }
}
