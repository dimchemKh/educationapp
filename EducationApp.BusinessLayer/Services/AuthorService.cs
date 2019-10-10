using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;

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
        public async Task<AuthorModel> GetAuthorsListAsync(FilterAuthorModel filterModel)
        {
            var responseModel = new AuthorModel();
            var repositoryModel = new DataFilter.FilterAuthorModel();
            var itemModel = new AuthorModelItem();
            if (filterModel == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidDataFromClient);
                return responseModel;
            }

            repositoryModel = _mapperHelper.MapToModelItem(filterModel, repositoryModel);

            var listAuthors = await _authorRepository.FilteringAsync(repositoryModel);

            foreach (var author in listAuthors)
            {
                itemModel = _mapperHelper.MapToModelItem(author, itemModel);
                responseModel.Items.Add(itemModel);
            }
           
            return responseModel;
        }
        public async Task<AuthorModel> CreateAuthorAsync(AuthorModelItem authorModelItem)
        {
            var responseModel = new AuthorModel();

            if (authorModelItem == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidDataFromClient);
                return responseModel;
            }
            if (string.IsNullOrWhiteSpace(authorModelItem.Name))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var author = new Author()
            {
                Name = authorModelItem.Name
            };

            await _authorRepository.CreateAsync(author);
            await _authorRepository.SaveAsync();
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
        public async Task<AuthorModel> EditAuthorAsync(AuthorModelItem authorModelItem)
        {
            var responseModel = new AuthorModel();
            if(authorModelItem == null)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidDataFromClient);
                return responseModel;
            }
            if(authorModelItem.Name == null)
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
