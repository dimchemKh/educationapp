using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EducationApp.DataAccessLayer.Entities.Base;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : IAuthorInPrintingEditionRepository
    {
        private readonly ApplicationContext _context;

        public AuthorInPrintingEditionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> EditPrintingEditionAuthorsAsync(PrintingEdition printingEdition, IList<int> authorsId)
        {
            var query = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId == printingEdition.Id).ToListAsync();
            _context.RemoveRange(query);
            if(await AddToPrintingEditionAuthorsAsync(printingEdition, authorsId))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AddToPrintingEditionAuthorsAsync(PrintingEdition printingEdition, ICollection<int> authorsId)
        {
            foreach (var authorId in  authorsId)
            {
                var author = await _context.Authors.FindAsync(authorId);
                printingEdition.AuthorInPrintingEditions.Add(new AuthorInPrintingEdition() { PrintingEdition = printingEdition, Author = author });
            }
            
            return true;
        }
    }
}
