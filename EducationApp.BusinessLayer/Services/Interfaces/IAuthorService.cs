using EducationApp.BusinessLayer.Models.Authors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorModel> GetAuthorsListAsync(AuthorModel authorModel, AuthorFilterModel authorFilterModel);
        Task<AuthorModel> AddNewAuthorAsync(AuthorModelItem authorModelItem);
        Task<AuthorModel> DeleteAuthorAsync(AuthorModel authorModel, int authorId);
        Task<AuthorModel> EditAuthorAsync(AuthorModelItem authorModel);

    }
}
