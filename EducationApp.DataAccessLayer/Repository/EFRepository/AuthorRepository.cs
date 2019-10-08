using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorRepository : BaseEFRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {            
        }
        public async Task<ICollection<string>> GetPrintingEditionsInAuthorAsync(Author author)
        {
            var query = await _context.AuthorInPrintingEditions.Where(x => x.Author == author).Include(z => z.PrintingEdition).ToListAsync();

            ICollection<string> printingEditionsList = null;

            foreach (var item in query)
            {
                printingEditionsList.Add(item.PrintingEdition.Title);
            }
            return printingEditionsList;
        }
    }
}
