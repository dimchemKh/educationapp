using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using EducationApp.DataAccessLayer.Repository.Interfaces;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : IAuthorInPrintingEditionRepository
    {
        public readonly ApplicationContext _context;
        public AuthorInPrintingEditionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
        {
            var authorsInPrintingEdition = _context.AuthorInPrintingEditions
                .Where(x => x.PrintingEditionId.Equals(printingEditionId));

            var result = await authorsInPrintingEdition.Select(x => x.AuthorId).ToArrayAsync();
            var isEqual = result.SequenceEqual(authorsId.OrderBy(x => x));

            if (isEqual)
            {
                return false;
            }

            var removeRange = await authorsInPrintingEdition.ToArrayAsync();

            _context.AuthorInPrintingEditions.RemoveRange(removeRange);

            await AddAuthorsInPrintingEditionAsync(printingEditionId, authorsId);

            return true;
        }
        public async Task<bool> AddAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
        {
            var authorInPrintingEditions = new List<AuthorInPrintingEdition>();

            for (int i = 0; i < authorsId.Length; i++)
            {
                authorInPrintingEditions.Add(new AuthorInPrintingEdition()
                {
                    AuthorId = authorsId[i],
                    PrintingEditionId = printingEditionId
                });
            }
            await _context.AuthorInPrintingEditions.AddRangeAsync(authorInPrintingEditions);
            await _context.SaveChangesAsync();
            return true;
        }
        // TODO: when remove Author too need remove PE with this the last Author
        public async Task<bool> DeleteByIdAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate)
        {
            var query = _context.AuthorInPrintingEditions
                .Include(x => x.PrintingEdition)
                .Include(x => x.Author)
                .Where(predicate);

            _context.AuthorInPrintingEditions.RemoveRange(query);

            //var res = query
            //    .GroupBy(x => x.PrintingEdition)
            //    .Select(x => x.Key);

            //var count = res.Count();
            //if (count == 1)
            //{
            //    var pe = res.FirstOrDefault();
            //    pe.IsRemoved = true;
            //}
            //if(count > 1)
            //{
            //    res.Where(x => x.AuthorInPrintingEditions.Select(z => z.PrintingEditionId == x.Id))
            //}
           

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
