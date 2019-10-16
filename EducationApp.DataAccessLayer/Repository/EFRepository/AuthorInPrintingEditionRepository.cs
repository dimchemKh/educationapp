using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : IAuthorInPrintingEditionRepository
    {
        private readonly ApplicationContext _context;

        public AuthorInPrintingEditionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task EditAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<long> authorsId)
        {
            var query = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId == printingEdition.Id).ToListAsync();
            _context.RemoveRange(query);

            await AddAuthorsInPrintingEditionAsync(printingEdition, authorsId);
        }
        public async Task AddAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, ICollection<long> authorsId)
        {
            foreach (var authorId in  authorsId)
            {
                var author = await _context.Authors.FindAsync(authorId);
                printingEdition.AuthorInPrintingEditions.Add(new AuthorInPrintingEdition() { PrintingEdition = printingEdition, Author = author });
            }
        }        
        public async Task<IList<string>> GetAuthorsInOnePrintingEditionAsync(long printingEditionId)
        {
            var list = await _context.AuthorInPrintingEditions.Include(x => x.Author).Include(x => x.PrintingEdition)
                                                            .Where(x => x.PrintingEditionId == printingEditionId)
                                                            .Select(z => z.Author.Name).ToListAsync();

            return list;
        }
        public async Task DeleteWhereAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate)
        {
            var list = _context.AuthorInPrintingEditions.Where(predicate);
            _context.AuthorInPrintingEditions.RemoveRange(list);

            await _context.SaveChangesAsync();
        }
    }
}
