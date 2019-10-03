using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
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
        public async Task<IActionResult> GetAuthorsAsync(AuthorFilterModel authorFilterModel)
        {
            var responseModel = new AuthorModel();
            
            return Ok(await _authorService.GetAuthorsListAsync(responseModel, authorFilterModel));
        }
        [HttpPut("get/Authors")]
        public async Task<IActionResult> EditAuthorAsync(AuthorModelItem authorModelItem)
        {
            return Ok(await _authorService.EditAuthorAsync(authorModelItem));
        }
    }
}
