using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<GenericModel<AuthorDataModel>> GetAllAuthorsAsync(BaseFilterModel filter);
        Task<GenericModel<AuthorDataModel>> GetFilteredAuthorsAsync(BaseFilterModel filter);

    }
}
