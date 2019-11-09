using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
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
        Task<bool> UpdateAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<long> authorsId);
        Task AddAuthorsInPrintingEditionAsync(long printingEditionId, IList<long> authorsId);
        Task<bool> DeleteByIdAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate);
        Task<GenericModel<AuthorDataModel>> GetAuthorsFilteredDataAsync(BaseFilterModel filter);
        Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin);
    }
}
