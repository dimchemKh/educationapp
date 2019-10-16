using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository
    {
        Task EditAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<long> authorsId);
        Task AddAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, ICollection<long> authors);
        Task<IList<string>> GetAuthorsInOnePrintingEditionAsync(long printingEditionId);
        Task DeleteWhereAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate);
    }
}
