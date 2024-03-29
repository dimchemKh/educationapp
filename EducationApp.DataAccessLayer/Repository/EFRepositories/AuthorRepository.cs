﻿using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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
                CollectionCount = await authors.CountAsync()
            };

            var authorsPage = await PaginationAsync(filter, x => x.Name, authors);

            responseModel.Collection = authorsPage;

            return responseModel;
        }
        public async Task<GenericModel<AuthorDataModel>> GetFilteredAuthorsAsync(BaseFilterModel filter)
        {
            var authors = _context.Authors
                .Where(x => x.IsRemoved == false)
                .Where(x => x.AuthorInPrintingEditions.Select(z => z.AuthorId == x.Id).FirstOrDefault())
                .Include(x => x.AuthorInPrintingEditions)
                .ThenInclude(x => x.PrintingEdition);

            Expression<Func<Author, object>> predicate = x => x.Id;

            if (filter.SortType == Enums.SortType.Name)
            {
                predicate = x => x.Name;
            }

            var responseModel = new GenericModel<AuthorDataModel>
            {
                CollectionCount = authors.Count()
            };

            var authorsPage = await PaginationAsync(filter, predicate, authors);

            var result = authorsPage.Select(x => new AuthorDataModel
            {
                Id = x.Id,
                Name = x.Name,
                PrintingEditionTitles = x.AuthorInPrintingEditions
                .Select(z => z.PrintingEdition.Title).ToArray()
            });

            responseModel.Collection = result;

            return responseModel;
        }

    }
}
