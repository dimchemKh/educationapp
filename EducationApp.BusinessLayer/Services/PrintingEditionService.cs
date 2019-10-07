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

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, 
                                IConverterHelper converterHelper)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _converterHelper = converterHelper;
        }
        private async Task<PrintingEditionModel> AddItemsToModel(PrintingEditionModel printingEditionsModel, IEnumerable<PrintingEdition> listItems, bool IsAdmin = false)
        {
            if (!IsAdmin)
            {
                foreach (var book in listItems)
                {
                    printingEditionsModel.Items.Add(new PrintingEditionModelItem()
                    {
                        Id = book.Id,
                        AuthorsNames = await _printingEditionRepository.GetAuthorsInPrintingEditionAsync(book),
                        Title = book.Name,
                        Currency = book.Currency,
                        Price = book.Price
                    });
                }
            }
            if (IsAdmin)
            {
                foreach (var book in listItems)
                {
                    printingEditionsModel.Items.Add(new PrintingEditionModelItem()
                    {
                        Id = book.Id,
                        AuthorsNames = await _printingEditionRepository.GetAuthorsInPrintingEditionAsync(book),
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
                || printingEditionsModelItem.Type == Enums.PrintingEditionType.None
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
                await _printingEditionRepository.CreateAsync(printingEdition);
                await _printingEditionRepository.SaveAsync();
            }
            return responseModel;
        }
        public async Task<PrintingEditionModel> GetUsersPrintingEditionsListAsync(PrintingEditionModel printingEditionsModel, FilterPrintingditionModel filterModel)
        {
            IQueryable<PrintingEdition> printingEditions = null;
            if (filterModel == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidModel);
                return printingEditionsModel;
            }            
            printingEditions = _printingEditionRepository.FiteringFromSearchWord(filterModel.SearchByBody, printingEditions);
            if (filterModel.PrintingEditionTypes == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }                        
            printingEditions = printingEditions.Select(x => new PrintingEdition()
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Description = x.Description,
                AuthorInPrintingEditions = x.AuthorInPrintingEditions,
                Currency = x.Currency,
                Price = _converterHelper.Converting(x.Currency, filterModel.Currency, x.Price)
            });

            printingEditions = _printingEditionRepository.FilteringByTypes(filterModel.PrintingEditionTypes, printingEditions);
            printingEditions = _printingEditionRepository.FilteringByPrice(filterModel.RangePrice, printingEditions);
            printingEditions = _printingEditionRepository.FilteringByProperty(filterModel.SortType, filterModel.SortState, printingEditions);
            var printingEditionsPage = _printingEditionRepository.FilteringPage(filterModel.Page, (int)filterModel.PageSize, printingEditions);
            return await AddItemsToModel(printingEditionsModel, printingEditionsPage);
        }
        public async Task<PrintingEditionModel> GetAdminPrintingEditionsListAsync(PrintingEditionModel printingEditionsModel, FilterPrintingditionModel filterModel, bool IsAdmin)
        {
            IQueryable<PrintingEdition> printingEditions = null;
            if (filterModel.PrintingEditionTypes == null)
            {
                printingEditionsModel.Errors.Add(Constants.Errors.InvalidData);
                return printingEditionsModel;
            }
            printingEditions = _printingEditionRepository.ReadAll();
            printingEditions = _printingEditionRepository.FilteringByTypes(filterModel.PrintingEditionTypes, printingEditions);
            printingEditions = _printingEditionRepository.FilteringByProperty(filterModel.SortType, filterModel.SortState, printingEditions);
            var printingEditionsPage = _printingEditionRepository.FilteringPage(filterModel.Page, (int)filterModel.PageSize, printingEditions);
            return await AddItemsToModel(printingEditionsModel, printingEditionsPage, IsAdmin);
        }

        public async Task<PrintingEditionModel> GetUserPrintingEditionPageAsync(PrintingEditionModel printingEditionsModel, FilterPrintingEditionDetailsModel pageFilterModel)
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
                AuthorsNames = await _printingEditionRepository.GetAuthorsInPrintingEditionAsync(printingEdition),
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
                || printingEditionsModelItem.Type == Enums.PrintingEditionType.None
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
