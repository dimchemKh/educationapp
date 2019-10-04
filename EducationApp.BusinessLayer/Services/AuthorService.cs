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
        private readonly ISorterHelper<Author> _sorterHelper;
        private readonly IPaginationHelper<Author> _paginationHelper;
        public AuthorService(IAuthorRepository authorRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository, 
            ISorterHelper<Author> sorterHelper, IPaginationHelper<Author> paginationHelper)
        {
            _authorRepository = authorRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
            _sorterHelper = sorterHelper;
            _paginationHelper = paginationHelper;
        }
        public async Task<AuthorModel> GetAuthorsListAsync(AuthorModel authorModel, AuthorFilterModel filterModel)
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

            var listAuthors = _authorRepository.GetAllAsync();

            var filteringAuthorsList = _sorterHelper.Sorting(filterModel.SortType, listAuthors);

            var authorsList = _paginationHelper.Pagination(filteringAuthorsList, filterModel);    
            
            foreach (var author in authorsList)
            {
                authorModel.Items.Add(new AuthorModelItem()
                {
                    Id = author.Id,
                    Name = author.Name,
                    ProductTitles = await _authorInPrintingEditionRepository.GetPrintingEditionAuthorsListAsync(author)
                });
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
            await _authorRepository.AddAsync(author);
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
