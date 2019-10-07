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
        public async Task<IActionResult> PrintingEditionsAsync([FromBody]FilterPrintingditionModel filterModel)
        {
            var responseModel = new PrintingEditionModel();
            var result = await _printingEditionService.GetUsersPrintingEditionsListAsync(responseModel, filterModel);

            return Ok(result.Items);
        }
        [HttpPost("addNewProduct")]
        public async Task<IActionResult> AddNewProductAsync([FromBody]PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionModel();
            if (!User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }
            responseModel = await _printingEditionService.AddNewPrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel);
        }
        [AllowAnonymous]
        [HttpPost("printingEditionPage")]
        public async Task<IActionResult> GetPrintingEditionPageAsync([FromBody]FilterPrintingEditionDetailsModel pageFilterModel)
        {
            var responseModel = new PrintingEditionModel();

            responseModel = await _printingEditionService.GetUserPrintingEditionPageAsync(responseModel, pageFilterModel);

            return Ok(responseModel.Items);
        }
        [HttpPost("printingEditions/admin")]
        public async Task<IActionResult> PrintingEditionsAdminAsync([FromBody]FilterPrintingditionModel filterModel)
        {
            var responseModel = new PrintingEditionModel();
            var isAdmin = User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin);
            if (!isAdmin)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }
            var result = await _printingEditionService.GetAdminPrintingEditionsListAsync(responseModel, filterModel, isAdmin);

            return Ok(result.Items);
        }
        [HttpPut("printingEditions/admin")]
        public async Task<IActionResult> EditPrintingEditionAsync([FromBody]PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionModel();
            if (!User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }
            responseModel = await _printingEditionService.EditPrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel);
        }
    }
}
