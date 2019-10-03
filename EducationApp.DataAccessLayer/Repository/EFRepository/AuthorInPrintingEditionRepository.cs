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
                printingEdition.AuthorInPrintingEdition.Add(new AuthorInPrintingEdition() { PrintingEdition = printingEdition, Author = author });
            }
            return true;
        }

        public async Task<ICollection<string>> GetPrintingEditionAuthorsListAsync(BaseEntity baseEnity)
        {
            ICollection<string> listEntity = new List<string>();

            if(baseEnity is PrintingEdition)
            {
                var printingEdition = baseEnity as PrintingEdition;
                var query = _context.AuthorInPrintingEditions.Where(x => x.PrintingEdition == printingEdition);

                foreach (var item in query)
                {
                    var listOfOneAuthor = await _context.Authors.Where(x => x.Id == item.AuthorId).Select(z => z.Name).ToListAsync();
                    listEntity.Add(listOfOneAuthor.First());
                }
            }
            if(baseEnity is Author)
            {
                var author = baseEnity as Author;
                var query = _context.AuthorInPrintingEditions.Where(x => x.Author == author);

                foreach (var item in query)
                {
                    var listOfOneAuthor = await _context.PrintingEditions.Where(x => x.Id == item.PrintingEditionId).Select(z => z.Name).ToListAsync();
                    listEntity.Add(listOfOneAuthor.First());
                }
            }

            return listEntity;
        }
    }
}
