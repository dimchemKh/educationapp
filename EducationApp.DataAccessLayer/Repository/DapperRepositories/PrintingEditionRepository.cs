using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class PrintingEditionRepository : BaseDapperRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<PrintingEditionDataModel> GetPrintingEditionDetailsAsync(long printingEditionId)
        {
            var printingEditionData = new PrintingEditionDataModel();

            var sql = new StringBuilder($@"SELECT b.Id, b.Title, b.Price, b.Description, b.Currency, b.PrintingEditionType, t.Name
                                           FROM PrintingEditions as b
                                           LEFT JOIN AuthorInPrintingEditions as ap ON ap.PrintingEditionId = b.Id 
                                           LEFT JOIN Authors as t ON t.Id = ap.AuthorId WHERE b.Id = @printingEditionId");

            var queryResult = new List<dynamic>();

            using(var connection = GetSqlConnection())
            {
                connection.Open();

                queryResult = (await connection.QueryAsync(sql.ToString(), new { printingEditionId })).ToList();
            }

            printingEditionData = queryResult.Select(x => new PrintingEditionDataModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Currency = (Enums.Currency)x.Currency,
                Price = x.Price,
                PrintingEditionType = (Enums.PrintingEditionType)x.PrintingEditionType
            }).FirstOrDefault();

            printingEditionData.Authors = queryResult.Select(x => new AuthorDataModel
            {
                Name = x.Name
            }).ToArray();

            return printingEditionData;
        }
        public async Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin)
        {
            var responseModel = new GenericModel<PrintingEditionDataModel>();

            var priceSql = string.Empty;
            var sort = string.Empty;
            var offsetSql = string.Empty;

            var searchSql = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                searchSql.Append($@"AND (COALESCE((
						SELECT TOP(1) CASE
						WHEN (LOWER(aut.Name)) LIKE @SearchString+'%'
						THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
						END
						FROM AuthorInPrintingEditions AS aPe
						INNER JOIN Authors AS aut ON aut.Id = aPe.AuthorId
						WHERE pe.Id = aPe.PrintingEditionId
					    ),0) = 1) OR (LOWER(pe.Title) LIKE @SearchString+'%')");
            }

            var sortTypeSql = "Id";

            if (!isAdmin)
            {
                priceSql = $@"AND pe.Price BETWEEN @PriceMinValue AND @PriceMaxValue ";
                sortTypeSql = "Price";
            }

            if (filter.SortType.Equals(Enums.SortType.Title))
            {
                sortTypeSql = "Title";
            }
            if (filter.SortType.Equals(Enums.SortType.Price))
            {
                sortTypeSql = "Price";
            }

            if (filter.SortState.Equals(Enums.SortState.Asc))
            {
                sort = "ASC";
            }
            if (filter.SortState.Equals(Enums.SortState.Desc))
            {
                sort = "DESC";
            }
            var orderBySql = $"pe.{sortTypeSql}";

            var orderBy = $"ORDER BY {orderBySql} {sort}";

            var sqlPrintingEditionTypes = new StringBuilder($"AND (pe.PrintingEditionType = {(int)filter.PrintingEditionTypes.FirstOrDefault()} ");

            foreach (var type in filter.PrintingEditionTypes.Skip(1))
            {
                sqlPrintingEditionTypes.Append($"OR pe.PrintingEditionType = {(int)type} ");
            }

            var columnSelectBuilder = new StringBuilder("pe.Id, pe.Title, pe.Price, pe.Description, pe.Currency, pe.PrintingEditionType, a.Id, a.Name");

            var countBuilder = new StringBuilder(@"SELECT COUNT(DISTINCT pe.Id)
                                                    FROM AuthorInPrintingEditions AS AiNP
                                                    INNER JOIN Authors AS a ON AiNP.AuthorId = a.Id
                                                    INNER JOIN (
                                                        SELECT pe.Id, pe.Price, pe.Title, pe.Description, pe.Currency, pe.PrintingEditionType FROM PrintingEditions AS pe ");

            var mainBuilder = new StringBuilder(countBuilder.ToString().Replace("COUNT(DISTINCT pe.Id)", columnSelectBuilder.ToString()));

            var filterSqlBuilder = new StringBuilder($"\nWHERE (pe.IsRemoved = 0) {sqlPrintingEditionTypes.ToString()}) {priceSql} {searchSql.ToString()}");

            var endBuilder = $@") AS pe ON AiNP.PrintingEditionId = pe.Id;";

            var newOrderSql = new StringBuilder($@"{orderBy}
                                OFFSET (@Page - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
                            ) AS pe ON AiNP.PrintingEditionId = pe.Id
                            {orderBy};");

            var countSql = countBuilder.Append(filterSqlBuilder.ToString())
                                       .Append(endBuilder);

            var mainSql = mainBuilder.Append(filterSqlBuilder.ToString())
                                     .Append(newOrderSql.ToString());

            var resultSql = mainSql.Append(countSql.ToString()).ToString();

            var printingEditions = new List<PrintingEditionDataModel>();
            
            using (var connection = GetSqlConnection())
            {
                var dict = new Dictionary<long, PrintingEditionDataModel>();

                var result = await connection.QueryMultipleAsync(resultSql, filter);

                printingEditions = result.Read<PrintingEditionDataModel, Author, PrintingEditionDataModel>(
                    (pe, author) =>
                    {
                        PrintingEditionDataModel model;

                        if (!dict.TryGetValue(pe.Id, out model))
                        {
                            model = pe;

                            model.Authors = new List<AuthorDataModel>();

                            dict.Add(model.Id, model);
                        }
                        model.Authors.Add(new AuthorDataModel
                        {
                            Id = author.Id,
                            Name = author.Name
                        });

                        return model;
                    }, splitOn: "Id")
                    .Distinct()
                    .ToList();

                responseModel.CollectionCount = result.Read<int>().FirstOrDefault();                            
            }

            responseModel.Collection = printingEditions;

            return responseModel;
        }
    }
}
