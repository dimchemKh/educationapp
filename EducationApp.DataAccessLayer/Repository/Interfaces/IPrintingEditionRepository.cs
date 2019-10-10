using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IPrintingEditionRepository : IBaseEFRepository<PrintingEdition>
    {
        Task<bool> IsExistedPrintingEdition(PrintingEditionModel model);
        Task<IEnumerable<PrintingEditionForAdminModel>> FilteringAsync(FilterPrintingEditionModel filter);
        //IQueryable<PrintingEdition> FiteringFromSearchWord(string searchByWord, IQueryable<PrintingEdition> printingEditions);
        //IQueryable<PrintingEdition> FilteringByTypes(ICollection<Enums.PrintingEditionType> types, IQueryable<PrintingEdition> printingEditions);
        //IQueryable<PrintingEdition> FilteringByPrice(IDictionary<Enums.RangePrice, decimal> RangePrice, IQueryable<PrintingEdition> printingEditions);
        //Task<ICollection<string>> GetAuthorsInPrintingEditionAsync(PrintingEdition printingEdition);
    }
}
