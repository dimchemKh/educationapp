using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository
    {
        Task EditPrintingEditionAuthorsAsync(PrintingEdition printingEdition, IList<long> authorsId);
        Task AddToPrintingEditionAuthorsAsync(PrintingEdition printingEdition, ICollection<long> authors);
        Task<IList<PrintingEditionsInAuthorModel>> GetPEsInAuthorAsync(IEnumerable<Author> authors);
        //Task<IList<AuthorsInPrintingEditionModel>> GetAuthorsInPEsAsync(IEnumerable<PrintingEdition> printingEditions);
        Task<IList<string>> GetAuthorsInOnePEAsync(long printingEditionId);
        Task DeleteWhereAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate);
    }
}
