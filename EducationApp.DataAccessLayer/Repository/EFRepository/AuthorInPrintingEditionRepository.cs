using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Models;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : IAuthorInPrintingEditionRepository
    {
        private readonly ApplicationContext _context;

        public AuthorInPrintingEditionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> EditPrintingEditionAuthorsAsync(PrintingEdition printingEdition, IList<long> authorsId)
        {
            var query = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId == printingEdition.Id).ToListAsync();
            _context.RemoveRange(query);
            if(await AddToPrintingEditionAuthorsAsync(printingEdition, authorsId))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AddToPrintingEditionAuthorsAsync(PrintingEdition printingEdition, ICollection<long> authorsId)
        {
            foreach (var authorId in  authorsId)
            {
                var author = await _context.Authors.FindAsync(authorId);
                printingEdition.AuthorInPrintingEditions.Add(new AuthorInPrintingEdition() { PrintingEdition = printingEdition, Author = author });
            }
            
            return true;
        }
        public async Task<object> GetAuthorsInPEAsync()
        {
            var groupList = await _context.AuthorInPrintingEditions.Include(x => x.Author).Include(z => z.PrintingEdition)
                                                                    .GroupBy(x => x.AuthorId)
                                                                    .Select(group => new AuthorInPrintingEditionModel
                                                                    {
                                                                        AuthorId = group.Key,
                                                                        AuthorName = group.Select(element => element.Author.Name).FirstOrDefault(),
                                                                        PrintingEditionTitle = group.Select(z => z.PrintingEdition.Title).ToList()
                                                                    }).ToListAsync();                
            
            return groupList;
        }
    }
}
