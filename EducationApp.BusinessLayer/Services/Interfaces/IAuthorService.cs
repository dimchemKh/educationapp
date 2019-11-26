using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Models.Filters;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
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
