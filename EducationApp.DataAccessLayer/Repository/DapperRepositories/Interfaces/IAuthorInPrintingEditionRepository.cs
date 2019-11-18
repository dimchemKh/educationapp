using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces
{
    public interface IAuthorInPrintingEditionRepository /*: IBaseDapperRepository<AuthorInPrintingEdition>*/
    {
        Task<bool> AddAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> DeleteByIdAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate);
    }
}
