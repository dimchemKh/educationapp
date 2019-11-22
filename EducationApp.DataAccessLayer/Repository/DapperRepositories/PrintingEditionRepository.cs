using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Linq.Expressions;

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

            var sql = "SELECT b.Id, b.Title, b.Price, b.Description, b.Currency, b.PrintingEditionType, t.Name " +
                "FROM PrintingEditions as b " +
                "LEFT JOIN AuthorInPrintingEditions as ap ON ap.PrintingEditionId = b.Id " +
                $"LEFT JOIN Authors as t ON t.Id = ap.AuthorId WHERE b.Id = {printingEditionId}";

            using(var connection = SqlConnection())
            {
                connection.Open();
                var res = await connection.QueryAsync(sql);
                printingEditionData = res.Select(x => new PrintingEditionDataModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Currency = (Enums.Currency)x.Currency,
                    Price = x.Price,
                    PrintingEditionType = (Enums.PrintingEditionType)x.PrintingEditionType
                }).FirstOrDefault();
                printingEditionData.Authors = res.Select(x => new AuthorDataModel { Name = x.Name }).ToArray();

            }

            return printingEditionData;
        }
        public async Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin)
        {
            var responseModel = new GenericModel<PrintingEditionDataModel>();

            var sqlTypes = "(";
            var i = 0;
            foreach (var type in filter.PrintingEditionTypes)
            {
                sqlTypes += $"{(int)type},";
                ++i;
                if (i == filter.PrintingEditionTypes.Count())
                {
                    sqlTypes = sqlTypes.Substring(0, sqlTypes.Length - 1);
                }
            }
            var searchSql = string.Empty;
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                searchSql = $@"AND (COALESCE((
						SELECT TOP(1) CASE
						WHEN (LOWER(aut.Name)) LIKE '{filter.SearchString}%'
						THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
						END
						FROM AuthorInPrintingEditions AS aPe
						INNER JOIN Authors AS aut ON aut.Id = aPe.AuthorId
						WHERE pe.Id = aPe.PrintingEditionId
					    ),0) = 1) OR (LOWER(pe.Title) LIKE '{filter.SearchString}%')";
            }

            var orderBySql = "pe.Id";
            var priceSql = string.Empty;

            if (!isAdmin)
            {
                priceSql += $"AND pe.Price BETWEEN {filter.PriceMinValue} AND {filter.PriceMaxValue} ";
                orderBySql = "pe.Price";
            }

            if (filter.SortType.Equals(Enums.SortType.Title))
            {
                orderBySql = "pe.Title";
            }
            if (filter.SortType.Equals(Enums.SortType.Price))
            {
                orderBySql = "pe.Price";
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
            var offsetSql = string.Empty;

            var orderBy = $"ORDER BY {orderBySql} {sort}";

            var columnSelectBuilder = "pe.Id, pe.Title, pe.Price, pe.Description, pe.Currency, pe.PrintingEditionType, a.Id, a.Name";

            var countBuilder = new StringBuilder(@"SELECT COUNT(DISTINCT pe.Id)
                                                    FROM AuthorInPrintingEditions AS AiNP
                                                    INNER JOIN Authors AS a ON AiNP.AuthorId = a.Id
                                                    INNER JOIN (
                                                        SELECT pe.Id, pe.Price, pe.Title, pe.Description, pe.Currency, pe.PrintingEditionType FROM PrintingEditions AS pe ");

            var mainBuilder = new StringBuilder(countBuilder.ToString().Replace("COUNT(DISTINCT pe.Id)", columnSelectBuilder));

            var filterSqlBuilder = $"\nWHERE (pe.IsRemoved = 0) AND pe.PrintingEditionType IN {sqlTypes}) {priceSql} {searchSql}";

            var endBuilder = $@") AS pe ON AiNP.PrintingEditionId = pe.Id;";

            var newOrderSql = $@"{orderBy}
                                OFFSET {(filter.Page - 1) * filter.PageSize} ROWS FETCH NEXT {filter.PageSize} ROWS ONLY
                            ) AS pe ON AiNP.PrintingEditionId = pe.Id
                            {orderBy};";

            var countSql = countBuilder.Append(filterSqlBuilder).Append(endBuilder);

            var mainSql = mainBuilder.Append(filterSqlBuilder).Append(newOrderSql);

            var resultSql = mainSql.Append(countSql.ToString()).ToString();

            var printingEditions = new List<PrintingEditionDataModel>();
            
            using (var connection = SqlConnection())
            {
                var dict = new Dictionary<long, PrintingEditionDataModel>();

                var result = await connection.QueryMultipleAsync(resultSql);

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
