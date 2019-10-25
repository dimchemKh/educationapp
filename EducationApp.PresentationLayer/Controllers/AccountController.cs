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
        public async Task<IActionResult> ForgorPasswordAsync([FromBody]UserLoginModel userModel)
        {
            var responseModel = await _accountService.ResetPasswordAsync(userModel.Email);

            return Ok(responseModel);
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync()
        {            
            var authModel = _jwtHelper.ValidateData(Request.Cookies["refresh"]);
            if (!authModel.Errors.Any())
            {
                return Ok(authModel);
            }
            authModel = await _accountService.IdentifyUser(authModel);
            var result = _jwtHelper.Generate(authModel, _configOptions);

            Response.Cookies.Append("access", result.AccessToken);
            Response.Cookies.Append("refresh", result.RefreshToken);

            return Ok(result);
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
            // TODO: maybe change callback to client?
            //var callbackUrl = "http://localhost:4200/account/confirmEmail?" + $"UserId={userModel.UserId}" + $"ConfirmToken={userModel.ConfirmToken}";
            _accountService.SendRegistrationMailAsync(userModel.UserId, callbackUrl);
            return Ok(responseModel);
        }
        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAsync([FromBody]UserLoginModel loginModel)
        {
            var authModel = await _accountService.SignInAsync(loginModel);
            if (authModel.Errors.Any())
            {
                return Ok(authModel);
            }
            var result = _jwtHelper.Generate(authModel, _configOptions);
            Response.Cookies.Append("access", result.AccessToken);
            Response.Cookies.Append("refresh", result.RefreshToken);

            return Ok(authModel);
        }         
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string confirmToken)
        {
            var regModel = await _accountService.ConfirmEmailAsync(userId, confirmToken);
            string url = "http://localhost:4200/account/confirmEmail";
            if (regModel.Errors.Any())
            {
                var _str = regModel.Errors.FirstOrDefault();
                url += $"?error={_str}";
            }

            return Redirect(url);
        }
    }
}
