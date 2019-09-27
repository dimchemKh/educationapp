using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
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

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var model = new PrintingEditionsModel();
            var result = await _printingEditionService.GetPrintingEditionsListAsync(model, DataAccessLayer.Entities.Enums.Enums.StateSort.PriceDesc);
            return Ok(result.Items);
        }
        [HttpPost("filtering")]
        public async Task<IActionResult> GetFilteringPrintingEditionAsync([FromBody]FilterModel filterModel)
        {
            var model = new PrintingEditionsModel();
            var result = await _printingEditionService.GetFilteringPrintingEditionsListAsync(model, filterModel);

            return Ok(result.Items);
        }
    }
}
