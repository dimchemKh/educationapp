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
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using DataModel = EducationApp.DataAccessLayer.Models;

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IMapperHelper _mapperHelper;

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, 
                                IConverterHelper converterHelper, IMapperHelper mapperHelper)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _converterHelper = converterHelper;
            _mapperHelper = mapperHelper;
        }
        
        private PrintingEditionModel CheckModel(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionModel();

            if (string.IsNullOrWhiteSpace(printingEditionsModelItem.Title) || string.IsNullOrWhiteSpace(printingEditionsModelItem.Description)
                || printingEditionsModelItem.Currency == Enums.Currency.None || printingEditionsModelItem.PrintingEditionType == Enums.PrintingEditionType.None
                || printingEditionsModelItem.AuthorsId.Any() || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            return responseModel;
        }
        public async Task<PrintingEditionModel> CreatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = CheckModel(printingEditionsModelItem);

            var entity = new PrintingEdition();

            entity = _mapperHelper.MapToEntity(printingEditionsModelItem, entity);

            var boolq = await _printingEditionRepository.IsExistedPrintingEdition(entity);


            var printingEdition = new PrintingEdition()
            {
                CreationDate = DateTime.Now,
                Title = printingEditionsModelItem.Title,
                Description = printingEditionsModelItem.Description,
                PrintingEditionType = printingEditionsModelItem.PrintingEditionType,
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
        public async Task<PrintingEditionModel> GetPrintingEditionsAsync(FilterPrintingEditionModel filter)
        {
            var responseModel = new PrintingEditionModel();
            var modelItem = new PrintingEditionModelItem();
            var repositoryFilter = new DataFilter.FilterPrintingEditionModel();

            await _authorInPrintingEditionRepository.GetAuthorsInPEAsync();
            repositoryFilter = _mapperHelper.MapToModelItem(filter, repositoryFilter);

            var list = await _printingEditionRepository.Filtering(repositoryFilter);

            foreach (var printingEdition in list)
            {
                modelItem = _mapperHelper.MapToModelItem(printingEdition, modelItem);
                responseModel.Items.Add(modelItem);
            }

            return responseModel;
        }

        public async Task<PrintingEditionModel> GetPrintingEditionDetailsAsync(FilterPrintingEditionDetailsModel filter)
        {
            var responseModel = new PrintingEditionModel();
                       
            var printingEdition = await _printingEditionRepository.GetByIdAsync(filter.Id);

            if(printingEdition == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            responseModel.Items.Add(new PrintingEditionModelItem()
            {
                Id = printingEdition.Id,
                Title = printingEdition.Title,
                AuthorsNames = null, //await _printingEditionRepository.GetAuthorsInPrintingEditionAsync(printingEdition),
                Description = printingEdition.Description,
                Currency = printingEdition.Currency,
                Price = _converterHelper.Converting(printingEdition.Currency, filter.Currency, printingEdition.Price)
            });
            return responseModel;
        }
        public async Task<PrintingEditionModel> DeletePrintingEditionAsync(int printingEditionId)
        {
            var responseModel = new PrintingEditionModel();
            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionId);

            if (printingEdition == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidDataFromClient); 
                return responseModel;
            }
            await _printingEditionRepository.DeleteAsync(printingEdition);
            return responseModel;
        }
        public async Task<PrintingEditionModel> EditPrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = CheckModel(printingEditionsModelItem);
            if (string.IsNullOrWhiteSpace(printingEditionsModelItem.Description)
                || string.IsNullOrWhiteSpace(printingEditionsModelItem.Title)
                || !printingEditionsModelItem.AuthorsId.Any()
                || printingEditionsModelItem.Currency == Enums.Currency.None
                || printingEditionsModelItem.PrintingEditionType == Enums.PrintingEditionType.None
                || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionsModelItem.Id);

            printingEdition.Title = printingEditionsModelItem.Title;
            printingEdition.Description = printingEditionsModelItem.Description;
            printingEdition.PrintingEditionType = printingEditionsModelItem.PrintingEditionType;
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
