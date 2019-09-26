using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "user")]
        [HttpPost("get")]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.GetUserAsync(userId);

            return Ok(user);
        }
        [HttpPost("edit")]
        public async Task<IActionResult> EditUserProfileAsync([FromBody]UserModel userModel)
        {
            if(await _userService.EditUserProfileAsync(userModel))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
