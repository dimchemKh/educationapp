using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
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
        public async Task<IEnumerable<AuthorDataModel>> FilteringAsync(BaseFilterModel filter)
        {
            var authors = _context.AuthorInPrintingEditions.Include(x => x.Author).Include(x => x.PrintingEdition)
                .Where(x => x.Author.IsRemoved == false).GroupBy(x => x.Author)
                .Select(group => new AuthorDataModel
                 {
                     Id = group.Key.Id,
                     Name = group.Select(x => x.Author.Name).FirstOrDefault(),
                 });

            Expression<Func<AuthorDataModel, object>> expression = null;

            if (filter.SortType == Enums.SortType.Id)
            {
                expression = x => x.Id;
            }
            if (filter.SortType == Enums.SortType.Name)
            {
                expression = x => x.Name;
            }

            var result = await PaginationAsync(filter, expression, authors);
            return result;
        }
    }
}
