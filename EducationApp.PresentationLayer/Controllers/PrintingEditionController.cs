using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace EducationApp.PresentationLayer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _printingEditionService;
        public PrintingEditionController(IPrintingEditionService printingEditionService)
        {
            _printingEditionService = printingEditionService;
        }
        [AllowAnonymous]
        [HttpPost("get")]
        public async Task<IActionResult> GetPrintingEditionsAsync([FromBody]FilterPrintingEditionModel filterModel)
        {
            var role = User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
            var isAdmin = (role == null) ? false : role.Equals(Constants.Roles.Admin);
            var responseModel = await _printingEditionService.GetPrintingEditionsAsync(filterModel, isAdmin);

            return Ok(responseModel);
        }
        [AllowAnonymous]
        [HttpPost("details")]
        public async Task<IActionResult> GetPrintingEditionDetailsAsync([FromBody]PrintingEditionModelItem printingEdition)
        {
            var responseModel = await _printingEditionService.GetPrintingEditionDetailsAsync(printingEdition);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePrintingEditionAsync([FromBody]PrintingEditionModelItem printingEditionsModelItem)
        {          
            var responseModel = await _printingEditionService.CreatePrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel);
        }


        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePrintingEditionAsync([FromBody]PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = await _printingEditionService.UpdatePrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpDelete("delete/{printingEditionId}")]
        public async Task<IActionResult> DeletePrintingEditionAsync(long printingEditionId)
        {
            var responseModel = await _printingEditionService.DeletePrintingEditionAsync(printingEditionId);

            return Ok(responseModel);
        }
    }
}
