using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _printingEditionService;
        public PrintingEditionController(IPrintingEditionService printingEditionService)
        {
            _printingEditionService = printingEditionService;
        }

        [HttpPost("printingEditions")]
        public async Task<IActionResult> GetFilteringPrintingEditionAsync([FromBody]UserFilterModel filterModel)
        {
            var responseModel = new PrintingEditionsModel();
            var result = await _printingEditionService.GetUsersPrintingEditionsListAsync(responseModel, filterModel);

            return Ok(result.Items);
        }
        [HttpPost("addNewProduct")]
        public async Task<IActionResult> AddNewProductAsync([FromBody]PrintingEditionsModelItem printingEditionsModelItem)
        {

            var responseModel = await _printingEditionService.AddNewPrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel.Items);
        }
        [HttpPost("getPrintingEditionPage")]
        public async Task<IActionResult> GetPrintingEditionPageAsync([FromBody]PageFilterModel pageFilterModel)
        {
            var responseModel = new PrintingEditionsModel();

            responseModel = await _printingEditionService.GetUserPrintingEditionPageAsync(responseModel, pageFilterModel);

            return Ok(responseModel.Items);
        }
        [HttpPost("printingEditions/admin")]
        public async Task<IActionResult> GetAdminPrintingEditionAsync([FromBody]AdminFilterModel filterModel)
        {
            var responseModel = new PrintingEditionsModel();
            var result = await _printingEditionService.GetAdminPrintingEditionsListAsync(responseModel, filterModel);

            return Ok(result.Items);
        }
        [HttpPut("printingEditions/admin")]
        public async Task<IActionResult> EditPrintingEditionAsync([FromBody]PrintingEditionsModelItem printingEditionsModelItem)
        {
            var responseModel = await _printingEditionService.EditPrintingEditionAsync(printingEditionsModelItem);

            return Ok(responseModel);
        }
    }
}
