using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using System.Threading.Tasks;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;

namespace EducationApp.BusinessLayer.Services
{
    public class AuthorService : IAuthorService 
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        private readonly IMapperHelper _mapperHelper;

        public AuthorService(IAuthorRepository authorRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, IMapperHelper mapperHelper)
        {
            _authorRepository = authorRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _mapperHelper = mapperHelper;
        }
        public async Task<AuthorModel> GetAllAuthorsAsync(FilterAuthorModel filterModel)
        {
            var responseModel = new AuthorModel();
            if (filterModel == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var repositoryModel = _mapperHelper.Map<FilterAuthorModel, DataFilter.BaseFilterModel>(filterModel);
            if (repositoryModel == null)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return responseModel;
            }
            var authors = await _authorRepository.GetAllAuthorsAsync(repositoryModel);
            foreach (var author in authors.Collection)
            {
                var itemModel = _mapperHelper.Map<AuthorDataModel, AuthorModelItem>(author);
                if (itemModel == null)
                {
                    responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                    continue;
                }
                responseModel.Items.Add(itemModel);
            }
            responseModel.ItemsCount = authors.CollectionCount;

            return responseModel;
        }
        public async Task<AuthorModel> GetAuthorsInPrintingEditionsAsync(FilterAuthorModel filterModel)
        {
            var responseModel = new AuthorModel();
            if (filterModel == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var repositoryModel = _mapperHelper.Map<FilterAuthorModel, DataFilter.BaseFilterModel>(filterModel);
            if(repositoryModel == null)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return responseModel;
            }
            var authors = await _authorInPrintingEditionRepository.GetAuthorsFilteredDataAsync(repositoryModel);

            foreach (var author in authors.Collection)
            {
                var itemModel = _mapperHelper.Map<AuthorDataModel, AuthorModelItem>(author);
                if (itemModel == null)
                {
                    responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                    continue;
                }
                responseModel.Items.Add(itemModel);
            }
            responseModel.ItemsCount = authors.CollectionCount;
            return responseModel;
        }
        public async Task<AuthorModel> CreateAuthorAsync(AuthorModelItem authorModelItem)
        {
            var responseModel = new AuthorModel();

            if (authorModelItem == null || string.IsNullOrWhiteSpace(authorModelItem.Name))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var author = new Author()
            {
                Name = authorModelItem.Name
            };
            await _authorRepository.CreateAsync(author);
            return responseModel;
        }
        public async Task<AuthorModel> DeleteAuthorAsync(long authorId)
        {
            var responseModel = new AuthorModel();
            var author = await _authorRepository.GetByIdAsync(authorId);
            if(author == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            //await _authorRepository.DeleteAsync(author);
            await _authorInPrintingEditionRepository.DeleteByIdAsync(x => x.AuthorId == authorId);
            return responseModel;
        }

        public async Task<AuthorModel> UpdateAuthorAsync(AuthorModelItem authorModelItem)
        {
            var responseModel = new AuthorModel();
            if(authorModelItem == null || string.IsNullOrWhiteSpace(authorModelItem.Name))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var author = await _authorRepository.GetByIdAsync(authorModelItem.Id);
            if(author == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            author.Name = authorModelItem.Name;

            await _authorRepository.UpdateAsync(author);
            return responseModel;
        }
    }
}
