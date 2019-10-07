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
        Task<PrintingEditionModel> GetUsersPrintingEditionsListAsync(PrintingEditionModel printingEditionsModel, FilterPrintingditionModel filterModel);
        Task<PrintingEditionModel> GetAdminPrintingEditionsListAsync(PrintingEditionModel printingEditionsModel, FilterPrintingditionModel filterModel, bool IsAdmin);
        Task<PrintingEditionModel> AddNewPrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem);
        Task<PrintingEditionModel> GetUserPrintingEditionPageAsync(PrintingEditionModel printingEditionsModel, FilterPrintingEditionDetailsModel pageFilterModel);
        Task<PrintingEditionModel> EditPrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem);
        Task<PrintingEditionModel> DeletePrintingEditionAsync(PrintingEditionModel printingEditionsModel, int printingEditionId);
    }
}
