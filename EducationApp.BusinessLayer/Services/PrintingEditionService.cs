using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.BusinessLayer.Common.Constants;
using System;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using DataModel = EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using System.Collections.Generic;
using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.BusinessLayer.Helpers.Mappers;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        private readonly IMapperHelper _mapperHelper;
        private readonly ICurrencyConverterHelper _currencyConverterHelper;
        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, 
                               IMapperHelper mapperHelper, ICurrencyConverterHelper currencyConverterHelper)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;            
            _mapperHelper = mapperHelper;
            _currencyConverterHelper = currencyConverterHelper;
        }        
        private PrintingEditionModel ValidateData(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionModel();

            if (string.IsNullOrWhiteSpace(printingEditionsModelItem.Title) || string.IsNullOrWhiteSpace(printingEditionsModelItem.Description)
                || !printingEditionsModelItem.Authors.Any() || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
            }
            return responseModel;
        }
        public async Task<PrintingEditionModel> CreatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = ValidateData(printingEditionsModelItem);
            if (responseModel.Errors.Any())
            {
                return responseModel;
            }
            var printingEdition = _mapperHelper.Map<PrintingEditionModelItem, PrintingEdition>(printingEditionsModelItem);

            printingEdition.Price = _currencyConverterHelper.Converting(printingEditionsModelItem.Currency, Enums.Currency.USD, printingEditionsModelItem.Price);
            printingEdition.Currency = Enums.Currency.USD;

            var authorsId = printingEditionsModelItem.Authors.Select(x => x.Id).ToList();
            if (authorsId == null || !authorsId.Any())
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            await _printingEditionRepository.CreateAsync(printingEdition);

            await _authorInPrintingEditionRepository.AddAuthorsInPrintingEditionAsync(printingEdition.Id, authorsId);

            return responseModel;
        }
        public async Task<PrintingEditionModel> GetPrintingEditionsAsync(FilterPrintingEditionModel filter, bool isAdmin)
        {
            var responseModel = new PrintingEditionModel();

            var repositoryFilter = _mapperHelper.Map<FilterPrintingEditionModel, DataFilter.FilterPrintingEditionModel>(filter);
            if(repositoryFilter == null)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return responseModel;
            }
            if (!isAdmin)
            {
                repositoryFilter.PriceMinValue = _currencyConverterHelper.Converting(repositoryFilter.Currency, Enums.Currency.USD, repositoryFilter.PriceMinValue);
                repositoryFilter.PriceMaxValue = _currencyConverterHelper.Converting(repositoryFilter.Currency, Enums.Currency.USD, repositoryFilter.PriceMaxValue);
            }
            var printingEditionsModel = await _authorInPrintingEditionRepository.GetPrintingEditionFilteredDataAsync(repositoryFilter, isAdmin);

            foreach (var printingEdition in printingEditionsModel.Collection)
            {
                var modelItem = printingEdition.MapToModel(filter.Currency);
                if (!isAdmin)
                {
                    modelItem.Price = _currencyConverterHelper.Converting(Enums.Currency.USD, filter.Currency, modelItem.Price);
                }
                responseModel.Items.Add(modelItem);
            }
            responseModel.ItemsCount = printingEditionsModel.CollectionCount;           

            return responseModel;
        }
        public async Task<PrintingEditionModel> DeletePrintingEditionAsync(long printingEditionId)
        {
            var responseModel = new PrintingEditionModel();
            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionId);

            if (printingEdition == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData); 
                return responseModel;
            }
            //await _printingEditionRepository.DeleteAsync(printingEdition);
            await _authorInPrintingEditionRepository.DeleteByIdAsync(x => x.PrintingEditionId == printingEditionId);

            return responseModel;
        }
        public async Task<PrintingEditionModel> UpdatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = ValidateData(printingEditionsModelItem);

            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionsModelItem.Id);
            printingEdition = printingEditionsModelItem.MapToEntity(printingEdition);

            var authorsId = printingEditionsModelItem.Authors.Select(x => x.Id).ToList();
            if (authorsId == null || !authorsId.Any())
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            printingEdition.Price = _currencyConverterHelper.Converting(printingEditionsModelItem.Currency, Enums.Currency.USD, printingEditionsModelItem.Price);
            await _printingEditionRepository.UpdateAsync(printingEdition);
            var updateResult = await _authorInPrintingEditionRepository.UpdateAuthorsInPrintingEditionAsync(printingEdition, authorsId);

            return responseModel;            
        }
    }
}
