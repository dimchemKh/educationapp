using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
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
    public class PrintingEditionRepository : BaseEFRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(ApplicationContext context) : base(context)
        {            
        }

        public async Task<IQueryable<PrintingEdition>> GetIncludeAsync()
        {
            var res = _context.PrintingEditions.Include(x => x.AuthorInBooks).ThenInclude(p => p.Author);
            //var res = await _context.PrintingEditions.Select(predicate).ToListAsync();

            return res;
        }
        //public Task<IQueryable<PrintingEdition>> GetPrintingEditionsAsync(Expression<Func<PrintingEdition, bool>> predicate1) => throw new NotImplementedException();

        //public async Task<IQueryable> GetAuthorsInBooks()
        //{
        //var q = _context.PrintingEditions.
        //return q;
        //}
        //public async Task<IQueryable<PrintingEdition>> GetPrintingEditionsAsync(Expression<Func<PrintingEdition, bool>> predicate1)
        //{
        //    var book = _context.PrintingEditions.Include(p => p.AuthorInBooks).Single(x => x.Name.Contains("avt"));
        //    book.


        //    return query;    
        //}
    }
}
