using EducationApp.BusinessLogic.Models.Filters;
using EducationApp.BusinessLogic.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogic.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        Task<PrintingEditionModel> GetPrintingEditionsAsync(FilterPrintingEditionModel filterModel, bool isAdmin);
        Task<PrintingEditionModel> GetPrintingEditionDetailsAsync(long printingEdition, Enums.Currency currency);
        Task<PrintingEditionModel> CreatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem);
        Task<PrintingEditionModel> UpdatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem);
        Task<PrintingEditionModel> DeletePrintingEditionAsync(long printingEditionId);
    }
}
