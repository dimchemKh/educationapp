using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using EducationApp.DataAccessLayer.Repository.Base;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : BaseEFRepository<AuthorInPrintingEdition>, IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<int> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
        {
            var authorsInPrintingEdition = _context.AuthorInPrintingEditions
                .Where(x => x.PrintingEditionId.Equals(printingEditionId));

            var result = await authorsInPrintingEdition
                .Select(x => x.AuthorId)
                .ToArrayAsync();

            var isEqual = result.SequenceEqual(authorsId.OrderBy(x => x));

            if (isEqual)
            {
                return 0;
            }

            var removeRange = await authorsInPrintingEdition.ToArrayAsync();

            _context.AuthorInPrintingEditions.RemoveRange(removeRange);

            var createResult = await CreateAuthorsInPrintingEditionAsync(printingEditionId, authorsId);

            return createResult;

        }
        public async Task<int> CreateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
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

            return await SaveAsync();
        }
        // TODO: when remove Author too need remove PE with this the last Author
        public async Task<bool> DeleteAuthorsById(long authorsId)
        {
            var query = await _context.AuthorInPrintingEditions
                .Where(x => x.AuthorId == authorsId)
                .ToListAsync();

            _context.AuthorInPrintingEditions.RemoveRange(query);
            return true;
        }
        public async Task<bool> DeletePrintingEditionsById(long printingEditionId)
        {
            var query = await _context.AuthorInPrintingEditions
                .Where(x => x.PrintingEditionId == printingEditionId)
                .ToListAsync();

            _context.AuthorInPrintingEditions.RemoveRange(query);
            return true;
        }
    }
}
