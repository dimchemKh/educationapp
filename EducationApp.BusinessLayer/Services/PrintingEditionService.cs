using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Common.Constants;
using System;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using DataModel = EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;

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
                || !printingEditionsModelItem.AuthorsId.Any() || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            return responseModel;
        }
        public async Task<PrintingEditionModel> CreatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = CheckModel(printingEditionsModelItem);

            var entity = new DataModel.DAPrintingEditionModel();

            entity = _mapperHelper.MapToEntity(printingEditionsModelItem, entity);

            var isExisted = await _printingEditionRepository.IsExistedPrintingEdition(entity);

            if (isExisted)
            {
                responseModel.Errors.Add(Constants.Errors.IsExistedPrintingEdition);
                return responseModel;
            }
            var printingEdition = new PrintingEdition();

            printingEdition = _mapperHelper.MapToEntity(printingEditionsModelItem, printingEdition);
            printingEdition.CreationDate = DateTime.Now;
            
            await _authorInPrintingEditionRepository.AddToPrintingEditionAuthorsAsync(printingEdition, printingEditionsModelItem.AuthorsId);

            await _printingEditionRepository.CreateAsync(printingEdition);
            await _printingEditionRepository.SaveAsync();
            
            return responseModel;
        }
        public async Task<PrintingEditionModel> GetPrintingEditionsAsync(FilterPrintingEditionModel filter, bool isAdmin = false)
        {
            //PrintingEditionModelItem modelItem = null;
            //if (!isAdmin)
            //{
            //    modelItem = new PrintingEditionModelItem();
            //}
            //if (isAdmin)
            //{
            //    modelItem = new PrintingEditionForAdminModel();
            //}
            var modelItem = new PrintingEditionModelItem();
            var responseModel = new PrintingEditionModel();
            var repositoryFilter = new DataFilter.FilterPrintingEditionModel();

            repositoryFilter = _mapperHelper.MapToModelItem(filter, repositoryFilter);

            var printingEditionsList = await _printingEditionRepository.FilteringAsync(repositoryFilter);    

            foreach (var printingEdition in printingEditionsList)
            {
                modelItem = _mapperHelper.MapToModelItem(printingEdition, modelItem); 
                responseModel.Items.Add(modelItem);
            }            
            return responseModel;
        }

        public async Task<PrintingEditionModel> GetPrintingEditionDetailsAsync(FilterPrintingEditionDetailsModel filter)
        {
            var responseModel = new PrintingEditionModel();
            var itemModel = new PrintingEditionModelItem();
            var printingEdition = await _printingEditionRepository.GetByIdAsync(filter.Id);

            if(printingEdition == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidDataFromClient);
                return responseModel;
            }

            itemModel = _mapperHelper.MapToModelItem(printingEdition, itemModel);
            
            var result = await _authorInPrintingEditionRepository.GetAuthorsInOnePEAsync(printingEdition.Id);

            itemModel.AuthorNames.AddRange(result);
            responseModel.Items.Add(itemModel);

            return responseModel;
        }
        public async Task<PrintingEditionModel> DeletePrintingEditionAsync(long printingEditionId)
        {
            var responseModel = new PrintingEditionModel();
            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionId);

            if (printingEdition == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidDataFromClient); 
                return responseModel;
            }
            await _authorInPrintingEditionRepository.DeleteWhereAsync(x => x.PrintingEditionId == printingEditionId);
            await _printingEditionRepository.DeleteAsync(printingEdition);

            return responseModel;
        }
        public async Task<PrintingEditionModel> EditPrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = CheckModel(printingEditionsModelItem);

            var printingEdition = await _printingEditionRepository.GetByIdAsync(printingEditionsModelItem.Id);

            printingEdition = _mapperHelper.MapToEntity(printingEditionsModelItem, printingEdition);

            await _authorInPrintingEditionRepository.EditPrintingEditionAuthorsAsync(printingEdition, printingEditionsModelItem.AuthorsId);

            await _printingEditionRepository.EditAsync(printingEdition);
            
            return responseModel;
        }
    }
}
