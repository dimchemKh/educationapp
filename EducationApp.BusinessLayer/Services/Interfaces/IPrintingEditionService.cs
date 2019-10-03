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
        Task<PrintingEditionsModel> GetUsersPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, UserFilterModel filterModel);
        Task<PrintingEditionsModel> GetAdminPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, AdminFilterModel filterModel);
        Task<PrintingEditionsModel> AddNewPrintingEditionAsync(PrintingEditionsModelItem printingEditionsModelItem);
        Task<PrintingEditionsModel> GetUserPrintingEditionPageAsync(PrintingEditionsModel printingEditionsModel, PageFilterModel pageFilterModel);
        Task<PrintingEditionsModel> EditPrintingEditionAsync(PrintingEditionsModelItem printingEditionsModelItem);
        Task<PrintingEditionsModel> DeletePrintingEditionAsync(PrintingEditionsModel printingEditionsModel, int printingEditionId);
    }
}
