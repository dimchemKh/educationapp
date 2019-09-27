using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Common.Constants;

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorInBooksRepository _authorInBooksRepository;
        
        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInBooksRepository authorInBooksRepository)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInBooksRepository = authorInBooksRepository;
        }

        public async Task<PrintingEditionsModel> GetPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, Enums.StateSort stateSort)
        {
            var listBooks = await _printingEditionRepository.ListAsync();
            IOrderedEnumerable<PrintingEdition> query = null;

            switch (stateSort)
            {
                case Enums.StateSort.None:
                    break;
                case Enums.StateSort.PriceAsc:
                    query = listBooks.OrderBy(x => x.Price);
                    break;
                case Enums.StateSort.PriceDesc:
                    query = listBooks.OrderByDescending(x => x.Price);
                    break;
                default:
                    break;
            }

            foreach (var book in query)
            {
                printingEditionsModel.Items.Add(new PrintingEditionsModelItem()
                {
                    Author = "Author???",
                    Name = book.Name,
                    Price = book.Price
                });
            }

            return printingEditionsModel;
        }

        public async Task<PrintingEditionsModel> GetFilteringPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, FilterModel filterModel)
        {
            IEnumerable<PrintingEdition> result = null;
            if (string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                result = await _printingEditionRepository.ListAsync(x => x.Name.Contains(filterModel.SearchByWord));
            }
                        
            foreach (var book in result)
            {
                printingEditionsModel.Items.Add(new PrintingEditionsModelItem()
                {
                    Name = book.Name,
                    Price = book.Price
                });
            }
            return printingEditionsModel;
        } 
    }
}
