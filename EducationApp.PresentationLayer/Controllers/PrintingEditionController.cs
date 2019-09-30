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

        [HttpPost("filtering")]
        public async Task<IActionResult> GetFilteringPrintingEditionAsync([FromBody]FilterModel filterModel)
        {
            var model = new PrintingEditionsModel();
            var result = await _printingEditionService.GetPrintingEditionsListAsync(model, filterModel);

            //string[] asd = null;

            //foreach (var item in result.Items)
            //{
            //    foreach (var q in item.AuthorModels)
            //    {
            //        return Ok(q);
            //    }
            //}

            return Ok(result.Items/*filterModel*/);
        }
    }
}
