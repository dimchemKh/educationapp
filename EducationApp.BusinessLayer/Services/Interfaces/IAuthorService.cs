using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Filters.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorModel> GetAllAuthorsAsync(FilterAuthorModel authorFilterModel);
        Task<AuthorModel> CreateAuthorAsync(AuthorModelItem authorModelItem);
        Task<AuthorModel> DeleteAuthorAsync(long authorId);
        Task<AuthorModel> UpdateAuthorAsync(AuthorModelItem authorModel);
        Task<AuthorModel> GetAuthorsLazyLoadAsync(FilterAuthorModel filterModel);
    }
}
