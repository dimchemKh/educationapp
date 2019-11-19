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
            var sqlQuery = $@"SELECT Id, Title, Price, Currency, PrintingEditionType, t.Name 
                                FROM PrintingEditions
                                LEFT JOIN AuthorInPrintingEditions as ap ON ap.PrintingEditionId = Id 
                                LEFT JOIN Authors as t ON t.Id = ap.AuthorId
                                WHERE PrintingEditionType IN {sqlTypes}) ";

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                sqlQuery += $"AND Title LIKE '{filter.SearchString}%' ";
            }
            

            Expression<Func<PrintingEditionDataModel, object>> predicate = x => x.Id;
            var orderBySql = "Id";

            if (!isAdmin)
            {
                sqlQuery += $"AND Price BETWEEN {filter.PriceMinValue} AND {filter.PriceMaxValue} ";
                orderBySql = "Price";
                predicate = b => b.Price;
            }

            if (filter.SortType.Equals(Enums.SortType.Title))
            {
                orderBySql = "Title";
                predicate = b => b.Title;
            }
            if (filter.SortType.Equals(Enums.SortType.Price))
            {
                orderBySql = "Price";
                predicate = b => b.Price;
            }

            sqlQuery += $"ORDER BY {orderBySql}";

            //IQueryable<PrintingEditionDataModel> query = null;

            //IEnumerable<dynamic> res;

            using(var connection = SqlConnection())
            {
                var authors = new Dictionary<long, PrintingEdition>();

                var list = connection.Query<PrintingEdition, AuthorInPrintingEdition, Author, PrintingEditionDataModel>(
                sqlQuery,
                (printingEdition, AiPe, author) =>
                {
                    PrintingEditionDataModel printingEditionData;

                    printingEditionData = new PrintingEditionDataModel { Id = printingEdition.Id };
                    //if (!authors.TryGetValue(printingEdition.Id, out peEntry))
                    //{
                    //    peEntry = printingEdition;
                    //    peEntry.AuthorInPrintingEditions = new List<OrderDetail>();
                    //    authors.Add(orderEntry.OrderID, orderEntry);
                    //}

                    //orderEntry.OrderDetails.Add(orderDetail);

                    return printingEditionData;
                })
            .Distinct()
            .ToList();


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
