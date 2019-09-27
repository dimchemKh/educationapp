using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        Task<PrintingEditionsModel> GetPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, Enums.StateSort stateSort);
        Task<PrintingEditionsModel> GetFilteringPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, FilterModel filterModel);
    }
}
