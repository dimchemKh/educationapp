using Dapper;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.OrderItems;
using EducationApp.DataAccessLayer.Models.Orders;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EducationApp.DataAccessLayer.Entities.Enums.Enums;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class OrderRepository : BaseDapperRepository<Order>, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<GenericModel<OrderDataModel>> GetAllOrdersAsync(FilterOrderModel filterOrder, long userId)
        {
            var responseModel = new GenericModel<OrderDataModel>();


            var columnSql = $"oi.Id, oi.Count, pe.Id, pe.Title, pe.PrintingEditionType, o.Id, o.CreationDate, o.Amount, pa.Id, pa.TransactionId, u.Id, u.FirstName, u.LastName, u.Email";

            var searchUserSql = string.Empty;

            var offsetSql = string.Empty;


            if (userId > 1)
            {
                searchUserSql = $"AND o.UserId = {userId}";
            }
            var transactionSql = string.Empty;
            if (filterOrder.TransactionStatus.Equals(TransactionStatus.Paid))
            {
                transactionSql = $"AND pa.TransactionId IS NOT NULL ";
            }
            if (filterOrder.TransactionStatus.Equals(TransactionStatus.UnPaid))
            {
                transactionSql = $"AND pa.TransactionId IS NULL ";
            }

            var predicateSql = $"o.Id";

            if (filterOrder.SortType.Equals(SortType.Amount))
            {
                predicateSql = $"o.Amount"; // todo change
            }

            if (filterOrder.SortType.Equals(SortType.Date))
            {
                predicateSql = $"o.CreationDate";
            }

            var sort = string.Empty;

            if (filterOrder.SortState.Equals(SortState.Asc))
            {
                sort = "ASC";
            }

            if (filterOrder.SortState.Equals(SortState.Desc))
            {
                sort = "DESC";
            }

            var orderSql = string.Empty;

            var countBuilder = new StringBuilder($@"
                                SELECT COUNT(DISTINCT o.Id)
                                FROM OrderItems AS oi
                                INNER JOIN PrintingEditions AS pe ON pe.Id = oi.PrintingEditionId                                                               
                                INNER JOIN (
	                                SELECT  o.Id, o.CreationDate, o.Amount, o.UserId, o.PaymentId
	                                FROM Orders AS o
	                                INNER JOIN Payments AS pa ON o.PaymentId = pa.Id
	                                WHERE o.IsRemoved = 0 {transactionSql} {searchUserSql} ");

            var endSql = $@"
                    ) AS o ON o.Id = oi.OrderId
                    INNER JOIN Payments AS pa ON pa.Id = o.PaymentId
                    INNER JOIN AspNetUsers AS u ON u.Id = o.UserId;";

            var mainBuilder = new StringBuilder(countBuilder.ToString().Replace("COUNT(DISTINCT o.Id)", columnSql));

            orderSql = $@"ORDER BY {predicateSql} {sort}
                          OFFSET {(filterOrder.Page - 1) * filterOrder.PageSize} ROWS FETCH NEXT {filterOrder.PageSize} ROWS ONLY";

            var resultSql = mainBuilder.Append(orderSql)
                                       .Append(endSql)
                                       .Append(countBuilder.Append(endSql).ToString()).ToString();

            var orders = new List<OrderDataModel>();
            
            using (var connection = SqlConnection())
            {
                var dict = new Dictionary<long, OrderDataModel>();

                var result = await connection.QueryMultipleAsync(resultSql);

                orders = result.Read<OrderItem, PrintingEdition, Order, Payment, ApplicationUser, OrderDataModel>(
                    (orderItem, printingEdition, order, payment, user) =>
                    {
                        OrderDataModel model;

                        if (!dict.TryGetValue(order.Id, out model))
                        {
                            model = new OrderDataModel();
                            model.Id = order.Id;
                            model.CreationDate = order.CreationDate;
                            model.Amount = order.Amount;
                            model.FirstName = user.FirstName;
                            model.LastName = user.LastName;
                            model.Email = user.Email;
                            model.PaymentId = payment.TransactionId;

                            model.OrderItems = new List<OrderItemDataModel>();

                            dict.Add(model.Id, model);
                        }
                        model.OrderItems.Add(new OrderItemDataModel
                        {
                            PrintingEditionType = printingEdition.PrintingEditionType,
                            Count = orderItem.Count,
                            Title = printingEdition.Title
                        });

                        return model;
                    },
                    splitOn: "Id")
                    .Distinct()
                    .ToList();
                responseModel.CollectionCount = result.Read<int>().FirstOrDefault();                    
            }

            responseModel.Collection = orders;

            return responseModel;
        }
        public async Task<bool> UpdateTransactionAsync(long orderId, string transactionId)
        {
            var sql = new StringBuilder($@"UPDATE Payments
                         SET TransactionId = '{transactionId}'
                         WHERE Id = (SELECT o.PaymentId
                         FROM Orders AS o
                         WHERE o.Id = {orderId})");

            using(var connection = SqlConnection())
            {
                 await connection.QueryFirstOrDefaultAsync(sql.ToString());
            }
            return true;
        }
    }
}
