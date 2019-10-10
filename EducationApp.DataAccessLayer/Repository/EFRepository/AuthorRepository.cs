using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorRepository : BaseEFRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {            
        }
        public async Task<IEnumerable<AuthorModel>> FilteringAsync(FilterAuthorModel filter)
        {
            var authors = _context.AuthorInPrintingEditions.Include(x => x.Author).Include(x => x.PrintingEdition).GroupBy(x => x.Author)
                                                            .Select(group => new AuthorModel
                                                            {
                                                                Id = group.Key.Id,
                                                                Name = group.Select(x => x.Author.Name).FirstOrDefault(),
                                                                PrintingEditionTitles = group.Select(x => x.PrintingEdition.Title).ToList()
                                                            });

            Expression<Func<AuthorModel, object>> expression = null;

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
