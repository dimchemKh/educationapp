using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : IAuthorInPrintingEditionRepository
    {
        private readonly ApplicationContext _context;

        public AuthorInPrintingEditionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToPrintingEditionAuthors(PrintingEdition printingEdition, ICollection<int> authorsId)
        {
            foreach (var id in  authorsId)
            {
                var author = await _context.Authors.FindAsync(id);
                printingEdition.AuthorInPrintingEdition.Add(new AuthorInPrintingEdition() { PrintingEdition = printingEdition, Author = author });
            }

            return true;
        }

        public async Task<IList<string>> GetPrintingEditionAuthorsListAsync(PrintingEdition printingEdition)
        {
            IList<string> listAuthors = new List<string>();

            var query = _context.AuthorInPrintingEditions.Where(x => x.PrintingEdition == printingEdition);

            foreach (var item in query)
            {
                var q = await _context.Authors.Where(x => x.Id == item.AuthorId).Select(z => z.Name).ToListAsync();
                listAuthors.Add(q.First());
            }

            return listAuthors;
        }
    }
}
