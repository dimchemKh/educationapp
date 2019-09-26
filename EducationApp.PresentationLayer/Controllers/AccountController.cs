using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.PresentationLayer.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using EducationApp.PresentationLayer.Helper.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOptions<Config> _configOptions;
        private readonly IJwtHelper _jwtHelper;

        public AccountController(IAccountService accountService, IOptions<Config> configOptions, IJwtHelper jwtHelper)
        {
            _accountService = accountService;
            _configOptions = configOptions;
            _jwtHelper = jwtHelper;
        }
        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgorPasswordAsync([FromBody]RegistrationModel userModel)
        {
            var user = await _accountService.GetUserByEmailAsync(userModel);
            if(user == null)
            {
                return BadRequest("Email not found");
            }
            if (await _accountService.ResetPasswordAsync(user))
            {
                return Ok("Please check your email");
            }
            return BadRequest("Invalid email!");
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody]TokenModel tokenModel)
        {
            var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenModel.RefreshToken);

            if (refreshToken.ValidTo < DateTime.Now)
            {
                return BadRequest("RefreshToken expired!");
            }
            var response = new ResponseModel<TokenModel>();

            var userId = refreshToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var role = await _accountService.GetRoleAsync(userId);
            var userName = await _accountService.GetUserNameAsync(userId);

            var accessClaims = _jwtHelper.GetAccessClaims(userId, role, userName);
            var refreshClaims = _jwtHelper.GetRefreshClaims(userId);

            var newAccessToken = _jwtHelper.GenerateToken(accessClaims, _configOptions, _configOptions.Value.AccessTokenExpiration);
            var newRefreshToken = _jwtHelper.GenerateToken(refreshClaims, _configOptions, _configOptions.Value.RefreshTokenExpiration);
            
            return Ok(new TokenModel(newAccessToken, newRefreshToken));
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromBody]RegistrationModel userModel)
        {
            var existedUser = await _accountService.SignUpAsync(userModel);
            if (!existedUser)
            {
                return BadRequest("User is created");
            }
            var user = await _accountService.GetUserByEmailAsync(userModel);

            var token = await _accountService.GetEmailConfirmTokenAsync(user);

            var callbackUrl = Url.Action(
                "confirmEmailAsync",
                "Account",
                new { userId = user.Id, token },
                protocol: HttpContext.Request.Scheme);

            await _accountService.SendRegistrationMailAsync(user, callbackUrl);

            return Ok("Created");
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAsync([FromBody]LoginModel loginModel)
        {
            var user = await _accountService.SignInAsync(loginModel);
            if(user != null)
            {
                var role = await _accountService.GetRoleAsync(user.Id.ToString());

                var accessClaims = _jwtHelper.GetAccessClaims(user.Id.ToString(), role, user.UserName);
                var refreshClaims = _jwtHelper.GetRefreshClaims(user.Id.ToString());

                var AccessToken = _jwtHelper.GenerateToken(accessClaims, _configOptions, _configOptions.Value.AccessTokenExpiration);
                var RefreshToken = _jwtHelper.GenerateToken(refreshClaims, _configOptions, _configOptions.Value.RefreshTokenExpiration);

                return Ok(new TokenModel(AccessToken, RefreshToken));
            }
            return BadRequest("Something wrong!");
        }
         
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {            
            if (await _accountService.ConfirmEmailAsync(userId, token))
            {
                return Ok("You confirmed your email");
            }
            return BadRequest("Something broke!");
        }
    }
}
