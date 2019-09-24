using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.PresentationLayer.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.PresentationLayer.Helper.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IOptions<Config> _configOptions;
        private IJwtHelper _jwtHelper;


        public AccountController(IAccountService accountService, IOptions<Config> configOptions, IJwtHelper jwtHelper)
        {
            _accountService = accountService;
            _configOptions = configOptions;
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            
            return Ok("OK");
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgorPasswordAsync([FromBody]UserModel userModel)
        {
            var user = await _accountService.GetUserByEmailAsync(userModel.Email);
            if(user != null)
            {
                var result = await _accountService.ResetPasswordAsync(user);
                if (result.Succeeded)
                {
                    return Ok("Please check your email");
                }
            }
            return BadRequest("Invalid email!");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody]TokenModel tokenModel)
        {
            JwtSecurityToken refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenModel.RefreshToken);

            if (refreshToken.ValidTo > DateTime.Now)
            {

                var userId = refreshToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var role = await _accountService.GetRoleAsync(userId);
                var userName = await _accountService.GetUserNameAsync(userId);

                var accessClaims = _jwtHelper.GetAccessClaims(userId, role, userName);
                var refreshClaims = _jwtHelper.GetRefreshClaims(userId);

                var AT = _jwtHelper.GenerateToken(accessClaims, _configOptions, _configOptions.Value.AccessTokenExpiration);
                var RT = _jwtHelper.GenerateToken(refreshClaims, _configOptions, _configOptions.Value.RefreshTokenExpiration);

                return Ok(new TokenModel(AT, RT));
            }
            return BadRequest("RefreshToken expired!");
        }


        [HttpPost("test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult Test()
        {
            return Ok("YOU ARE AUTHENTIFICATE");
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] UserModel userModel)
        {
            var existedUser = await _accountService.SignUpAsync(userModel.FirstName, userModel.LastName, userModel.Email, userModel.Password);
            if (!existedUser)
            {
                return BadRequest("User is created");
            }
            var user = await _accountService.GetUserByEmailAsync(userModel.Email);

            var code = await _accountService.GetEmailConfirmTokenAsync(user);

            var callbackUrl = Url.Action(
                "confirmEmail",
                "account",
                new { userId = user.Id, code = code },
                protocol: HttpContext.Request.Scheme);

            await _accountService.SendRegistrationMailAsync(user, callbackUrl);
            
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAsync([FromBody] UserModel userModel)
        {
            if (userModel.Email == null || userModel.Password == null)
            {
                return BadRequest();
            }
            var user = await _accountService.SignInAsync(userModel.Email, userModel.Password);
            if(user != null)
            {
                var role = await _accountService.GetRoleAsync(user);

                var accessClaims = _jwtHelper.GetAccessClaims(user.Id.ToString(), role, user.UserName);
                var refreshClaims = _jwtHelper.GetRefreshClaims(user.Id.ToString());

                var AT = _jwtHelper.GenerateToken(accessClaims, _configOptions, _configOptions.Value.AccessTokenExpiration);
                var RT = _jwtHelper.GenerateToken(refreshClaims, _configOptions, _configOptions.Value.RefreshTokenExpiration);

                return Ok(new TokenModel(AT, RT));
            }
            return BadRequest("Something wrong!");
        }

        //public async Task<IActionResult> Forgot

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            if(userId == null || code == null)
            {
                //TODO
                return BadRequest();
            }

            var result = await _accountService.ConfirmEmailAsync(userId, code);

            if (result)
            {
                return Ok("You confirmed your email");
            }
            else
            {
                return BadRequest("Something broke!");
            }

        }
    }
}
