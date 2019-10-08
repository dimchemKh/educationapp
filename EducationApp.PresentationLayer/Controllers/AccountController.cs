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
            var userId = await _accountService.GetUserByEmailAsync(loginModelItem.Email);
            if(userId == Constants.Errors.NotFindUserId)
            {
                userModel.Errors.Add(Constants.Errors.InvalidEmail);
                return Ok(userModel);
            }
            if (!await _accountService.ResetPasswordAsync(userId))
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
                return Ok(userModel);
            }
            var userId = await _accountService.GetUserByEmailAsync(userRegModel.Email);
            if (userId != Constants.Errors.NotFindUserId)
            {
                userModel.Errors.Add(Constants.Errors.IsExistedUser);
                return Ok(userModel);
            }
            var token = await _accountService.GetEmailConfirmTokenAsync(userId);
            
            var callbackUrl = Url.Action(
                "confirmEmailAsync",
                "Account",
                new { userId = userId, token },
                protocol: HttpContext.Request.Scheme);
            await _accountService.SendRegistrationMailAsync(userId, callbackUrl);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAsync([FromBody]UserLoginModel loginModel)
        {
            var authModel = await _accountService.SignInAsync(loginModel);
            if (!authModel.Errors.Any())
            {
                return Ok(authModel);
            }
            authModel.UserRole = await _accountService.GetRoleAsync(authModel.UserId);
            authModel.UserName = await _accountService.GetUserNameAsync(authModel.UserId);
            var result = _jwtHelper.Generate(authModel, _configOptions);
            return Ok(result);
        }         
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            var regModel = await _accountService.ConfirmEmailAsync(userId, token);
            if (!regModel.Errors.Any())
            {
                regModel.Errors.Add(Constants.Errors.InvalidIdOrToken);
            }
            return Ok(regModel);
        }
    }
}
