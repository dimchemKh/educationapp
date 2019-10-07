using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IPrintingEditionRepository : IBaseEFRepository<PrintingEdition>
    {
        Task<bool> IsExistedPrintingEdition(string printingEditionName);
        IQueryable<PrintingEdition> FiteringFromSearchWord(string searchByWord, IQueryable<PrintingEdition> printingEditions);
        IQueryable<PrintingEdition> FilteringByTypes(ICollection<Enums.PrintingEditionType> types, IQueryable<PrintingEdition> printingEditions);
        IQueryable<PrintingEdition> FilteringByPrice(IDictionary<Enums.RangePrice, decimal> RangePrice, IQueryable<PrintingEdition> printingEditions);
        Task<ICollection<string>> GetAuthorsInPrintingEditionAsync(PrintingEdition printingEdition);
    }
}
