﻿using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }        
        [Authorize(Roles = Constants.Roles.User)]
        [HttpPost("getMe")]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;             
            var responseModel = await _userService.GetUserAsync(userId);

            return Ok(responseModel);
        }
        [Authorize]
        [HttpPost("edit")]
        public async Task<IActionResult> EditProfileAsync([FromBody]UserEditModel userModel)
        {           
            var responseModel = await _userService.EditUserProfileAsync(userModel);

            if (!responseModel.Errors.Any())
            {
                return Ok(responseModel);
            }
            return Ok();
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPut("edit")]
        public async Task<IActionResult> EditUserProfileAsync([FromBody]UserEditModel userModel)
        {
            var isAdmin = User.IsInRole(Constants.Roles.Admin);
            var responseModel = await _userService.EditUserProfileAsync(userModel, isAdmin);
            if (!responseModel.Errors.Any())
            {
                return Ok(responseModel);
            }
            return Ok();
        }
        //[Authorize(Roles = Constants.Roles.Admin)]
        [HttpPost("getUsers")]
        public async Task<IActionResult> GetUsersAsync([FromBody]FilterUserModel filterUserModel)
        {
            var responseModel = await _userService.GetUsersAsync(filterUserModel);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPut("getUsers/{userId}")]
        public async Task<IActionResult> BlockUserAsync(string userId, Enums.IsBlocked isBlocked)
        {
            var result = await _userService.BlockUserAsync(userId, isBlocked);
            return Ok(result);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpDelete("getUsers/{userId}")]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return Ok(result);
        }
    }
}
