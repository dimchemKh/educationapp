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
        
        [HttpPost("getMe")]
        public async Task<IActionResult> GetUserAsync()
        {
            var responseModel = new UserModel();

            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;
             
            await _userService.GetUserAsync(userId, responseModel);

            return Ok(responseModel.Items);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditUserProfileAsync([FromBody]EditModelItem userModel)
        {
            var responseModel = await _userService.EditUserProfileAsync(userModel);
            if (responseModel.Errors.Any())
            {
                return Ok(responseModel.Errors);
            }
            return Ok();
        }

        [HttpPost("getMe/users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var responseModel = new UserModel();
            if (!User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }

            return Ok(await _userService.GetAllUsersAsync(responseModel));
        }
        [HttpPost("getMe/users")]
        public async Task<IActionResult> GetUsersAsync(string userName)
        {
            var responseModel = new UserModel();
            if (!User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }

            return Ok(await _userService.GetAllUsersAsync(responseModel, userName));
        }
        [HttpPut("getMe/users/{id}")]
        public async Task<IActionResult> BlockUserAsync(string userId, Enums.IsBlocked isBlocked)
        {
            var responseModel = new UserModel();
            if (!User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }

            return Ok(await _userService.BlockUserAsync(userId, isBlocked, responseModel));
        }

    }
}
