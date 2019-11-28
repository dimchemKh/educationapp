using EducationApp.BusinessLogic.Models.Authors;
using EducationApp.BusinessLogic.Services.Interfaces;
using EducationApp.BusinessLogic.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Threading.Tasks;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.BusinessLogic.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLogic.Models.Filters;

namespace EducationApp.BusinessLogic.Services
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
        public async Task<AuthorModel> GetFilteredAuthorsAsync(FilterAuthorModel filterModel)
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
            var authors = await _authorRepository.GetFilteredAuthorsAsync(repositoryModel);

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
            var createResult = await _authorRepository.CreateAsync(author);
            if (createResult.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedCreate);
            }
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

            var deleteResult = await _authorRepository.DeleteAsync(author);

            if(deleteResult.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedDelete);
            }
            var deleteAuthorsProduct = await _authorInPrintingEditionRepository.DeleteAuthorsById(authorId);

            if (!deleteAuthorsProduct)
            {
                responseModel.Errors.Add(Constants.Errors.FailedDelete);
            }
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

            var updateResult = await _authorRepository.UpdateAsync(author);

            if (updateResult.Equals(0))
            {
                responseModel.Errors.Add(Constants.Errors.FailedUpdate);
            }

            return responseModel;
        }
    }
}
