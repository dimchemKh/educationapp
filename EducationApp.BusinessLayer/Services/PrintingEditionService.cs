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

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        private readonly IMapperHelper _mapperHelper;

        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, 
                               IMapperHelper mapperHelper)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;            
            _mapperHelper = mapperHelper;
        }
        
        private PrintingEditionModel CheckModel(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = new PrintingEditionModel();

            if (string.IsNullOrWhiteSpace(printingEditionsModelItem.Title) || string.IsNullOrWhiteSpace(printingEditionsModelItem.Description)
                || !printingEditionsModelItem.Authors.Any() || printingEditionsModelItem.Price == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            return responseModel;
        }
        public async Task<PrintingEditionModel> CreatePrintingEditionAsync(PrintingEditionModelItem printingEditionsModelItem)
        {
            var responseModel = CheckModel(printingEditionsModelItem);

            var entity = _mapperHelper.MapToModelItem<PrintingEditionModelItem, PrintingEditionDataModel>(printingEditionsModelItem);

            var isExisted = await _printingEditionRepository.IsExistedPrintingEdition(entity);

            if (isExisted)
            {
                responseModel.Errors.Add(Constants.Errors.IsExistedPrintingEdition);
                return responseModel;
            }


            var printingEdition = _mapperHelper.MapToModelItem<PrintingEditionModelItem, PrintingEdition>(printingEditionsModelItem);
            printingEdition.CreationDate = DateTime.Now;
            
            await _authorInPrintingEditionRepository.AddAuthorsInPrintingEditionAsync(printingEdition, printingEditionsModelItem.Authors);

            await _printingEditionRepository.CreateAsync(printingEdition);
            await _printingEditionRepository.SaveAsync();
            
            return responseModel;
        }
        public async Task<PrintingEditionModel> GetPrintingEditionsAsync(FilterPrintingEditionModel filter)
        {

            var responseModel = new PrintingEditionModel();

            var repositoryFilter = _mapperHelper.MapToModelItem<FilterPrintingEditionModel, DataFilter.FilterPrintingEditionModel>(filter);

            var printingEditionsList = await _printingEditionRepository.FilteringAsync(repositoryFilter);    

            foreach (var printingEdition in printingEditionsList)
            {
                var modelItem = _mapperHelper.MapToModelItem<PrintingEditionDataModel, PrintingEditionModelItem>(printingEdition); 
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
                responseModel.Errors.Add(Constants.Errors.InvalidDataFromClient);
                return responseModel;
            }

            var itemModel = _mapperHelper.MapToModelItem<PrintingEdition, PrintingEditionModelItem>(printingEdition);
            
            var result = await _authorInPrintingEditionRepository.GetAuthorsInOnePrintingEditionAsync(printingEdition.Id);

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

            await _authorInPrintingEditionRepository.EditAuthorsInPrintingEditionAsync(printingEdition, printingEditionsModelItem.AuthorsId);

            await _printingEditionRepository.EditAsync(printingEdition);
            
            return responseModel;
        }
    }
}
