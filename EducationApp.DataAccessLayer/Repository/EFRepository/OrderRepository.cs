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

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class OrderRepository : BaseEFRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }
        private async Task<IEnumerable<DalOrderModel>> Filtering(FilterOrderModel filterOrder, IQueryable<DalOrderModel> ordersList)
        {
            Expression<Func<DalOrderModel, object>> lambda = null;
            if (filterOrder.SortType == Enums.SortType.Id)
            {
                lambda = x => x.Id;
            }
            if (filterOrder.SortType == Enums.SortType.PrintingEditionType)
            {
                lambda = x => x.Date;
            }
            if (filterOrder.SortType == Enums.SortType.TransactionStatus)
            {
                lambda = x => x.TransactionStatus;
            }

            var result = await PaginationAsync(filterOrder, lambda, ordersList);
            return result;
        }
        public async Task<IEnumerable<DalOrderModel>> GetOrdersAsync(FilterOrderModel filterOrder, long userId)
        {
            var ordersList = _context.OrderItems.Include(x => x.PrintingEdition).Include(x => x.Order).Where(x => x.Order.User.Id == userId).GroupBy(x => x.OrderId)
                .Select(x => new DalOrderModel
                {
                    Id = x.Key,
                    Amount = x.Select(z => z.Amount).Sum(),
                    Date = x.Select(z => z.CreationDate).FirstOrDefault(),
                    TransactionStatus = x.Select(z => z.Order.TransactionStatus).FirstOrDefault(),
                    PaymentId = x.Select(z => z.Order.PaymentId).FirstOrDefault(),
                    Currency = x.Select(z => z.Currency).FirstOrDefault(),
                    OrderItems = x.Select(z => new DalOrderItemModel
                    {
                        Title = z.PrintingEdition.Title,
                        Count = z.Count,
                        PrintingEditionType = z.PrintingEdition.PrintingEditionType,
                        Amount = z.Amount,
                        Currency = z.Currency
                    }).ToList()
                });           

            return await Filtering(filterOrder, ordersList);
        }
        public async Task<IEnumerable<DalOrderModel>> GetAllOrdersAsync(FilterOrderModel filterOrder)
        {
            var ordersList = _context.OrderItems.Include(x => x.PrintingEdition).Include(x => x.Order).ThenInclude(x => x.User).GroupBy(x => x.OrderId)
                .Select(x => new DalOrderModel
                {
                    Id = x.Key,
                    Amount = x.Select(z => z.Amount).Sum(),
                    Date = x.Select(z => z.CreationDate).FirstOrDefault(),
                    Email = x.Select(z => z.Order.User.Email).FirstOrDefault(),
                    UserName = x.Select(z => string.Concat(z.Order.User.FirstName, " ", z.Order.User.LastName)).FirstOrDefault(),
                    TransactionStatus = x.Select(z => z.Order.TransactionStatus).FirstOrDefault(),
                    PaymentId = x.Select(z => z.Order.PaymentId).FirstOrDefault(),
                    Currency = x.Select(z => z.Currency).FirstOrDefault(),
                    OrderItems = x.Select(z => new DalOrderItemModel
                    {
                        Title = z.PrintingEdition.Title,
                        Count = z.Count,
                        PrintingEditionType = z.PrintingEdition.PrintingEditionType,
                        Amount = z.Amount,
                        Currency = z.Currency
                    }).ToList()
                });

            return await Filtering(filterOrder, ordersList);
        }
    }
}
