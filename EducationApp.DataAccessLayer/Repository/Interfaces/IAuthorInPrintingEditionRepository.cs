using EducationApp.DataAccessLayer.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository
    {
        Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> AddAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> DeleteByIdAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate);
    }
}
