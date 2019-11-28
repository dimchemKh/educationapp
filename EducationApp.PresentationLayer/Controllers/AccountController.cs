using EducationApp.BusinessLogic.Models.Users;
using Microsoft.AspNetCore.Mvc;
using EducationApp.BusinessLogic.Services.Interfaces;
using EducationApp.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using EducationApp.Presentation.Helper.Interfaces;
using System.Linq;
using EducationApp.BusinessLogic.Common.Constants;
using Microsoft.AspNetCore.Http;
using System;
using EducationApp.BusinessLogic.Models.Auth;

namespace EducationApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOptions<AuthConfig> _configOptions;
        private readonly IJwtHelper _jwtHelper;
        public AccountController(IAccountService accountService, IOptions<AuthConfig> configOptions, IJwtHelper jwtHelper)
        {
            _accountService = accountService;
            _configOptions = configOptions;
            _jwtHelper = jwtHelper;
        }
        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgorPasswordAsync([FromBody]UserLoginModel userModel)
        {
            var responseModel = await _accountService.ResetPasswordAsync(userModel.Email);

            return Ok(responseModel);
        }
        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokenAsync()
        {      
            var token = Request.Cookies["Refresh"];

            var userInfoModel = _jwtHelper.ValidateData(token);

            if (userInfoModel.Errors.Any())
            {
                return Ok(userInfoModel);
            }

            userInfoModel = await _accountService.IdentifyUser(userInfoModel);

            var result = _jwtHelper.Generate(userInfoModel, _configOptions);

            GenerateCookie(result);

            return Ok(userInfoModel);
        }
        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromBody]UserRegistrationModel registrationModel)
        {
            var responseModel = await _accountService.SignUpAsync(registrationModel);

            if (responseModel.Errors.Any())
            {
                return Ok(responseModel);
            }
            var userModel = await _accountService.GetEmailConfirmTokenAsync(registrationModel.Email);

            if (userModel.Errors.Any())
            {
                responseModel.Errors = userModel.Errors;
                return Ok(responseModel);
            }

            var callbackUrl = Url.Action(
                "confirmEmailAsync",
                "Account",
                new { userModel.UserId, userModel.ConfirmToken },
                protocol: HttpContext.Request.Scheme);

            await _accountService.SendRegistrationMailAsync(userModel.UserId, callbackUrl);

            return Ok(responseModel);
        }
        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAsync([FromBody]UserLoginModel loginModel)
        {
            var userInfoModel = await _accountService.SignInAsync(loginModel);

            if (userInfoModel.Errors.Any())
            {
                return Ok(userInfoModel);
            }

            var result = _jwtHelper.Generate(userInfoModel, _configOptions);

            GenerateCookie(result);

            return Ok(userInfoModel);
        }         
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string confirmToken)
        {
            var regModel = await _accountService.ConfirmEmailAsync(userId, confirmToken);

            string url = Constants.SmtpSettings.ConfirmEmailUrl;

            if (regModel.Errors.Any())
            {
                var _str = regModel.Errors.FirstOrDefault();
                url += $"?error={_str}";
            }

            return Redirect(url);
        }        
        private void GenerateCookie(AuthModel result)
        {
            Response.Cookies.Append(_configOptions.Value.AccessName, result.AccessToken, new CookieOptions()
            {
                Expires = DateTime.Now.Add(_configOptions.Value.AccessTokenExpiration)
            });

            Response.Cookies.Append(_configOptions.Value.RefreshName, result.RefreshToken, new CookieOptions()
            {
                Expires = DateTime.Now.Add(_configOptions.Value.RefreshTokenExpiration)
            });
        }
    }
}
