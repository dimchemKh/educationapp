using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
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

        public async Task<bool> IsExistedPrintingEdition(string printingEditionName)
        {
            if(await _context.PrintingEditions.Where(x => x.Title == printingEditionName).AnyAsync())
            {
                return true;
            }
            return false;
        }
        public IQueryable<PrintingEdition> FiteringFromSearchWord(string searchByWord, IQueryable<PrintingEdition> printingEditions)
        {
            if (string.IsNullOrWhiteSpace(searchByWord))
            {
                printingEditions = ReadAll();
            }
            if (!string.IsNullOrWhiteSpace(searchByWord))
            {
                printingEditions = ReadWhere(x => x.Title.Contains(searchByWord));
            }
            return printingEditions;
        }
        public IQueryable<PrintingEdition> FilteringByTypes (ICollection<Enums.PrintingEditionType> types, IQueryable<PrintingEdition> printingEditions)
        {          
            return printingEditions.Where(x => types.Contains(x.PrintingEditionType));
        }
        public IQueryable<PrintingEdition> FilteringByPrice(IDictionary<Enums.RangePrice, decimal> rangePrice, IQueryable<PrintingEdition> printingEditions)
        {
            return printingEditions.Where(x => x.Price >= rangePrice[Enums.RangePrice.MinValue]
                                                 && x.Price <= rangePrice[Enums.RangePrice.MaxValue]);
        }
        
        public async Task<ICollection<string>> GetAuthorsInPrintingEditionAsync(PrintingEdition printingEdition)
        {
            var query = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEdition == printingEdition).Include(z => z.Author).ToListAsync();

            ICollection<string> authorsList = null;
                        
            foreach (var item in query)
            {
                authorsList.Add(item.Author.Name);
            }
            return authorsList;
        }
    }
}
