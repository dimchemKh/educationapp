using EducationApp.BusinessLogic.Models.Authors;
using EducationApp.BusinessLogic.Models.Filters;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogic.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorModel> GetFilteredAuthorsAsync(FilterAuthorModel authorFilterModel);
        Task<AuthorModel> CreateAuthorAsync(AuthorModelItem authorModelItem);
        Task<AuthorModel> DeleteAuthorAsync(long authorId);
        Task<AuthorModel> UpdateAuthorAsync(AuthorModelItem authorModel);
        Task<AuthorModel> GetAllAuthorsAsync(FilterAuthorModel filterModel);
    }
}
