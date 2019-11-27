using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using EducationApp.DataAccessLayer.Models.Orders;
using EducationApp.DataAccessLayer.Models.OrderItems;
using EducationApp.DataAccessLayer.Models.Filters;
using System.Linq.Expressions;
using System;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using EducationApp.DataAccessLayer.Models;
using static EducationApp.DataAccessLayer.Entities.Enums.Enums;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class OrderRepository : BaseEFRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<GenericModel<OrderDataModel>> GetAllOrdersAsync(FilterOrderModel filterOrder, long userId)
        {
            var query = _context.Orders
                .Include(x => x.Payment)
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.PrintingEdition)
                .AsQueryable();

            if (userId > 1)
            {
                query = query.Where(x => x.User.Id.Equals(userId));
            }

            if (filterOrder.TransactionStatus.Equals(TransactionStatus.Paid))
            {
                query = query.Where(x => x.Payment.TransactionId != null);
            }
            if (filterOrder.TransactionStatus.Equals(TransactionStatus.UnPaid))
            {
                query = query.Where(x => x.Payment.TransactionId == null);
            }

            var orders = query.Select(x => new OrderDataModel
            {
                Id = x.Id,
                Amount = x.Amount,
                CreationDate = x.CreationDate,
                Email = x.User.Email,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Currency = x.OrderItems.Select(z => z.Currency).FirstOrDefault(),
                PaymentId = x.Payment.TransactionId,
                OrderItems = x.OrderItems.Select(z => new OrderItemDataModel
                {
                    Title = z.PrintingEdition.Title,
                    Count = z.Count,
                    PrintingEditionType = z.PrintingEdition.PrintingEditionType,
                    Amount = z.Amount,
                    Currency = z.Currency
                }).ToList()
            });

            Expression<Func<OrderDataModel, object>> predicate = x => x.Id;

            if (filterOrder.SortType.Equals(SortType.Amount))
            {
                predicate = x => x.Amount;
            }
            if (filterOrder.SortType.Equals(SortType.Date))
            {
                predicate = x => x.CreationDate;
            }

            var responseModel = new GenericModel<OrderDataModel>()
            {
                CollectionCount = orders.Count()
            };
            var ordersPage = await PaginationAsync(filterOrder, predicate, orders);

            responseModel.Collection = ordersPage;

            return responseModel;
        }
        public async Task<bool> UpdateTransactionAsync(long orderId, string transactionId)
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

            var result = await SaveAsync();

            if (result.Equals(0))
            {
                return false;
            }
            return true;
        }
    }
}
