using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository // : IBaseEFRepository<AuthorInPrintingEdition>
    {
        Task<bool> EditPrintingEditionAuthorsAsync(PrintingEdition printingEdition, IList<int> authorsId);
        Task<bool> AddToPrintingEditionAuthorsAsync(PrintingEdition printingEdition, ICollection<int> authors);
        Task<ICollection<string>> GetPrintingEditionAuthorsListAsync(BaseEntity baseEnity);
    }
}
