using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository // : IBaseEFRepository<AuthorInPrintingEdition>
    {
        
        Task<bool> AddToPrintingEditionAuthors(PrintingEdition printingEdition, ICollection<int> authors);

        Task<IList<string>> GetPrintingEditionAuthorsListAsync(PrintingEdition printingEdition);
    }
}
