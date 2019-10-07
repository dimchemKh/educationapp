using EducationApp.BusinessLayer.Models.Users;
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
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.BusinessLayer.Models.Auth;

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
        public async Task<IActionResult> ForgorPasswordAsync([FromBody]UserLoginModel loginModelItem)
        {
            var userModel = new UserLoginModel();
            var user = await _accountService.GetUserByEmailAsync(loginModelItem.Email);
            if(user == null)
            {
                userModel.Errors.Add(Constants.Errors.InvalidEmail);
                return Ok(userModel);
            }
            if (!await _accountService.ResetPasswordAsync(user))
            {
                userModel.Errors.Add(Constants.Errors.InvalidEmail);
                return Ok(userModel);
            }
            return Ok(userModel);
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody]string token)
        {
            var authModel = _jwtHelper.CheckAccess(token);
            if (!authModel.Errors.Any())
            {
                return Ok(authModel);
            }
            authModel.UserRole = await _accountService.GetRoleAsync(authModel.UserId);
            authModel.UserName = await _accountService.GetUserNameAsync(authModel.UserId);
            var result = _jwtHelper.Generate(authModel, _configOptions);            
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromBody]UserRegistrationModel userRegModel)
        {
            var userModel = await _accountService.SignUpAsync(userRegModel);
            if (!userModel.Errors.Any())
            {
                responseModel.Errors.Add(Constants.Errors.IsExistedUser);
                return Ok(responseModel.Errors);
            }
            var user = await _accountService.GetUserByEmailAsync(userModel.Email);
            var token = await _accountService.GetEmailConfirmTokenAsync(user);
            var callbackUrl = Url.Action(
                "confirmEmailAsync",
                "Account",
                new { userId = user.Id, token },
                protocol: HttpContext.Request.Scheme);
            await _accountService.SendRegistrationMailAsync(user, callbackUrl);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAsync([FromBody]UserLoginModel loginModel)
        {
            var responseModel = new AuthModel();

            var user = await _accountService.SignInAsync(loginModel);
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNull);
                return Ok(responseModel.Errors);
            }
            var role = await _accountService.GetRoleAsync(user.Id.ToString());

            var accessClaims = _jwtHelper.GetAccessClaims(user.Id.ToString(), role, user.UserName);
            var refreshClaims = _jwtHelper.GetRefreshClaims(user.Id.ToString());

            responseModel = new AuthModel()
            {
                AccessToken = _jwtHelper.GenerateToken(accessClaims, _configOptions, _configOptions.Value.AccessTokenExpiration),
                RefreshToken = _jwtHelper.GenerateToken(refreshClaims, _configOptions, _configOptions.Value.RefreshTokenExpiration)
            };

            return Ok(responseModel);
        }         
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            var responseModel = new UserRegistrationModel();

            if (!await _accountService.ConfirmEmailAsync(userId, token))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidIdOrToken);
                return Ok(responseModel.Errors);
            }
            return Ok();
        }
    }
}
