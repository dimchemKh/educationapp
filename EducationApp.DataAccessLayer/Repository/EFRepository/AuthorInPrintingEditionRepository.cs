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
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Models.Filters;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : BaseEFRepository<AuthorInPrintingEdition>, IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<IEnumerable<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin)
        {
            var printingEditions = _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => x.PrintingEdition.IsRemoved.Equals(false) && x.IsRemoved.Equals(false))
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
                        Id = z.Id,
                        Name = z.Author.Name
                    }).ToList()
                });

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                printingEditions = printingEditions.Where(x => x.Title.Contains(filter.SearchString));
            }

            printingEditions = printingEditions.Where(x => filter.PrintingEditionTypes.Contains(x.PrintingEditionType));

            if (!isAdmin)
            {
                printingEditions = printingEditions.Where(x => x.Price >= filter.PriceMinValue && x.Price <= filter.PriceMaxValue);
            }

            Expression<Func<PrintingEditionDataModel, object>> expression = null;
            if (filter.SortType.Equals(Enums.SortType.Id))
            {
                expression = x => x.Id;
            }
            if (filter.SortType.Equals(Enums.SortType.PrintingEditionType))
            {
                expression = x => x.PrintingEditionType;
            }
            if (filter.SortType.Equals(Enums.SortType.Price))
            {
                expression = x => x.Price;
            }
            var result = await PaginationAsync(filter, expression, printingEditions);
            return result;
        }
        public async Task<IEnumerable<AuthorDataModel>> GetAuthorsFilteredDataAsync(BaseFilterModel filter)
        {
            var authors = _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => x.Author.IsRemoved == false)
                .GroupBy(x => x.AuthorId)
                .Select(group => new AuthorDataModel
                {
                    Id = group.Key,
                    Name = group.Select(x => x.Author.Name).FirstOrDefault(),
                    PrintingEditionTitles = group.Select(z => z.PrintingEdition.Title).ToList()
                });
            Expression<Func<AuthorDataModel, object>> expression = null;
            if (filter.SortType.Equals(Enums.SortType.Id))
            {
                expression = x => x.Id;
            }
            if (filter.SortType.Equals(Enums.SortType.Name))
            {
                expression = x => x.Name;
            }
            var result = await PaginationAsync(filter, expression, authors);
            return result;
        } 

        public async Task<bool> UpdateAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<long> authorsId)
        {
            var queryAuthorsList = await _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Where(x => x.PrintingEditionId.Equals(printingEdition.Id))
                .GroupBy(x => x.Author)
                .Select(x => x.Key).ToListAsync();
            var list = queryAuthorsList.Select(x => x.Id).ToList();
            var isEqual = list.SequenceEqual(authorsId.OrderBy(x => x));
            if (isEqual)
            {
                return false;
            }
            var removeRange = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId.Equals(printingEdition.Id)).ToListAsync();
            foreach (var item in removeRange)
            {
                item.IsRemoved = true;
            }
            _context.AuthorInPrintingEditions.UpdateRange(removeRange);
            await AddAuthorsInPrintingEditionAsync(printingEdition, authorsId);
            return true;
        }
        public async Task AddAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<long> authorsId)
        {
            var query = await _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => authorsId.Contains(x.Author.Id))
                .GroupBy(x => x.Author)
                .Select(x => x.Key)
                .ToListAsync();
            var tempList = query.Select(z => new AuthorInPrintingEdition
            {
                AuthorId = z.Id,
                PrintingEdition = printingEdition
            }).ToList();
            await _context.AddRangeAsync(tempList);
            await _context.SaveChangesAsync();
        }        
        public async Task<IList<AuthorDataModel>> GetAuthorsInOnePrintingEditionAsync(long printingEditionId)
        {
            var list = await _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => x.PrintingEditionId == printingEditionId)
                .Select(z => new AuthorDataModel
                {
                    Name = z.Author.Name
                }).ToListAsync();

            return list;
        }
        public async Task<bool> DeleteByAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate)
        {
            var list = await _context.AuthorInPrintingEditions.Where(predicate).ToListAsync();
            if(list == null)
            {
                return false;
            }
            _context.AuthorInPrintingEditions.AttachRange(list);
            foreach (var item in list)
            {
                item.IsRemoved = true;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
