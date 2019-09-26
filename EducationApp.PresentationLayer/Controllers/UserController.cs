using EducationApp.BusinessLayer.Models.Enums;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.Roles.User)]
        [HttpPost("get")]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.GetUserAsync(userId);

            return Ok(user);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.Roles.User)]
        [HttpPost("edit")]
        public async Task<IActionResult> EditUserProfileAsync([FromBody]RegistrationModel userModel)
        {
            if(await _userService.EditUserProfileAsync(userModel))
            {
                return Ok("Edit");
            }
            return BadRequest();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.Roles.Admin)]
        [HttpGet("get")]
        public async Task<IActionResult> GetAdminAsync()
        {
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.GetUserAsync(userId);

            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.Roles.Admin)]
        [HttpPost("get/users")]
        public async Task<IActionResult> GetUsersAsync(string userName = null, bool blocked = false)
        {
            if(string.IsNullOrWhiteSpace(userName) && !blocked)
            {
                return Ok(await _userService.GetAllUsersAsync());
            }
            if (!string.IsNullOrWhiteSpace(userName))
            {
                return Ok(await _userService.GetAllUsersAsync(userName, blocked));
            }

            return BadRequest();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.Roles.Admin)]
        [HttpPost("get/usersOrders")]
        public Task<IActionResult> GetUsersOrdersAsync() => throw new System.NotImplementedException();
    }
}
