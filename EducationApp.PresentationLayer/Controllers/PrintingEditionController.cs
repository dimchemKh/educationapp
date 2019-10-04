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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _printingEditionService;
        public PrintingEditionController(IPrintingEditionService printingEditionService)
        {
            _printingEditionService = printingEditionService;
        }
        [AllowAnonymous]
        [HttpGet("printingEditions")]
        public async Task<IActionResult> PrintingEditions(/*[FromBody]UserFilterModel filterModel*/)
        {
            var responseModel = new PrintingEditionModel();
            //var result = await _printingEditionService.GetUsersPrintingEditionsListAsync(responseModel, filterModel);

            //return Ok(result.Items);
            return Ok();
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
        public async Task<IActionResult> GetPrintingEditionPageAsync([FromBody]PageFilterModel pageFilterModel)
        {
            var responseModel = new PrintingEditionModel();

            responseModel = await _printingEditionService.GetUserPrintingEditionPageAsync(responseModel, pageFilterModel);

            return Ok(responseModel.Items);
        }
        [HttpPost("printingEditions/admin")]
        public async Task<IActionResult> GetAdminPrintingEditionAsync([FromBody]AdminFilterModel filterModel)
        {
            var responseModel = new PrintingEditionModel();
            if (!User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }
            var result = await _printingEditionService.GetAdminPrintingEditionsListAsync(responseModel, filterModel);

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
