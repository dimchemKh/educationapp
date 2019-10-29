using EducationApp.BusinessLayer.Models.Users;
using Microsoft.AspNetCore.Mvc;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.PresentationLayer.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using EducationApp.PresentationLayer.Helper.Interfaces;
using System.Linq;
using EducationApp.BusinessLayer.Common.Constants;
using Microsoft.AspNetCore.Http;
using System;

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
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            Response.Cookies.Append("Test", "this is secret cookie");
            return Ok();
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

            Response.Cookies.Append(_configOptions.Value.AccessName, result.AccessToken, new CookieOptions()
            {
                Expires = DateTime.Now.Add(_configOptions.Value.AccessTokenExpiration)
            });
            Response.Cookies.Append(_configOptions.Value.RefreshName, result.RefreshToken, new CookieOptions()
            {
                Expires = DateTime.Now.Add(_configOptions.Value.RefreshTokenExpiration)
            });

            return Ok();
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
            Response.Cookies.Append(_configOptions.Value.AccessName, result.AccessToken, new CookieOptions()
            {
                Expires = DateTime.Now.Add(_configOptions.Value.AccessTokenExpiration)
            });
            Response.Cookies.Append(_configOptions.Value.RefreshName, result.RefreshToken, new CookieOptions()
            {
                Expires = DateTime.Now.Add(_configOptions.Value.RefreshTokenExpiration)
            });

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
    }
}
