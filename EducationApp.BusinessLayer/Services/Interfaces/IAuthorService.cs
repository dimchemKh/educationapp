using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.BusinessLayer.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorModel> GetAuthorsListAsync(FilterAuthorModel authorFilterModel);
        Task<AuthorModel> CreateAuthorAsync(AuthorModelItem authorModelItem);
        Task<AuthorModel> DeleteAuthorAsync(long authorId);
        Task<AuthorModel> EditAuthorAsync(AuthorModelItem authorModel);

    }
}
