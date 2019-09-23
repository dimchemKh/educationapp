using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
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
        [ValidateAntiForgeryToken]
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserModel userModel)
        {
            var user = await _accountService.RegisterAsync(userModel.FirstName, userModel.LastName, userModel.Email, userModel.Password);

            var code = await _accountService.GetConfirmToken(user);

            EmailHelper emailHelper = new EmailHelper();

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code = code },
                protocol: HttpContext.Request.Scheme);

            await emailHelper.SendAsync(user, callbackUrl);

            return Ok();
        } 
        
        public async Task ConfirmEmail()
        {

        }
    }
}
