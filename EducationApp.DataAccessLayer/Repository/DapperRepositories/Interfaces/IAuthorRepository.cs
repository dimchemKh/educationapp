﻿using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces
{
    public interface IAuthorRepository : IBaseDapperRepository<Author>
    {
        Task<GenericModel<AuthorDataModel>> GetAuthorsLazyLoadAsync(BaseFilterModel filter);
        Task<GenericModel<AuthorDataModel>> GetAllAuthorsAsync(BaseFilterModel filter);
    }
}
