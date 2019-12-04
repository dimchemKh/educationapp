using EducationApp.BusinessLogic.Models.Users;
using Microsoft.AspNetCore.Mvc;
using EducationApp.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using EducationApp.Presentation.Helper.Interfaces;
using System.Linq;
using Microsoft.AspNetCore.Http;
using EducationApp.Presentation.Common.Models.Configs;
using EducationApp.BusinessLogic.Models.Configs;

namespace EducationApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IOptions<AuthConfig> _configOptions;
        private readonly IJwtHelper _jwtHelper;
        private readonly IOptions<EmailConfig> _emailConfigs;
        public AccountController(IAccountService accountService, IOptions<AuthConfig> authConfigs, IJwtHelper jwtHelper, IOptions<EmailConfig> emailConfigs)
        {
            _accountService = accountService;
            _configOptions = authConfigs;
            _jwtHelper = jwtHelper;
            _emailConfigs = emailConfigs;
        }

        [HttpGet("signOut")]
        public async Task<IActionResult> LogOut()
        {
            var responseModel = await _accountService.SignOutAsync();

            SignOut();

            return Ok(responseModel);
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgorPasswordAsync([FromBody]UserLoginModel userModel)
        {
            var responseModel = await _accountService.ResetPasswordAsync(userModel.Email);

            return Ok(responseModel);
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshToken)
        {      
            var userInfoModel = _jwtHelper.ValidateData(refreshToken);

            if (userInfoModel.Errors.Any())
            {
                return Ok(userInfoModel);
            }

            userInfoModel = await _accountService.IdentifyUser(userInfoModel);

            var authModel = _jwtHelper.Generate(userInfoModel);

            authModel.Image = userInfoModel.Image;

            return Ok(authModel);
        }

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

        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAsync([FromBody]UserLoginModel loginModel)
        {
            var userInfoModel = await _accountService.SignInAsync(loginModel);

            if (userInfoModel.Errors.Any())
            {
                return Ok(userInfoModel);
            }

            var authModel = _jwtHelper.Generate(userInfoModel);

            authModel.Image = userInfoModel.Image;

            return Ok(authModel);
        }        
        
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string confirmToken)
        {
            var regModel = await _accountService.ConfirmEmailAsync(userId, confirmToken);

            string url = _emailConfigs.Value.ConfirmEmailUrl;

            if (regModel.Errors.Any())
            {
                var _str = regModel.Errors.FirstOrDefault();
                url += $"?error={_str}";
            }

            return Redirect(url);
        }        
    }
}
