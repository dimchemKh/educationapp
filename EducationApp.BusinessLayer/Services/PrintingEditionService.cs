using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Common.Constants;
using System;
using EducationApp.BusinessLayer.Helpers.Interfaces;

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        private readonly IConverterHelper _converterHelper;

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, IConverterHelper converterHelper)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _converterHelper = converterHelper;
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
        private async Task<PrintingEditionsModel> AddItemsToModel(PrintingEditionsModel printingEditionsModel, List<PrintingEdition> listItems, bool IsUser)
        {
            if (IsUser)
            {
                foreach (var book in listItems)
                {
                    printingEditionsModel.Items.Add(new PrintingEditionsModelItem()
                    {
                        Id = book.Id,
                        AuthorsNames = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(book),
                        Title = book.Name,
                        Price = book.Price
                    });
                }
            }
            if (!IsUser)
            {
                foreach (var book in listItems)
                {
                    printingEditionsModel.Items.Add(new PrintingEditionsModelItem()
                    {
                        Id = book.Id,
                        AuthorsNames = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(book),
                        Title = book.Name,
                        Price = book.Price,
                        Description = book.Description
                    });
                }
            }            

            return printingEditionsModel;
        }
        private PrintingEditionsModel CheckModel(PrintingEditionsModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionsModel();
            if (printingEditionsModelItem == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidModel);
                return responseModel;
            }            
            return responseModel;
        }
        public async Task<PrintingEditionsModel> AddNewPrintingEditionAsync(PrintingEditionsModelItem printingEditionsModelItem)
        {
            var responseModel = CheckModel(printingEditionsModelItem);
            if (string.IsNullOrWhiteSpace(printingEditionsModelItem.Title)
                || string.IsNullOrWhiteSpace(printingEditionsModelItem.Description)
                || printingEditionsModelItem.Currency == Enums.Currency.None
                || printingEditionsModelItem.Type == Enums.Type.None
                || !printingEditionsModelItem.AuthorsId.Any()
                || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            if (await _printingEditionRepository.IsExistedPrintingEdition(printingEditionsModelItem.Title))
            {
                responseModel.Errors.Add(Constants.Errors.IsExistedPrintingEdition);
                return responseModel;
            }
            var printingEdition = new PrintingEdition()
            {
                CreationDate = DateTime.Now,
                Name = printingEditionsModelItem.Title,
                Description = printingEditionsModelItem.Description,
                Type = printingEditionsModelItem.Type,
                Price = printingEditionsModelItem.Price,
                Currency = printingEditionsModelItem.Currency
            };                 
            if(await _authorInPrintingEditionRepository.AddToPrintingEditionAuthorsAsync(printingEdition, printingEditionsModelItem.AuthorsId))
            {
                await _printingEditionRepository.AddAsync(printingEdition);
                await _printingEditionRepository.SaveAsync();
            }
            return responseModel;
        }
        public async Task<PrintingEditionsModel> GetUsersPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, UserFilterModel filterModel)
        {
            var _pageSize = (int)Enums.PageSizes.Twelve;
            //var _pageSize = 3;

            IEnumerable<PrintingEdition> printingEditions = null;
            if (filterModel == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidModel);
                return printingEditionsModel;
            }                       
            if (string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                printingEditions = await _printingEditionRepository.GetAllAsync();
            }
            if (!string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                printingEditions = await _printingEditionRepository.GetWhereAsync(x => x.Name.Contains(filterModel.SearchByWord));
            }
            if (filterModel.Types == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }

            printingEditions = _converterHelper.GetFilteringListAsync(printingEditions, filterModel);


            printingEditions = printingEditions.Where(x => filterModel.Types.Contains(x.Type))
                    .Where(x => x.Price >= filterModel.RangePrice[Enums.RangePrice.MinValue]
                                              && x.Price <= filterModel.RangePrice[Enums.RangePrice.MaxValue]);

            var filteringListBooks = SortingList(filterModel, printingEditions);
            var printingEditionsOnPage = filteringListBooks.Skip((filterModel.Page - 1) * _pageSize).Take(_pageSize).ToList();
            return await AddItemsToModel(printingEditionsModel, printingEditionsOnPage, filterModel is UserFilterModel);
        }
        public async Task<PrintingEditionsModel> GetAdminPrintingEditionsListAsync(PrintingEditionsModel printingEditionsModel, AdminFilterModel filterModel)
        {
            var _pageSize = (int)Enums.PageSizes.Twelve;
            if (filterModel.Types == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }
            var printingEditions = await _printingEditionRepository.GetAllAsync();            
            printingEditions = printingEditions.Where(x => filterModel.Types.Contains(x.Type));
            var filteringListBooks = SortingList(filterModel, printingEditions);
            var items = filteringListBooks.Skip((filterModel.Page - 1) * _pageSize).Take(_pageSize).ToList();
            return await AddItemsToModel(printingEditionsModel, items, filterModel is UserFilterModel);
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
        public async Task<PrintingEditionsModel> DeletePrintingEditionAsync(PrintingEditionsModel printingEditionsModel, int printingEditionId)
        {
            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionId);

            if (printingEdition == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }
            await _printingEditionRepository.DeleteAsync(printingEdition);
            return printingEditionsModel;
        }

        public async Task<PrintingEditionsModel> EditPrintingEditionAsync(PrintingEditionsModelItem printingEditionsModelItem)
        {
            var responseModel = CheckModel(printingEditionsModelItem);
            if (string.IsNullOrWhiteSpace(printingEditionsModelItem.Description)
                || string.IsNullOrWhiteSpace(printingEditionsModelItem.Title)
                || !printingEditionsModelItem.AuthorsId.Any()
                || printingEditionsModelItem.Currency == Enums.Currency.None
                || printingEditionsModelItem.Type == Enums.Type.None
                || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionsModelItem.Id);

            printingEdition.Name = printingEditionsModelItem.Title;
            printingEdition.Description = printingEditionsModelItem.Description;
            printingEdition.Type = printingEditionsModelItem.Type;
            printingEdition.Currency = printingEditionsModelItem.Currency;
            printingEdition.Price = printingEditionsModelItem.Price;

            await _authorInPrintingEditionRepository.EditPrintingEditionAuthorsAsync(printingEdition, printingEditionsModelItem.AuthorsId);

            await _printingEditionRepository.EditAsync(printingEdition);

            return responseModel;
        }
    }
}
