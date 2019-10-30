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
        [HttpPost("create")]
        public async Task<IActionResult> CreateAuthorAsync([FromBody]AuthorModelItem authorModel)
        {
            var responseModel = await _authorService.CreateAuthorAsync(authorModel);
            return Ok(responseModel);
        }
        [HttpPost("get")]
        public async Task<IActionResult> GetAuthorsAsync([FromBody]BaseFilterModel authorFilterModel)
        {
            var responseModel = await _authorService.GetAuthorsListAsync(authorFilterModel);            
            return Ok(responseModel);
        } 
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAuthorAsync([FromBody]AuthorModelItem authorModelItem)
        {
            var responseModel = await _authorService.UpdateAuthorAsync(authorModelItem);
            return Ok(responseModel);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAuthorAsync([FromBody]AuthorModelItem authorModel)
        {
            var responseModel = await _authorService.DeleteAuthorAsync(authorModel.Id);
            return Ok(responseModel);
        }
    }
}
