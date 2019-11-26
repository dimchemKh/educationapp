using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IPrintingEditionRepository : IBaseRepository<PrintingEdition>
    {
        Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin);
        Task<PrintingEditionDataModel> GetPrintingEditionDetailsAsync(long printingEdition);
    }
}
