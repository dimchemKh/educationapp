using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInPrintingEditionRepository : BaseEFRepository<AuthorInPrintingEdition>, IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin)
        {
            var model = new GenericModel<PrintingEditionDataModel>();
            var printingEditions = _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => x.PrintingEdition.IsRemoved.Equals(false) && x.IsRemoved.Equals(false))
                .GroupBy(x => x.PrintingEditionId)
                .Select(x => new PrintingEditionDataModel
                {
                    Id = x.Key,
                    Currency = x.Select(z => z.PrintingEdition.Currency).FirstOrDefault(),
                    Price = x.Select(z => z.PrintingEdition.Price).FirstOrDefault(),
                    PrintingEditionType = x.Select(z => z.PrintingEdition.PrintingEditionType).FirstOrDefault(),
                    Title = x.Select(z => z.PrintingEdition.Title).FirstOrDefault(),
                    Description = x.Select(z => z.PrintingEdition.Description).FirstOrDefault(),
                    Authors = x.Select(z => new AuthorDataModel
                    {
                        Id = z.Author.Id,
                        Name = z.Author.Name
                    }).ToList()
                });

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                printingEditions = printingEditions.Where(x => x.Title.ToLower().StartsWith(filter.SearchString.ToLower()));
            }

            if (filter.PrintingEditionTypes.Any())
            {
                printingEditions = printingEditions.Where(x => filter.PrintingEditionTypes.Contains(x.PrintingEditionType));
            }

            if (!isAdmin)
            {
                printingEditions = printingEditions.Where(x => x.Price >= filter.PriceMinValue && x.Price <= filter.PriceMaxValue);
            }

            Expression<Func<PrintingEditionDataModel, object>> expression = x => x.Id;

            if (filter.SortType == Enums.SortType.Title)
            {
                expression = x => x.Title;
            }
            model.Collection = await PaginationAsync(filter, expression, printingEditions);

            model.CollectionCount = await printingEditions.CountAsync();

            return model;
        }
        public async Task<GenericModel<AuthorDataModel>> GetAuthorsFilteredDataAsync(BaseFilterModel filter)
        {
            var authors = _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Include(x => x.PrintingEdition)
                .Where(x => x.IsRemoved == false)
                .Where(x => x.Author.IsRemoved == false)
                .GroupBy(x => x.AuthorId)
                .Select(group => new AuthorDataModel
                {
                    Id = group.Key,
                    Name = group.Select(x => x.Author.Name).FirstOrDefault(),
                    PrintingEditionTitles = group.Select(z => z.PrintingEdition.Title).ToList()
                });
            Expression<Func<AuthorDataModel, object>> expression = x => x.Id;
            var responseModel = new GenericModel<AuthorDataModel>();

            if (filter.SortType == Enums.SortType.Id)
            {
                expression = x => x.Id;
            }
            if (filter.SortType == Enums.SortType.Name)
            {
                expression = x => x.Name;
            }
            responseModel.Collection = await PaginationAsync(filter, expression, authors);
            responseModel.CollectionCount = await authors.CountAsync();

            return responseModel;
        }

        public async Task<bool> UpdateAuthorsInPrintingEditionAsync(PrintingEdition printingEdition, IList<long> authorsId)
        {
            var queryAuthors = await _context.AuthorInPrintingEditions
                .Include(x => x.Author)
                .Where(x => x.PrintingEditionId.Equals(printingEdition.Id))
                .GroupBy(x => x.Author)
                .Select(x => x.Key).ToListAsync();
            var result = queryAuthors.Select(x => x.Id).ToList();
            var isEqual = result.SequenceEqual(authorsId.OrderBy(x => x));
            if (isEqual)
            {
                return false;
            }
            var removeRange = await _context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId.Equals(printingEdition.Id)).ToListAsync();
            foreach (var item in removeRange)
            {
                item.IsRemoved = true;
            }
            _context.AuthorInPrintingEditions.UpdateRange(removeRange);
            await AddAuthorsInPrintingEditionAsync(printingEdition.Id, authorsId);
            return true;
        }
        public async Task AddAuthorsInPrintingEditionAsync(long printingEditionId, IList<long> authorsId)
        {
            var authorInPrintingEditions = new List<AuthorInPrintingEdition>();
            for (int i = 0; i < authorsId.Count; i++)
            {
                authorInPrintingEditions.Add(new AuthorInPrintingEdition()
                {
                    AuthorId = authorsId[i],
                    PrintingEditionId = printingEditionId
                });
            }
            await _context.AddRangeAsync(authorInPrintingEditions);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteByIdAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate)
        {
            var authorsInPe = await _context.AuthorInPrintingEditions.Where(x => x.IsRemoved == false).Where(predicate).ToListAsync();
            _context.AuthorInPrintingEditions.AttachRange(authorsInPe);
            foreach (var item in authorsInPe)
            {
                item.IsRemoved = true;                
            }

            await _context.SaveChangesAsync();

            var pesId = _context.AuthorInPrintingEditions
                .Include(x => x.PrintingEdition)
                .Where(predicate)
                .GroupBy(x => x.PrintingEditionId)
                .Select(x => new
                {
                    peId = x.Key,
                    authors = x.Select(z => z.AuthorId).ToArray()
                }).ToList();

            if(pesId.Count == 1)
            {
                _context.AuthorInPrintingEditions.Where(x => x.IsRemoved == true).Where(x => x.PrintingEditionId == pesId.FirstOrDefault().peId).FirstOrDefault().PrintingEdition.IsRemoved = true;
                return true;
            }

            var authorId = pesId.Where(x => x.authors.Length <= 1).Select(x => x.authors.FirstOrDefault()).FirstOrDefault();
            if(authorId != null)
            {
                var q = pesId.Select(x => x.peId).ToArray();
                var pes = _context.AuthorInPrintingEditions.Where(x => x.IsRemoved == true).Where(x => q.Contains(x.PrintingEditionId)).ToList();
                foreach (var item in pes)
                {
                    item.PrintingEdition.IsRemoved = true;
                }
                _context.AuthorInPrintingEditions.Where(x => x.IsRemoved == true).Where(x => x.Author.Id == authorId).Select(x => x.Author).FirstOrDefault().IsRemoved = true;
            }

            //if(authorId != null)
            //{
            //    var temp = _context.AuthorInPrintingEditions.Where(x => x.IsRemoved == true).Where(x => x.AuthorId == authorId.FirstOrDefault()).ToList();
            //    foreach (var item in temp)
            //    {
            //        item.Author.IsRemoved = true;
            //    }
            //}

            //foreach (var item in pesId)
            //{
            //    if(item.authors.Length <= 1)
            //    {
            //        //_context.AuthorInPrintingEditions.Where(x => x.PrintingEditionId == pesId.FirstOrDefault().peId).FirstOrDefault().PrintingEdition.IsRemoved = true;
            //    }
            //}

            await _context.SaveChangesAsync();

            //var pesId = _context.AuthorInPrintingEditions
            //    .Where(x => x.IsRemoved == false)
            //    .GroupBy(x => x.PrintingEditionId)
            //    .Select(x => new
            //    {
            //        PeId = x.Key,
            //        AuthorsId = x.Select(z => z.AuthorId).ToList()
            //    });
            //.Where(x => x.AuthorsId.Count <= 1)
            //.Where(x => x.AuthorsId.Contains(authorsInPe.Select(z => z.AuthorId).FirstOrDefault()))
            //.Select(x => x.PeId)
            //.ToArray();

            //var authorsId = _context.AuthorInPrintingEditions
            //    .Where(x => x.IsRemoved == false)
            //    .Where(x => pesId.Contains(x.PrintingEditionId))
            //    .GroupBy(x => x.Author)
            //    .Select(x => new { AuthorId = x.Key, counts = x.Select(z => z.AuthorId).ToArray() });

            //var printingEditions = await _context.AuthorInPrintingEditions
            //    .Where(x => x.IsRemoved == false)
            //    .Include(x => x.PrintingEdition)
            //    //.Where(x => pesId.Contains(x.PrintingEditionId))
            //    .Select(z => z.PrintingEdition)
            //    .ToListAsync();

            //_context.PrintingEditions.AttachRange(printingEditions);
            
            //foreach (var printingEdition in printingEditions)
            //{
            //    printingEdition.IsRemoved = true;
            //}

            //await _context.SaveChangesAsync();
            return true;
        }
    }
    public class TestModel
    {
        public long PeId { get; set; }
        public ICollection<long> authors = new List<long>();
    }
}
