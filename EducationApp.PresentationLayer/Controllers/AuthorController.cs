using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
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

        [HttpPost("get/Authors")]
        public async Task<IActionResult> GetAuthorsAsync([FromBody]BaseFilterModel authorFilterModel)
        {
            var responseModel = await _authorService.GetAuthorsListAsync(authorFilterModel);
            
            return Ok(responseModel);
        } 
        [HttpPut("get/Authors")]
        public async Task<IActionResult> EditAuthorAsync([FromBody]AuthorModelItem authorModelItem)
        {
            var responseModel = await _authorService.EditAuthorAsync(authorModelItem);
            return Ok(responseModel);
        }
        [HttpDelete("get/Authors")]
        public async Task<IActionResult> DeleteAync([FromBody]AuthorModelItem authorModel)
        {
            var responseModel = await _authorService.DeleteAuthorAsync(authorModel.Id);

            return Ok(responseModel);
        }
    }
}
