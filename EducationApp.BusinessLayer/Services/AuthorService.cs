using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using System.Threading.Tasks;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters.Base;

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
        public async Task<AuthorModel> GetAuthorsListAsync(BaseFilterModel filterModel)
        {
            var responseModel = new AuthorModel();
            if (filterModel == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            var repositoryModel = _mapperHelper.MapToModelItem<BaseFilterModel, DataFilter.BaseFilterModel>(filterModel);

            var listAuthors = await _authorRepository.FilteringAsync(repositoryModel);

            foreach (var author in listAuthors)
            {
                var itemModel = _mapperHelper.MapToModelItem<AuthorDataModel, AuthorModelItem>(author);
                responseModel.Items.Add(itemModel);
            }           
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
            //await _authorRepository.SaveAsync();
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
            await _authorInPrintingEditionRepository.DeleteWhereAsync(x => x.AuthorId == authorId);
            await _authorRepository.DeleteAsync(author);
            return responseModel;
        }
        // ???
        public async Task<AuthorModel> EditAuthorAsync(AuthorModelItem authorModelItem)
        {
            var responseModel = new AuthorModel();
            if(authorModelItem == null || authorModelItem.Name == null)
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
            await _authorRepository.EditAsync(author);
            return responseModel;
        }
    }
}
