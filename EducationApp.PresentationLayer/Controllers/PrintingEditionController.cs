using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.PrintingEditions;
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
        [HttpPost("printingEditions")]
        public async Task<IActionResult> PrintingEditionsAsync([FromBody]FilterPrintingEditionModel filterModel)
        {            
            var responseModel = await _printingEditionService.GetPrintingEditionsAsync(filterModel);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPost("createPrintingEdition")]
        public async Task<IActionResult> CreatePrintingEditionAsync([FromBody]PrintingEditionModelItem printingEditionsModelItem)
        {          
            var responseModel = await _printingEditionService.CreatePrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel);
        }
        [AllowAnonymous]
        [HttpPost("printingEditionPage")]
        public async Task<IActionResult> GetPrintingEditionDetailsAsync([FromBody]FilterPrintingEditionDetailsModel pageFilterModel)
        {            
            var responseModel = await _printingEditionService.GetPrintingEditionDetailsAsync(pageFilterModel);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPost("printingEditions/admin")]
        public async Task<IActionResult> PrintingEditionsAdminAsync([FromBody]FilterPrintingEditionModel filterModel)
        {
            var responseModel = new PrintingEditionModel();  //await _printingEditionService.GetAdminPrintingEditionsListAsync(filterModel);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPut("printingEditions/admin")]
        public async Task<IActionResult> EditPrintingEditionAsync([FromBody]PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = await _printingEditionService.EditPrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel);
        }
    }
}
