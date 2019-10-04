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
using EducationApp.BusinessLayer.Models.Filters;

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly ISorterHelper<PrintingEdition> _sorterHelper;
        private readonly IPaginationHelper<PrintingEdition> _paginationHelper;

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, 
                                IConverterHelper converterHelper, ISorterHelper<PrintingEdition> sorterHelper, IPaginationHelper<PrintingEdition> paginationHelper)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _converterHelper = converterHelper;
            _sorterHelper = sorterHelper;
            _paginationHelper = paginationHelper;
        }
        private async Task<PrintingEditionModel> AddItemsToModel(PrintingEditionModel printingEditionsModel, IEnumerable<PrintingEdition> listItems, bool IsUser)
        {
            if (IsUser)
            {
                foreach (var book in listItems)
                {
                    printingEditionsModel.Items.Add(new PrintingEditionModelItem()
                    {
                        Id = book.Id,
                        AuthorsNames = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(book),
                        Title = book.Name,
                        Currency = book.Currency,
                        Price = book.Price
                    });
                }
            }
            if (!IsUser)
            {
                foreach (var book in listItems)
                {
                    printingEditionsModel.Items.Add(new PrintingEditionModelItem()
                    {
                        Id = book.Id,
                        AuthorsNames = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(book),
                        Title = book.Name,
                        Currency = book.Currency,
                        Price = book.Price,
                        Description = book.Description
                    });
                }
            }            

            return printingEditionsModel;
        }
        private PrintingEditionModel CheckModel(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionModel();
            if (printingEditionsModelItem == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidModel);
                return responseModel;
            }            
            return responseModel;
        }
        public async Task<PrintingEditionModel> AddNewPrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
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
        public async Task<PrintingEditionModel> GetUsersPrintingEditionsListAsync(PrintingEditionModel printingEditionsModel, UserFilterModel filterModel)
        {
            IQueryable<PrintingEdition> printingEditions = null;
            if (filterModel == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidModel);
                return printingEditionsModel;
            }                       
            if (string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                printingEditions = _printingEditionRepository.GetAllAsync();
            }
            if (!string.IsNullOrWhiteSpace(filterModel.SearchByWord))
            {
                printingEditions = _printingEditionRepository.GetWhereAsync(x => x.Name.Contains(filterModel.SearchByWord));
            }
            if (filterModel.Types == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }
            printingEditions = printingEditions.Where(x => filterModel.Types.Contains(x.Type));
            printingEditions = printingEditions.Select(x => new PrintingEdition()
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Description = x.Description,
                AuthorInPrintingEdition = x.AuthorInPrintingEdition,
                Currency = x.Currency,
                Price = _converterHelper.Converting(x.Currency, filterModel.Currency, x.Price)
            });                          
            printingEditions = printingEditions.Where(x => x.Price >= filterModel.RangePrice[Enums.RangePrice.MinValue]
                                                 && x.Price <= filterModel.RangePrice[Enums.RangePrice.MaxValue]);

            var filteringListBooks = _sorterHelper.Sorting(filterModel.SortType, printingEditions);           

            if (filteringListBooks == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.ReturnNull);
                return printingEditionsModel;
            }
            var printingEditionsOnPage = _paginationHelper.Pagination(filteringListBooks, filterModel);

            return await AddItemsToModel(printingEditionsModel, printingEditionsOnPage, filterModel is UserFilterModel);
        }
        public async Task<PrintingEditionModel> GetAdminPrintingEditionsListAsync(PrintingEditionModel printingEditionsModel, AdminFilterModel filterModel)
        {
            if (filterModel.Types == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }
            var printingEditionsList = _printingEditionRepository.GetAllAsync();
            printingEditionsList = printingEditionsList.Where(x => filterModel.Types.Contains(x.Type));
            var filteringListBooks = _sorterHelper.Sorting(filterModel.SortType, printingEditionsList);
            var printingEditions = _paginationHelper.Pagination(filteringListBooks, filterModel);
            return await AddItemsToModel(printingEditionsModel, printingEditions, filterModel is UserFilterModel);
        }

        public async Task<PrintingEditionModel> GetUserPrintingEditionPageAsync(PrintingEditionModel printingEditionsModel, PageFilterModel pageFilterModel)
        {            
            if(pageFilterModel == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidModel);
                return printingEditionsModel;
            }
            var printingEdition = await _printingEditionRepository.GetByIdAsync(pageFilterModel.Id);
            if(printingEdition == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }
            printingEditionsModel.Items.Add(new PrintingEditionModelItem()
            {
                Id = printingEdition.Id,
                Title = printingEdition.Name,
                AuthorsNames = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(printingEdition),
                Description = printingEdition.Description,
                Currency = printingEdition.Currency,
                Price = _converterHelper.Converting(printingEdition.Currency, pageFilterModel.Currency, printingEdition.Price)
            });
            return printingEditionsModel;
        }
        public async Task<PrintingEditionModel> DeletePrintingEditionAsync(PrintingEditionModel printingEditionsModel, int printingEditionId)
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
        public async Task<PrintingEditionModel> EditPrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
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
            if(await _authorInPrintingEditionRepository.EditPrintingEditionAuthorsAsync(printingEdition, printingEditionsModelItem.AuthorsId))
            {
                await _printingEditionRepository.EditAsync(printingEdition);
            }

            return responseModel;
        }
    }
}
