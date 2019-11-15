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
using EducationApp.DataAccessLayer.Entities.Enums;

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
        public async Task<IActionResult> GetPrintingEditionsAsync(string role, [FromBody]FilterPrintingEditionModel filterModel)
        {
            var isAdmin = false;

            if(!string.IsNullOrWhiteSpace(role) && role.Equals(Constants.Roles.Admin))
            {
                isAdmin = true;
            }

            if(string.IsNullOrWhiteSpace(role))
            {
                var claimRole = User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value;
                isAdmin = claimRole.Equals(Constants.Roles.Admin) ? true : false;
            }

            var responseModel = await _printingEditionService.GetPrintingEditionsAsync(filterModel, isAdmin);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.User)]
        [HttpGet("details")]
        public async Task<IActionResult> GetPrintingEditionDetails(long printingEditionId, Enums.Currency currency)
        {
            var responseModel = await _printingEditionService.GetPrintingEditionDetailsAsync(printingEditionId, currency);

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
        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePrintingEditionAsync(long printingEditionId)
        {
            var responseModel = await _printingEditionService.DeletePrintingEditionAsync(printingEditionId);

            return Ok(responseModel);
        }
    }
}
