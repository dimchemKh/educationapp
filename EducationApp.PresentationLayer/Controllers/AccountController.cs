﻿using EducationApp.BusinessLayer.Models.Users;
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
        public async Task<IActionResult> ForgorPasswordAsync([FromBody]LoginModelItem loginModelItem)
        {
            var responseModel = new LoginModel();
            var user = await _accountService.GetUserByEmailAsync(loginModelItem.Email);
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidEmail);
                return Ok(responseModel.Errors);
            }
            if (!await _accountService.ResetPasswordAsync(user))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidEmail);
                return Ok(responseModel.Errors);
            }
            return Ok(responseModel.Items);
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody]TokenModeltem tokenModelItem)
        {
            var responseModel = new TokenModel();

            var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenModelItem.RefreshToken);

            if (refreshToken.ValidTo < DateTime.Now)
            {
                responseModel.Errors.Add(Constants.Errors.TokenExpire);
                return Ok(responseModel.Errors);
            }

            var userId = refreshToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var role = await _accountService.GetRoleAsync(userId);
            var userName = await _accountService.GetUserNameAsync(userId);

            var accessClaims = _jwtHelper.GetAccessClaims(userId, role, userName);
            var refreshClaims = _jwtHelper.GetRefreshClaims(userId);

            responseModel.Items.Add(new TokenModeltem()
            {
                AccessToken = _jwtHelper.GenerateToken(accessClaims, _configOptions, _configOptions.Value.AccessTokenExpiration),
                RefreshToken = _jwtHelper.GenerateToken(refreshClaims, _configOptions, _configOptions.Value.RefreshTokenExpiration)
            }) ;
            
            return Ok(responseModel.Items);
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromBody]RegistrationModelItem userModel)
        {
            var responseModel = new RegistrationModel();
            var isExistedUser = await _accountService.SignUpAsync(userModel);
            if (!isExistedUser)
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
        public async Task<IActionResult> SignInAsync([FromBody]LoginModelItem loginModel)
        {
            var responseModel = new TokenModel();

            var user = await _accountService.SignInAsync(loginModel);
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNull);
                return Ok(responseModel.Errors);
            }
            var role = await _accountService.GetRoleAsync(user.Id.ToString());

            var accessClaims = _jwtHelper.GetAccessClaims(user.Id.ToString(), role, user.UserName);
            var refreshClaims = _jwtHelper.GetRefreshClaims(user.Id.ToString());

            responseModel.Items.Add(new TokenModeltem()
            {
                AccessToken = _jwtHelper.GenerateToken(accessClaims, _configOptions, _configOptions.Value.AccessTokenExpiration),
                RefreshToken = _jwtHelper.GenerateToken(refreshClaims, _configOptions, _configOptions.Value.RefreshTokenExpiration)
            });

            return Ok(responseModel.Items);
        }
         
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            var responseModel = new RegistrationModel();

            if (!await _accountService.ConfirmEmailAsync(userId, token))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidIdOrToken);
                return Ok(responseModel.Errors);
            }
            return Ok();
        }
    }
}
