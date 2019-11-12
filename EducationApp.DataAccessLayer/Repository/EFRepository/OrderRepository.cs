using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using EducationApp.DataAccessLayer.Models.Orders;
using System.Collections.Generic;
using EducationApp.DataAccessLayer.Models.OrderItems;
using EducationApp.DataAccessLayer.Models.Filters;
using System.Linq.Expressions;
using System;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.DataAccessLayer.Models;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class OrderRepository : BaseEFRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<GenericModel<OrderDataModel>> GetAllOrdersAsync(FilterOrderModel filterOrder, long userId)
        {
            var query = _context.OrderItems
                .AsNoTracking()
                .Include(x => x.PrintingEdition)
                .Include(x => x.Order)
                .AsQueryable();

            if (userId > 1)
            {
                query = query.Where(x => x.Order.User.Id.Equals(userId));
            }

            var orders = query
                .GroupBy(x => x.OrderId)
                .Select(x => new OrderDataModel
                {
                    Id = x.Key,
                    Amount = x.Select(z => z.Amount).Sum(),
                    Date = x.Select(z => z.CreationDate).FirstOrDefault(),
                    Email = x.Select(z => z.Order.User.Email).FirstOrDefault(),
                    UserName = x.Select(z => $"{z.Order.User.FirstName} {z.Order.User.LastName}").FirstOrDefault(),
                    PaymentId = x.Select(z => z.Order.Payment.TransactionId).FirstOrDefault(),
                    Currency = x.Select(z => z.Currency).FirstOrDefault(),
                    OrderItems = x.Select(z => new OrderItemDataModel
                    {
                        Title = z.PrintingEdition.Title,
                        Count = z.Count,
                        PrintingEditionType = z.PrintingEdition.PrintingEditionType,
                        Amount = z.Amount,
                        Currency = z.Currency
                    }).ToList()
                });

            Expression<Func<OrderDataModel, object>> predicate = x => x.Id;

            if (filterOrder.SortType.Equals(Enums.SortType.Amount))
            {
                predicate = x => x.Amount;
            }
            if (filterOrder.SortType.Equals(Enums.SortType.PrintingEditionType))
            {
                predicate = x => x.Date;
            }
            if (filterOrder.SortType.Equals(Enums.SortType.TransactionStatus))
            {
                predicate = x => x.TransactionStatus;
            }

            var responseModel = new GenericModel<OrderDataModel>()
            {
                CollectionCount = orders.Count()
            };
            var ordersPage = await PaginationAsync(filterOrder, predicate, orders);

            responseModel.Collection.AddRange(ordersPage);

            return responseModel;
        }
        public async Task<bool> UpdateTransactionAsync(long orderId, long transactionId)
        {
            var payment = await _context.Orders
                .Where(x => x.Id.Equals(orderId))
                .Select(x => x.Payment)
                .FirstOrDefaultAsync();

            if(payment == null)
            {
                return false;
            }

            payment.TransactionId = transactionId;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
