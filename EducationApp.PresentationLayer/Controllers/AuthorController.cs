using EducationApp.BusinessLogic.Models.Authors;
using EducationApp.BusinessLogic.Models.Filters;
using EducationApp.BusinessLogic.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationApp.Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost("getAll")]
        public async Task<IActionResult> GetAllAuthorsAsync([FromBody]FilterAuthorModel filterModel)
        {
            var response = await _authorService.GetAllAuthorsAsync(filterModel);

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAuthorAsync([FromBody]AuthorModelItem authorModel)
        {
            var responseModel = await _authorService.CreateAuthorAsync(authorModel);

            return Ok(responseModel);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetAuthorsInPrintingEditionAsync([FromBody]FilterAuthorModel authorFilterModel)
        {
            var responseModel = await _authorService.GetFilteredAuthorsAsync(authorFilterModel);     
            
            return Ok(responseModel);
        } 

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAuthorAsync([FromBody]AuthorModelItem authorModelItem)
        {
            var responseModel = await _authorService.UpdateAuthorAsync(authorModelItem);

            return Ok(responseModel);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAuthorAsync(long authorId)
        {
            var responseModel = await _authorService.DeleteAuthorAsync(authorId);

            return Ok(responseModel);
        }
    }
}
