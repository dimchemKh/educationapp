﻿using EducationApp.BusinessLayer.Models.PrintingEditions;
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
        [HttpGet("getPrintingEditionPage")]
        public async Task<IActionResult> GetPrintingEditionPageAsync(int printingEditionId)
        {
            var responseModel = new PrintingEditionsModel();

            responseModel = await _printingEditionService.GetUserPrintingEditionPageAsync(responseModel, printingEditionId);

            return Ok(responseModel.Items);
        }
    }
}
