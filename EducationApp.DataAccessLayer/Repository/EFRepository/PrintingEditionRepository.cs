using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<bool> IsExistedPrintingEdition(DAPrintingEditionModel printingEdition)
        {

            var result = await _context.AuthorInPrintingEditions.Include(x => x.PrintingEdition).Include(z => z.Author).GroupBy(x => x.PrintingEdition)
                                                    .Where(x => x.Key.Title == printingEdition.Title && x.Key.PrintingEditionType == printingEdition.PrintingEditionType)
                                                    .Select(z => new DAPrintingEditionShortModel
                                                    {
                                                        Id = z.Key.Id,
                                                        AuthorsId = z.Select(x => x.Author.Id).ToList()
                                                    }).ToListAsync();
            if(result == null)
            {
                return false;
            }
            foreach (var item in result)
            {
                var res2 = item.AuthorsId.Except(printingEdition.AuthorsId);
                var res1 = printingEdition.AuthorsId.Except(item.AuthorsId);
                if (!res1.Any() && !res2.Any())
                { 
                    return true;
                }                
            }
            return false;           
        }
        
        public async Task<IEnumerable<DAPrintingEditionModel>> FilteringAsync(FilterPrintingEditionModel filter)
        {           
            IQueryable<DAPrintingEditionModel> printingEditions = _context.AuthorInPrintingEditions.Include(x => x.Author).Include(x => x.PrintingEdition).GroupBy(x => x.PrintingEditionId)
                                                                                            .Select(x => new DAPrintingEditionModel
                                                                                            {
                                                                                                Id = x.Key,
                                                                                                Currency = x.Select(z => z.PrintingEdition.Currency).FirstOrDefault(),
                                                                                                Price = x.Select(z => z.PrintingEdition.Price).FirstOrDefault(),
                                                                                                PrintingEditionType = x.Select(z => z.PrintingEdition.PrintingEditionType).FirstOrDefault(),
                                                                                                Title = x.Select(z => z.PrintingEdition.Title).FirstOrDefault(),
                                                                                                Description = x.Select(z => z.PrintingEdition.Description).FirstOrDefault(),
                                                                                                AuthorNames = x.Select(z => z.Author.Name).ToList()
                                                                                            });

            if (!string.IsNullOrWhiteSpace(filter.SearchByBody))
            {
                printingEditions = printingEditions.Where(x => x.Title.Contains(filter.SearchByBody));
            }

            printingEditions = printingEditions.Where(x => filter.PrintingEditionTypes.Contains(x.PrintingEditionType));

            // TODO: Price filter don't working right

            //printingEditions = printingEditions.Where(x => x.Price >= filter.RangePrice.FirstOrDefault() && x.Price <= filter.RangePrice.LastOrDefault());

            Expression<Func<DAPrintingEditionModel, object>> lambda = null;
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

            foreach (var item in result)
            {
                item.Price = Converting(item.Currency, filter.Currency, item.Price);
            }

            return result;
        }
        private decimal Converting(Enums.Currency fromCurrency, Enums.Currency toCurrency, decimal result)
        {
            decimal valueFrom = 0;
            decimal valueTo = 0;
            foreach (var item in Constants.CurrencyRates.ConverterList)
            {
                if (item.Key == fromCurrency)
                {
                    valueFrom = item.Value;
                };
                if (item.Key == toCurrency)
                {
                    valueTo = item.Value;
                };
            }
            return valueFrom / valueTo * result;
        }
    }
}
