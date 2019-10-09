using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository // : IBaseEFRepository<AuthorInPrintingEdition>
    {
        Task<bool> EditPrintingEditionAuthorsAsync(PrintingEdition printingEdition, IList<long> authorsId);
        Task<bool> AddToPrintingEditionAuthorsAsync(PrintingEdition printingEdition, ICollection<long> authors);
        Task<object> GetAuthorsInPEAsync();
    }
}
