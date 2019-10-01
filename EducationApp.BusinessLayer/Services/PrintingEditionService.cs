using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Collections.Generic;
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
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
        }
        private IOrderedEnumerable<PrintingEdition> SortingList(AdminFilterModel filterModel, IEnumerable<PrintingEdition> printingEditions)
        {
            IOrderedEnumerable<PrintingEdition> filteringListBooks = null;

            if (filterModel.StateSort == Enums.StateSort.PriceAsc)
            {
                filteringListBooks = printingEditions.OrderBy(x => x.Price);
            }
            if (filterModel.StateSort == Enums.StateSort.PriceDesc)
            {
                filteringListBooks = printingEditions.OrderByDescending(x => x.Price);
            }
            if (filterModel.StateSort == Enums.StateSort.IdAsc)
            {
                filteringListBooks = printingEditions.OrderBy(x => x.Id);
            }
            if (filterModel.StateSort == Enums.StateSort.IdDesc)
            {
                filteringListBooks = printingEditions.OrderByDescending(x => x.Id);
            }
            if (filterModel.StateSort == Enums.StateSort.BookAsc)
            {
                filteringListBooks = printingEditions.OrderBy(x => x.Type);
            }
            if (filterModel.StateSort == Enums.StateSort.BookDesc)
            {
                filteringListBooks = printingEditions.OrderByDescending(x => x.Type);
            }

            return filteringListBooks;
        }
        private decimal GetOrderCurrency(FilterModel filterModel)
        {
            decimal currencyRate = 1.0m;

            if (filterModel.Currency == Enums.Currency.UAH)
            {
                currencyRate = Constants.CurrencyRates.UAH;
            }
            if (filterModel.Currency == Enums.Currency.CHF)
            {
                currencyRate = Constants.CurrencyRates.CHF;
            }
            if (filterModel.Currency == Enums.Currency.EUR)
            {
                currencyRate = Constants.CurrencyRates.EUR;
            }
            if (filterModel.Currency == Enums.Currency.GBP)
            {
                currencyRate = Constants.CurrencyRates.GBP;
            }
            if (filterModel.Currency == Enums.Currency.JPY)
            {
                currencyRate = Constants.CurrencyRates.JPY;
            }
            return currencyRate;
        }
        private async Task<PrintingEditionsModel> AddItemsToModel(PrintingEditionsModel printingEditionsModel, List<PrintingEdition> listItems, decimal currencyRate = Constants.CurrencyRates.USD)
        {
            foreach (var book in listItems)
            {
                printingEditionsModel.Items.Add(new PrintingEditionsModelItem()
                {
                    Id = book.Id,
                    AuthorsNames = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(book),
                    Title = book.Name,
                    Price = book.Price*currencyRate,
                    Description = book.Description
                });
            }
            return printingEditionsModel;
        }
        public async Task<PrintingEditionsModel> AddNewPrintingEditionAsync(PrintingEditionsModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionsModel();
            if (printingEditionsModelItem == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidModel);
                return responseModel;
            }
            if (string.IsNullOrWhiteSpace(printingEditionsModelItem.Title)
                || string.IsNullOrWhiteSpace(printingEditionsModelItem.Description)
                || printingEditionsModelItem.Type == Enums.Type.None
                || !printingEditionsModelItem.AuthorsId.Any()
                || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            
            var printingEdition = new PrintingEdition()
            {
                Name = printingEditionsModelItem.Title,
                Description = printingEditionsModelItem.Description,
                Type = printingEditionsModelItem.Type,
                Price = printingEditionsModelItem.Price,
                Currency = printingEditionsModelItem.Currency
            };
                 
            if(await _authorInPrintingEditionRepository.AddToPrintingEditionAuthors(printingEdition, printingEditionsModelItem.AuthorsId))
            {
                await _printingEditionRepository.AddAsync(printingEdition);
                await _printingEditionRepository.SaveAsync();
            }

            return responseModel;
        }
        public async Task<PrintingEditionsModel> GetUsersPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, FilterModel filterModel)
        {
            var _pageSize = (int)Enums.PageSizes.Six;

            IEnumerable<PrintingEdition> printingEditions = null;

            if (filterModel == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidModel);
                return printingEditionsModel;
            }                       
            if (string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                printingEditions = await _printingEditionRepository.GetWhereAsync(x => x.IsRemoved == false);
            }
            if (!string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                printingEditions = await _printingEditionRepository.GetWhereAsync(x => x.IsRemoved == false && x.Name.Contains(filterModel.SearchByWord));
            }

            var currencyRate = GetOrderCurrency(filterModel);

            if (filterModel.Types != null)
            {
                printingEditions = printingEditions.Where(x => filterModel.Types.Contains(x.Type))
                    .Where(x => x.Price >= filterModel.RangePrice[Enums.RangePrice.MinValue]/currencyRate
                                              && x.Price <= filterModel.RangePrice[Enums.RangePrice.MaxValue]/currencyRate);
            }

            var filteringListBooks = SortingList(filterModel, printingEditions);

            var items = filteringListBooks.Skip((filterModel.Page - 1) * _pageSize).Take(_pageSize).ToList();

            return await AddItemsToModel(printingEditionsModel, items, currencyRate);
        }
        public async Task<PrintingEditionsModel> GetAdminPrintingEditionModelAsync(PrintingEditionsModel printingEditionsModel, AdminFilterModel filterModel)
        {
            var _pageSize = (int)Enums.PageSizes.Six;

            var printingEditions = await _printingEditionRepository.GetWhereAsync(x => x.IsRemoved == false);

            if (filterModel.Types != null)
            {
                printingEditions = printingEditions.Where(x => filterModel.Types.Contains(x.Type));
            }

            var filteringListBooks = SortingList(filterModel, printingEditions);

            var items = filteringListBooks.Skip((filterModel.Page - 1) * _pageSize).Take(_pageSize).ToList();

            return await AddItemsToModel(printingEditionsModel, items);
        }

        public async Task<PrintingEditionsModel> GetUserPrintingEditionPageAsync(PrintingEditionsModel printingEditionsModel, int printingEditionId)
        {
            
            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionId);

            if(printingEdition == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }

            printingEditionsModel.Items.Add(new PrintingEditionsModelItem()
            {
                Title = printingEdition.Name,
                AuthorsNames = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(printingEdition),
                Description = printingEdition.Description,
                Price = printingEdition.Price
            });

            return printingEditionsModel;
        }
    }
}
