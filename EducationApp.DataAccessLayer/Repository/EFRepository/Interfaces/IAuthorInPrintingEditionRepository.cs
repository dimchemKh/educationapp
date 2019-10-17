using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository : IBaseEFRepository<AuthorInPrintingEdition>
    {
        Task UpdateAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<AuthorDataModel> authors);
        Task AddAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<AuthorDataModel> authors);
        Task<IList<AuthorDataModel>> GetAuthorsInOnePrintingEditionAsync(long printingEditionId);
        Task DeleteByAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate);
        Task<IEnumerable<AuthorDataModel>> GetAuthorsFilteredDataAsync(BaseFilterModel filter);
        Task<IEnumerable<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter);
    }
}
