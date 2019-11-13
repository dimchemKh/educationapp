using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces
{
    public interface IAuthorRepository : IBaseEFRepository<Author>
    {
        Task<GenericModel<AuthorDataModel>> GetAuthorsLazyLoadAsync(BaseFilterModel filter);
        Task<GenericModel<AuthorDataModel>> GetAllAuthorsAsync(BaseFilterModel filter);

    }
}
