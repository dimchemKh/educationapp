using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        
        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository)
        {
            _printingEditionRepository = printingEditionRepository;
        }

        public async Task<PrintingEditionsModel> GetPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, FilterModel filterModel)
        {
            IEnumerable<PrintingEdition> listBooks = null;

            if (filterModel == null)
            {
                return null;
            }

            

            if (string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                listBooks = await _printingEditionRepository.ListAsync(x => x.IsRemoved == false);
            }
            else
            {
                listBooks = await _printingEditionRepository.ListAsync(x => x.IsRemoved == false && x.Name.Contains(filterModel.SearchByWord));
            }

            if (filterModel.Types != null)
            {
                listBooks = listBooks.Where(x => filterModel.Types.Contains(x.Type));
            }

            //listBooks = listBooks.Where(x => filterModel.RangePrice.);

            IOrderedEnumerable<PrintingEdition> query = null;

            switch (filterModel.SortPrice)
            {
                case Enums.StateSort.None:
                    break;
                case Enums.StateSort.PriceAsc:
                    query = listBooks.OrderBy(x => x.Price);
                    break;
                case Enums.StateSort.PriceDesc:
                    query = listBooks.OrderByDescending(x => x.Price);
                    break;
                case Enums.StateSort.IdAsc:
                    query = listBooks.OrderBy(x => x.Id);
                    break;
                case Enums.StateSort.IdDesc:
                    query = listBooks.OrderByDescending(x => x.Id);
                    break;
                case Enums.StateSort.BookAsc:
                    query = listBooks.OrderBy(x => x.Type);
                    break;
                case Enums.StateSort.BookDesc:
                    query = listBooks.OrderByDescending(x => x.Type);
                    break;
                default:
                    break;
            }

            var tempQuery = await _printingEditionRepository.GetIncludeAsync();

            var model = tempQuery.Select(o => new PrintingEditionsModelItem()
            {
                Id = o.Id,
                Name = o.Name,
                Price = o.Price,
                AuthorModels = o.AuthorInBooks.Select(ot => ot.Author).ToList()
            });

            foreach (var item in model)
            {
                printingEditionsModel.Items.Add(item);
            }


            //foreach (var book in query)
            //{
            //    printingEditionsModel.Items.Add(new PrintingEditionsModelItem()
            //    {
            //        Id = book.Id,
            //        AuthorModels = null,
            //        Name = book.Name,
            //        Price = book.Price
            //    });
            //}

            return printingEditionsModel;
        }
    }
}
