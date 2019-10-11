using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository
    {
        Task EditPrintingEditionAuthorsAsync(PrintingEdition printingEdition, IList<long> authorsId);
        Task AddToPrintingEditionAuthorsAsync(PrintingEdition printingEdition, ICollection<long> authors);
        Task<IList<string>> GetAuthorsInOnePEAsync(long printingEditionId);
        Task DeleteWhereAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate);
    }
}
