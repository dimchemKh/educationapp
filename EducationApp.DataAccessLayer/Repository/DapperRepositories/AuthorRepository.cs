using Dapper;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class AuthorRepository : BaseDapperRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<GenericModel<AuthorDataModel>> GetAuthorsLazyLoadAsync(BaseFilterModel filter)
        {
            var responseModel = new GenericModel<AuthorDataModel>();

            var sql = $@"SELECT a.Id, a.Name FROM Authors AS a
                            WHERE a.IsRemoved = 0
                            ORDER BY a.Name
                            OFFSET {(filter.Page - 1) * filter.PageSize} ROWS FETCH NEXT {filter.PageSize} ROWS ONLY

                            SELECT COUNT(a.Id) FROM Authors AS a
                            WHERE a.IsRemoved = 0";

            using(var connection = SqlConnection())
            {
                var result = await connection.QueryMultipleAsync(sql);

                responseModel.Collection = await result.ReadAsync<AuthorDataModel>();
                responseModel.CollectionCount = (await result.ReadAsync<int>()).FirstOrDefault();
            }

            return responseModel;
        }
        public async Task<GenericModel<AuthorDataModel>> GetAllAuthorsAsync(BaseFilterModel filter)
        {
            var responseModel = new GenericModel<AuthorDataModel>();


            var predicateSql = $"a.Id";

            if (filter.SortType == Enums.SortType.Name)
            {
                predicateSql = $"a.Name";
            }
            var sort = string.Empty;
            if (filter.SortState.Equals(Enums.SortState.Asc))
            {
                sort = "ASC";
            }
            if (filter.SortState.Equals(Enums.SortState.Desc))
            {
                sort = "DESC";
            }

            var columnSql = $"a.Id, a.Name, p.Id, p.Title";
            var offset = string.Empty;
            var orderSql = string.Empty;

            var countBuilder = new StringBuilder($@"SELECT COUNT(DISTINCT a.Id)
                        FROM AuthorInPrintingEditions AS aInP
                        LEFT JOIN PrintingEditions AS p ON aInP.PrintingEditionId = p.Id
                        INNER JOIN (
	                        SELECT a.Id, a.Name
	                        FROM Authors AS a
	                        WHERE a.IsRemoved = 0");

            var mainBuilder = new StringBuilder(countBuilder.ToString()).Replace("COUNT(DISTINCT a.Id)", columnSql);

            var endSql = $@") AS a ON aInP.AuthorId = a.Id";

            countBuilder.Append(orderSql).Append(offset).Append(endSql).Append(orderSql);


            orderSql = $"ORDER BY {predicateSql} {sort}";
            offset = $"OFFSET {(filter.Page - 1) * filter.PageSize} ROWS FETCH NEXT {filter.PageSize} ROWS ONLY";

            mainBuilder.Append(endSql);

            var res = mainBuilder.Append(countBuilder.ToString()).ToString();

            var authors = new List<AuthorDataModel>();

            using(var connection = SqlConnection())
            {
                var result = await connection.QueryMultipleAsync(countBuilder.ToString());

                var dict = new Dictionary<long, AuthorDataModel>();

                authors = result.Read<AuthorDataModel, PrintingEdition, AuthorDataModel>(
                    (author, printingEdition) =>
                    {
                        AuthorDataModel model;

                        if (!dict.TryGetValue(author.Id, out model))
                        {
                            model = author;

                            model.PrintingEditionTitles = new List<string>();

                            dict.Add(model.Id, model);
                        }
                        model.PrintingEditionTitles.Add(printingEdition.Title);
                        return model;
                    }, splitOn: "Id")
                    .Distinct()
                    .ToList();
                responseModel.CollectionCount = result.Read<int>().FirstOrDefault();
            }            
            
            return responseModel;
        }

    }
}
