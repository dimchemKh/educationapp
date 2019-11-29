using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class PrintingEditionRepository : BaseEFRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin)
        {
            var queryPrintingEditions = _context.PrintingEditions
                .Where(x => x.IsRemoved == false)
                .Include(x => x.AuthorInPrintingEditions)
                .ThenInclude(x => x.Author);

            IQueryable<PrintingEdition> printingEditions = null;

            if (filter.PrintingEditionTypes.Any())
            {
                printingEditions = queryPrintingEditions.Where(x => filter.PrintingEditionTypes.Contains(x.PrintingEditionType));
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                printingEditions = printingEditions
                    .Where(x => x.AuthorInPrintingEditions.Select(z => z.Author.Name.ToLower().StartsWith(filter.SearchString.ToLower())).FirstOrDefault() 
                             || x.Title.ToLower().StartsWith(filter.SearchString.ToLower()));
            }

            Expression<Func<PrintingEdition, object>> predicate = x => x.Id;

            if (!isAdmin)
            {
                printingEditions = printingEditions.Where(x => x.Price >= filter.PriceMinValue && x.Price <= filter.PriceMaxValue);
                predicate = x => x.Price;
            }

            if (filter.SortType.Equals(Enums.SortType.Title))
            {
                predicate = x => x.Title;
            }

            if (filter.SortType.Equals(Enums.SortType.Price))
            {
                predicate = x => x.Price;
            }

            var responseModel = new GenericModel<PrintingEditionDataModel>
            {
                CollectionCount = printingEditions.Count()
            };

            var printingEditionPage = await PaginationAsync(filter, predicate, printingEditions);

            var result = printingEditionPage.Select(x => new PrintingEditionDataModel()
            {
                Id = x.Id,
                Currency = x.Currency,
                Price = x.Price,
                PrintingEditionType = x.PrintingEditionType,
                Title = x.Title,
                Authors = x.AuthorInPrintingEditions
                    .Select(z => new AuthorDataModel()
                    {
                        Id = z.Author.Id,
                        Name = z.Author.Name
                    }).ToArray()
            });

            responseModel.Collection = result;

            return responseModel;
        }
        public async Task<PrintingEditionDataModel> GetPrintingEditionDetailsAsync(long printingEditionid)
        {
            var printingEdition = await _context.PrintingEditions
                .Where(x => x.IsRemoved == false)
                .Where(x => x.Id == printingEditionid)
                .Include(x => x.AuthorInPrintingEditions)
                .ThenInclude(x => x.Author)
                .Select(x => new PrintingEditionDataModel()
                {
                    Id = x.Id,
                    Currency = x.Currency,
                    Price = x.Price,
                    PrintingEditionType = x.PrintingEditionType,
                    Title = x.Title,
                    Description = x.Description,
                    Authors = x.AuthorInPrintingEditions
                    .Select(z => new AuthorDataModel()
                    {
                        Id = z.Author.Id,
                        Name = z.Author.Name
                    }).ToArray()
                }).FirstOrDefaultAsync();

            return printingEdition;
        }
    }
}
