using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Models;
using System;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : IAuthorInPrintingEditionRepository
    {
        private readonly ApplicationContext _context;

        public AuthorInPrintingEditionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task EditPrintingEditionAuthorsAsync(PrintingEdition printingEdition, IList<long> authorsId)
        {
            var query = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId == printingEdition.Id).ToListAsync();
            _context.RemoveRange(query);

            await AddToPrintingEditionAuthorsAsync(printingEdition, authorsId);
        }
        public async Task AddToPrintingEditionAuthorsAsync(PrintingEdition printingEdition, ICollection<long> authorsId)
        {
            foreach (var authorId in  authorsId)
            {
                var author = await _context.Authors.FindAsync(authorId);
                printingEdition.AuthorInPrintingEditions.Add(new AuthorInPrintingEdition() { PrintingEdition = printingEdition, Author = author });
            }
        }
        public async Task<IList<PrintingEditionsInAuthorModel>> GetPEsInAuthorAsync(IEnumerable<Author> authors)
        {
            var groupList = await _context.AuthorInPrintingEditions.Include(x => x.Author).Include(z => z.PrintingEdition).Where(x => authors.Any(z => z.Id == x.AuthorId))
                                                                    .GroupBy(x => x.AuthorId)
                                                                    .Select(group => new PrintingEditionsInAuthorModel
                                                                    {
                                                                        Id = group.Key,
                                                                        Name = group.Select(element => element.Author.Name).FirstOrDefault(),
                                                                        PrintingEditionTitles = group.Select(z => z.PrintingEdition.Title).ToList()
                                                                    }).ToListAsync();                
            
            return groupList;
        }
        public async Task<IList<string>> GetAuthorsInOnePEAsync(long printingEditionId)
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
