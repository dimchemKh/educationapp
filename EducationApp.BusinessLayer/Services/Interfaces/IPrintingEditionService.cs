using EducationApp.BusinessLayer.Models.Filters;
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
        Task<PrintingEditionModel> GetPrintingEditionsAsync(FilterPrintingEditionModel filterModel, bool isAdmin);
        Task<PrintingEditionModel> CreatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem);
        Task<PrintingEditionModel> GetPrintingEditionDetailsAsync(PrintingEditionModelItem printingEdition);
        Task<PrintingEditionModel> UpdatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem);
        Task<PrintingEditionModel> DeletePrintingEditionAsync(long printingEditionId);
    }
}
