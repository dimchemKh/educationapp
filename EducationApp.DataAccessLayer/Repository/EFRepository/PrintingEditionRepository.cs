using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Common;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
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
        public async Task<bool> IsExistedPrintingEdition(PrintingEdition printingEdition)
        {
            //var comparer = new EquailtyComparerPrintingEdition();
            var source = _context.PrintingEditions;

            var list = source.Where(z => z.Title == printingEdition.Title).Include(x => x.AuthorInPrintingEditions).ToList();
            //var list = source.Where(x => x.Title == printingEdition.Title && x.IsRemoved == false).SelectMany(
            //    pe => pe.AuthorInPrintingEditions,
            //    (pe, result) => new PrintingEditionModel
            //    {
            //        Title = pe.Title
            //    });
            //_context.PrintingEditions.

            return false;
        }
        public async Task<IEnumerable<PrintingEdition>> Filtering(FilterPrintingEditionModel filter)
        {
            IQueryable<PrintingEdition> printingEditions = null;

            if (string.IsNullOrWhiteSpace(filter.SearchByBody))
            {
                printingEditions = ReadAll();
            }
            if (!string.IsNullOrWhiteSpace(filter.SearchByBody))
            {
                printingEditions = ReadWhere(x => x.Title.Contains(filter.SearchByBody));
            }

            printingEditions = printingEditions.Where(x => filter.PrintingEditionTypes.Contains(x.PrintingEditionType));

            // TODO: Price filter don't working right
            printingEditions = printingEditions.Where(x => x.Price >= filter.RangePrice.FirstOrDefault() && x.Price <= filter.RangePrice.LastOrDefault());

            Expression<Func<PrintingEdition, object>> lambda = null;
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

            if (filter.SortState == Enums.SortState.Asc)
            {
                printingEditions = printingEditions.OrderBy(lambda);
            }
            if (filter.SortState == Enums.SortState.Desc)
            {
                printingEditions = printingEditions.OrderByDescending(lambda);
            }

            return await printingEditions.Skip(filter.Page - 1 * filter.PageSize).Take(filter.PageSize).ToListAsync();
        }
        
        //public async Task<ICollection<string>> GetAuthorsInPrintingEditionAsync(PrintingEdition printingEdition)
        //{
        //    var query = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEdition == printingEdition).Include(z => z.Author).ToListAsync();

        //    ICollection<string> authorsList = null;
                        
        //    foreach (var item in query)
        //    {
        //        authorsList.Add(item.Author.Name);
        //    }
        //    return authorsList;
        //}
    }
}
