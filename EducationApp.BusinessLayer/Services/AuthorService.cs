using EducationApp.BusinessLayer.Models.Authors;
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
        public AuthorService(IAuthorRepository authorRepository, IAuthorInPrintingEditionRepository authorInPrintingEditionRepository)
        {
            _authorRepository = authorRepository;
            _authorInPrintingEditionRepository = authorInPrintingEditionRepository;
        }
        public async Task<AuthorModel> GetAuthorsListAsync(AuthorModel authorModel, AuthorFilterModel authorFilterModel)
        {
            if(authorFilterModel == null)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidModel);
                return authorModel;
            }
            if(authorFilterModel.StateSort == Enums.StateSort.None)
            {
                authorModel.Errors.Add(Constants.Errors.InvalidData);
                return authorModel;
            }
            var _pageSize = (int)Enums.PageSizes.Six;
            var listAuthors = await _authorRepository.GetAllAsync();
            IOrderedEnumerable<Author> filteringListBooks = null;
            if (authorFilterModel.StateSort == Enums.StateSort.IdAsc)
            {
                filteringListBooks = listAuthors.OrderBy(x => x.Id);
            }
            if(authorFilterModel.StateSort == Enums.StateSort.IdDesc)
            {
                filteringListBooks = listAuthors.OrderByDescending(x => x.Id);
            }
            var authorsOnPage = filteringListBooks.Skip((authorFilterModel.Page - 1) * _pageSize).Take(_pageSize).ToList();            
            foreach (var author in authorsOnPage)
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
