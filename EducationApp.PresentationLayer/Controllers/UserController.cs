using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using BusinessLayerConstants = EducationApp.BusinessLayer.Common.Constants;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }        
        [HttpPost("get")]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;             
            var responseModel = await _userService.GetOneUserAsync(userId);

            return Ok(responseModel);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody]UserUpdateModel userModel)
        {            
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole(Constants.Roles.Admin);

            if(!long.TryParse(userId, out long _userId)){
                userModel.Id = _userId;
            }            
            var responseModel = await _userService.UpdateUserProfileAsync(userModel, isAdmin);
            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPost("getAll")]
        public async Task<IActionResult> GetAllUsersAsync([FromBody]FilterUserModel filterUserModel)
        {
            var responseModel = await _userService.GetAllUsersAsync(filterUserModel);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPut("block")]
        public async Task<IActionResult> BlockUserAsync([FromBody]UserModelItem userModelItem)
        {
            var result = await _userService.BlockUserAsync(userModelItem.Id, userModelItem.LockoutEnabled);
            return Ok(result);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserAsync(long userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return Ok(result);
        }
    }
}
