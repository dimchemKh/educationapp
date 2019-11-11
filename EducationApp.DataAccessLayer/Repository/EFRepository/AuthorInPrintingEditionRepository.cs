using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : BaseEFRepository<AuthorInPrintingEdition>, IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin)
        {            
            var printingEditions = _context.AuthorInPrintingEditions
                //.AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => x.IsRemoved == false)
                .GroupBy(x => x.PrintingEditionId)
                .Select(x => new PrintingEditionDataModel
                {
                    Id = x.Key,
                    Currency = x.Select(z => z.PrintingEdition.Currency).FirstOrDefault(),
                    Price = x.Select(z => z.PrintingEdition.Price).FirstOrDefault(),
                    PrintingEditionType = x.Select(z => z.PrintingEdition.PrintingEditionType).FirstOrDefault(),
                    Title = x.Select(z => z.PrintingEdition.Title).FirstOrDefault(),
                    Description = x.Select(z => z.PrintingEdition.Description).FirstOrDefault(),
                    Authors = x.Select(z => new AuthorDataModel
                    {
                        Id = z.Author.Id,
                        Name = z.Author.Name
                    }).ToArray()
                });
                       
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                printingEditions = printingEditions.Where(x => x.Title.ToLower().StartsWith(filter.SearchString.ToLower()));
            }
            if (filter.PrintingEditionTypes.Any())
            {
                printingEditions = printingEditions.Where(x => filter.PrintingEditionTypes.Contains(x.PrintingEditionType));
            }
            if (!isAdmin)
            {
                printingEditions = printingEditions.Where(x => x.Price >= filter.PriceMinValue && x.Price <= filter.PriceMaxValue);
            }

            Expression<Func<PrintingEditionDataModel, object>> predicate = x => x.Id;

            if (filter.SortType.Equals(Enums.SortType.Title))
            {
                predicate = x => x.Title;
            }
            if (filter.SortType.Equals(Enums.SortType.Price))
            {
                predicate = x => x.Price;
            }

            var responseModel = new GenericModel<PrintingEditionDataModel>();

            responseModel.CollectionCount = await _context.AuthorInPrintingEditions
                //.AsNoTracking()
                .Include(x => x.PrintingEdition)
                .Where(x => x.PrintingEdition.IsRemoved == false)
                .GroupBy(x => x.PrintingEdition.Id)
                .Select(x => x.Key)
                .CountAsync();

            var res = await PaginationAsync(filter, predicate, printingEditions);

            responseModel.Collection.AddRange(res);

            return responseModel;
        }
        public async Task<GenericModel<AuthorDataModel>> GetAuthorsFilteredDataAsync(BaseFilterModel filter)
        {
            var authors = _context.AuthorInPrintingEditions
                //.AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => x.IsRemoved == false)
                .GroupBy(x => x.AuthorId)
                .Select(group => new AuthorDataModel
                {
                    Id = group.Key,
                    Name = group.Select(x => x.Author.Name).FirstOrDefault(),
                    PrintingEditionTitles = group.Select(z => z.PrintingEdition.Title).ToList()
                });

            Expression<Func<AuthorDataModel, object>> predicate = x => x.Id;

            if (filter.SortType == Enums.SortType.Name)
            {
                predicate = x => x.Name;
            }

            var responseModel = new GenericModel<AuthorDataModel>()
            {
                CollectionCount = await _context.AuthorInPrintingEditions
                .AsNoTracking()
                .Where(x => x.Author.IsRemoved == false)
                .GroupBy(x => x.Author.Id)
                .Select(x => x.Key)
                .CountAsync()
            };

            responseModel.Collection.AddRange(await PaginationAsync(filter, predicate, authors));

            return responseModel;
        }

        public async Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
        {
            var authorsInPrintingEdition = _context.AuthorInPrintingEditions
                //.AsNoTracking()
                .Where(x => x.IsRemoved == false)
                .Include(x => x.Author)
                .Where(x => x.PrintingEditionId.Equals(printingEditionId));

            var result = await authorsInPrintingEdition.Select(x => x.AuthorId).ToArrayAsync();

            var isEqual = result.SequenceEqual(authorsId.OrderBy(x => x));

            if (isEqual)
            {
                return false;
            }

            var removeRange = await authorsInPrintingEdition.ToArrayAsync();

            foreach (var item in removeRange)
            {
                item.IsRemoved = true;
            }

            await _context.SaveChangesAsync();

            await AddAuthorsInPrintingEditionAsync(printingEditionId, authorsId);

            return true;
        }
        public async Task<bool> AddAuthorsInPrintingEditionAsync(long printingEditionId, IList<long> authorsId)
        {
            var authorInPrintingEditions = new List<AuthorInPrintingEdition>();

            for (int i = 0; i < authorsId.Count; i++)
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
            var authorsInPe = await _context.AuthorInPrintingEditions
                .AsNoTracking()
                .Where(x => x.IsRemoved == false)
                .Where(predicate)
                .ToListAsync();

            foreach (var item in authorsInPe)
            {
                item.IsRemoved = true;                
            }

            var printingEditionGroup = await _context.AuthorInPrintingEditions
                .AsNoTracking()
                .Where(x => x.IsRemoved == false)
                .Where(predicate)
                .GroupBy(x => x.PrintingEditionId)
                .Select(x => new
                {
                    printingEditionId = x.Key,
                    authorsId = x.Select(z => z.AuthorId).ToArray()
                }).ToListAsync();

            if(printingEditionGroup.Count == 1)
            {
                
            }


            await _context.SaveChangesAsync();

            return true;
        }
    }
}
