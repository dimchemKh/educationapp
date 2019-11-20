using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces;
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

            Expression<Func<PrintingEditionDataModel, object>> predicate = x => x.Id;
            var orderBySql = " pe.Id ";
            var priceSql = string.Empty;
            if (!isAdmin)
            {
                priceSql += $"AND pe.Price BETWEEN {filter.PriceMinValue} AND {filter.PriceMaxValue} ";
                orderBySql = " pe.Price ";
                predicate = b => b.Price;
            }

            if (filter.SortType.Equals(Enums.SortType.Title))
            {
                orderBySql = " pe.Title ";
                predicate = b => b.Title;
            }
            if (filter.SortType.Equals(Enums.SortType.Price))
            {
                orderBySql = " pe.Price ";
                predicate = b => b.Price;
            }

            var offsetSql = string.Empty;

            var orderBy = $"ORDER BY {orderBySql} ";
            var selectSql = $@"AiNP.Id, AiNP.AuthorId, AiNP.PrintingEditionId, a.Id, a.CreationDate,
                                a.IsRemoved, a.Name, pe.Title, pe.Price";
            var mainSql = $@"SELECT COUNT (pe.Id)
                                FROM AuthorInPrintingEditions AS AiNP
                                INNER JOIN Authors AS a ON AiNP.AuthorId = a.Id
                                INNER JOIN (
                                    SELECT pe.Id, pe.Price, pe.Title FROM PrintingEditions AS pe ";

            var filterSql = $"\nWHERE (pe.IsRemoved = 0) AND pe.PrintingEditionType IN {sqlTypes}) {priceSql}";

            var orderSql = $@"{orderBy}
                            ) AS pe ON AiNP.PrintingEditionId = pe.Id 
                            WHERE pe.Title LIKE '{filter.SearchString}%' OR a.Name LIKE '{filter.SearchString}%'
                            {orderBy};";

            var countSql = mainSql + filterSql + orderSql;

            mainSql = mainSql.Replace("COUNT (pe.Id)", selectSql);

            var newOrderSql = $@"{orderBy}
                                OFFSET { (filter.Page - 1) * filter.PageSize} ROWS FETCH NEXT { filter.PageSize} ROWS ONLY
                            ) AS pe ON AiNP.PrintingEditionId = pe.Id 
                            WHERE pe.Title LIKE '{filter.SearchString}%' OR a.Name LIKE '{filter.SearchString}%'
                            {orderBy};";

            var resultSql = mainSql + filterSql + newOrderSql;

            using (var connection = SqlConnection())
            {
                try
                {
                    var result = await connection.QueryMultipleAsync(resultSql + countSql);
                    var pe = result.Read().ToList();
                    var authors = result.Read().ToArray();
                    var count = result.Read();
                    foreach (var item in pe)
                    {
                        var authorPe = authors
                            .Where(x => x.PrintingEditionId == item.Id)
                            .Select(x => new AuthorDataModel
                            {
                                Name = x.Name
                            }).ToArray();

                        item.Authors = authorPe;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

                
            }

            //query = res.GroupBy(x => x.Id).Select(x => x.Select(pe => new PrintingEditionDataModel
            //{
            //    Id = pe.Id,
            //    Title = pe.Title,
            //    Description = pe.Description,
            //    Currency = (Enums.Currency)pe.Currency,
            //    Price = pe.Price,
            //    PrintingEditionType = (Enums.PrintingEditionType)pe.PrintingEditionType,
            //    Authors = res.Where(author => author.Id == pe.Id).Select(author => new AuthorDataModel { Name = author.Name }).ToArray()
            //}).FirstOrDefault()).AsQueryable();

            //responseModel.CollectionCount = query.Count();

            //var responsePage = await PaginationAsync(filter, predicate, query);

            //responseModel.Collection = responsePage;

            return responseModel;
        }
    }
}
