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

namespace EducationApp.BusinessLayer.Services
{
    public class AuthorService : IAuthorService 
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IAuthorInPrintingEditionRepository _authorInPrintingEditionRepository;
        //private readonly IPaginationHelper<Author> _paginationHelper;
        public AuthorService(IAuthorRepository authorRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository)
        {
            _authorRepository = authorRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
        }
        public async Task<AuthorModel> GetAuthorsListAsync(AuthorModel authorModel, FilterAuthorModel filterModel)
        {
            if(filterModel == null)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidModel);
                return authorModel;
            }
            if(filterModel.SortState == Enums.SortState.None)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidData);
                return authorModel;
            }

            var listAuthors = _authorRepository.ReadAll();
            var newlistAuthors = _authorRepository.FilteringPage(filterModel.Page, (int)filterModel.PageSize, listAuthors);

            foreach (var item in newlistAuthors)
            {
                //authorModel.Items.
            }
           
            return authorModel;
        }
        public async Task<AuthorModel> AddNewAuthorAsync(AuthorModelItem authorModelItem)
        {
            var authorModel = new AuthorModel();
            if(authorModelItem == null)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidModel);
                return authorModel;
            }
            if (string.IsNullOrWhiteSpace(authorModelItem.Name))
            {
                authorModel.Errors.Add(Constants.Errors.InvalidData);
                return authorModel;
            }
            var author = new Author()
            {
                Name = authorModelItem.Name
            };
            await _authorRepository.CreateAsync(author);
            await _authorRepository.SaveAsync();
            return authorModel;
        }
        public async Task<AuthorModel> DeleteAuthorAsync(AuthorModel authorModel, int authorId)
        {
            var author = await _authorRepository.GetByIdAsync(authorId);
            if(author == null)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidData);
                return authorModel;
            }
            await _authorRepository.DeleteAsync(author);
            return authorModel;
        }
        public async Task<AuthorModel> EditAuthorAsync(AuthorModelItem authorModelItem)
        {
            var authorModel = new AuthorModel();
            if(authorModelItem == null)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidModel);
                return authorModel;
            }
            if(authorModelItem.Name == null)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidData);
                return authorModel;
            }
            var author = await _authorRepository.GetByIdAsync(authorModelItem.Id);
            if(author == null)
            {
                authorModel.Errors.Add(Constants.Errors.ReturnNull);
                return authorModel;
            }
            await _authorRepository.EditAsync(author);
            return authorModel;
        }
    }
}
