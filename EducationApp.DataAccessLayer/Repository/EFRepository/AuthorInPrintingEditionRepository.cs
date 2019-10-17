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
        public async Task<IEnumerable<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter)
        {
            var printingEditions = _context.AuthorInPrintingEditions.Include(x => x.Author).Include(x => x.PrintingEdition).GroupBy(x => x.PrintingEditionId)
                                        .Select(x => new PrintingEditionDataModel
                                        {
                                            Id = x.Key,
                                            Currency = x.Select(z => z.PrintingEdition.Currency).FirstOrDefault(),
                                            Price = x.Select(z => z.PrintingEdition.Price).FirstOrDefault(),
                                            PrintingEditionType = x.Select(z => z.PrintingEdition.PrintingEditionType).FirstOrDefault(),
                                            Title = x.Select(z => z.PrintingEdition.Title).FirstOrDefault(),
                                            Description = x.Select(z => z.PrintingEdition.Description).FirstOrDefault(),
                                            Authors = x.Select(z => new AuthorDataModel { Name = z.Author.Name }).ToList()
                                        });

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                printingEditions = printingEditions.Where(x => x.Title.Contains(filter.SearchString));
            }

            printingEditions = printingEditions.Where(x => filter.PrintingEditionTypes.Contains(x.PrintingEditionType));

            printingEditions = printingEditions.Where(x => x.Price >= filter.PriceMinValue && x.Price <= filter.PriceMaxValue);

            Expression<Func<PrintingEditionDataModel, object>> lambda = null;
            if (filter.SortType == Enums.SortType.Id)
            {
                lambda = x => x.Id;
            }
            if (filter.SortType == Enums.SortType.PrintingEditionType)
            {
                lambda = x => x.PrintingEditionType;
            }
            if (filter.SortType == Enums.SortType.Price)
            {
                lambda = x => x.Price;
            }

            var result = await PaginationAsync(filter, lambda, printingEditions);

            return result;
        }
        public async Task<IEnumerable<AuthorDataModel>> GetAuthorsFilteredDataAsync(BaseFilterModel filter)
        {
            var authors = _context.AuthorInPrintingEditions.Include(x => x.Author).Include(x => x.PrintingEdition)
                .Where(x => x.Author.IsRemoved == false).GroupBy(x => x.Author)
                .Select(group => new AuthorDataModel
                {
                    Id = group.Key.Id,
                    Name = group.Select(x => x.Author.Name).FirstOrDefault(),
                    PrintingEditionTitles = group.Select(z => z.PrintingEdition.Title).ToList()
                });
            Expression<Func<AuthorDataModel, object>> expression = null;
            if (filter.SortType == Enums.SortType.Id)
            {
                expression = x => x.Id;
            }
            if (filter.SortType == Enums.SortType.Name)
            {
                expression = x => x.Name;
            }
            var result = await PaginationAsync(filter, expression, authors);
            return result;
        } 
        // ???????
        public async Task UpdateAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<AuthorDataModel> authors)
        {
            var query = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId == printingEdition.Id).ToListAsync();

            foreach (var item in query)
            {
                item.IsRemoved = true;
            }
            _context.AuthorInPrintingEditions.UpdateRange(query);

            await AddAuthorsInPrintingEditionAsync(printingEdition, authors);
        }
        public async Task AddAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<AuthorDataModel> authors)
        {
            foreach (var author in authors)
            {
                var findedAuthor = await _context.Authors.FindAsync(author.Id);
                printingEdition.AuthorInPrintingEditions.Add(new AuthorInPrintingEdition() { PrintingEdition = printingEdition, Author = findedAuthor });
            }
        }        
        public async Task<IList<AuthorDataModel>> GetAuthorsInOnePrintingEditionAsync(long printingEditionId)
        {
            var list = await _context.AuthorInPrintingEditions.Include(x => x.Author).Include(x => x.PrintingEdition)
                                                            .Where(x => x.PrintingEditionId == printingEditionId)
                                                            .Select(z => new AuthorDataModel { Name = z.Author.Name }).ToListAsync();

            return list;
        }
        public async Task DeleteByAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate)
        {
            var list = await _context.AuthorInPrintingEditions.Where(predicate).ToListAsync();
            _context.AuthorInPrintingEditions.AttachRange(list);
            foreach (var item in list)
            {
                item.IsRemoved = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
