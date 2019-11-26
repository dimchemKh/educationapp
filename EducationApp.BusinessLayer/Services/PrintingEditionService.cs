using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
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

            printingEdition.Price = _currencyConverterHelper.Convert(printingEditionsModelItem.Currency, Enums.Currency.USD, printingEditionsModelItem.Price);

            printingEdition.Currency = Enums.Currency.USD;

            var authorsId = printingEditionsModelItem.Authors.Select(x => x.Id).ToArray();

            if (authorsId == null || !authorsId.Any())
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            var createResult = await _printingEditionRepository.CreateAsync(printingEdition);

            if (createResult.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedCreate);
                return responseModel;
            }

            var createAuthorsInPrintingEditionResult = await _authorInPrintingEditionRepository.CreateAuthorsInPrintingEditionAsync(printingEdition.Id, authorsId);

            if (createAuthorsInPrintingEditionResult.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedCreate);
                return responseModel;
            }

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
                repositoryFilter.PriceMinValue = _currencyConverterHelper.Convert(repositoryFilter.Currency, Enums.Currency.USD, repositoryFilter.PriceMinValue);
                repositoryFilter.PriceMaxValue = _currencyConverterHelper.Convert(repositoryFilter.Currency, Enums.Currency.USD, repositoryFilter.PriceMaxValue);
            }

            var printingEditionsModel = await _printingEditionRepository.GetPrintingEditionFilteredDataAsync(repositoryFilter, isAdmin);

            foreach (var printingEdition in printingEditionsModel.Collection)
            {
                var modelItem = printingEdition.MapToModel(filter.Currency);
                if (!isAdmin)
                {
                    modelItem.Price = _currencyConverterHelper.Convert(Enums.Currency.USD, filter.Currency, modelItem.Price);
                }
                responseModel.Items.Add(modelItem);
            }

            responseModel.ItemsCount = printingEditionsModel.CollectionCount;

            return responseModel;
        }
        public async Task<PrintingEditionModel> GetPrintingEditionDetailsAsync(long printingEditionId, Enums.Currency currency)
        {
            var responseModel = new PrintingEditionModel();

            var printingEditionData = await _printingEditionRepository.GetPrintingEditionDetailsAsync(printingEditionId);

            var printingEdition = printingEditionData.MapToModel(currency);

            printingEdition.Price = _currencyConverterHelper.Convert(Enums.Currency.USD, currency, printingEdition.Price);

            responseModel.Items.Add(printingEdition);

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

            var deleteResult = await _printingEditionRepository.DeleteAsync(printingEdition);

            if (deleteResult.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedDelete);
            }

            var deleteAuthorsInPrintingEdition = await _authorInPrintingEditionRepository.DeletePrintingEditionsById(printingEditionId);

            if (deleteAuthorsInPrintingEdition.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedDelete);
            }

            return responseModel;
        }
        public async Task<PrintingEditionModel> UpdatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = ValidateData(printingEditionsModelItem);

            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionsModelItem.Id);

            printingEdition = printingEditionsModelItem.MapToEntity(printingEdition);

            var authorsId = printingEditionsModelItem.Authors.Select(x => x.Id).ToArray();

            if (authorsId == null || !authorsId.Any())
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            printingEdition.Price = _currencyConverterHelper.Convert(printingEditionsModelItem.Currency, Enums.Currency.USD, printingEditionsModelItem.Price);

            var updateResult = await _printingEditionRepository.UpdateAsync(printingEdition);

            if (updateResult.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedUpdate);
                return responseModel;
            }

            var updateAuthorsProduct = await _authorInPrintingEditionRepository.UpdateAuthorsInPrintingEditionAsync(printingEdition.Id, authorsId);

            if (!updateAuthorsProduct)
            {
                responseModel.Errors.Add(Constants.Errors.FailedUpdate);
                return responseModel;
            }

            return responseModel;            
        }
    }
}
