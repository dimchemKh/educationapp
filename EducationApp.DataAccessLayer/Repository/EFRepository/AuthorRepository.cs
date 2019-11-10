﻿using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorRepository : BaseEFRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {            
        }
        public async Task<GenericModel<AuthorDataModel>> GetAllAuthorsAsync(BaseFilterModel filter)
        {
            var authors = _context.Authors
                .AsNoTracking()
                .Where(x => x.IsRemoved == false)
                .Select(x => new AuthorDataModel()
                {
                    Id = x.Id,
                    Name = x.Name
                });

            var responseModel = new GenericModel<AuthorDataModel>()
            {
                //Collection = await PaginationAsync(filter, x => x.Name, authors),
                //CollectionCount = await authors.CountAsync()
            };
            return responseModel;

        }
    }
}
