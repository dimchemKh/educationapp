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
        public async Task<IEnumerable<DAOrderModel>> GetOrdersAsync(FilterOrderModel filterOrder, long userId)
        {
            var ordersList = _context.OrderItems.Include(x => x.PrintingEdition).Include(x => x.Order).ThenInclude(x => x.User).Where(x => x.Order.User.Id == userId).GroupBy(x => x.OrderId)
                .Select(x => new DAOrderModel
                {
                    Id = x.Key,
                    Amount = x.Select(z => z.Amount).Sum(),
                    Date = x.Select(z => z.CreationDate).FirstOrDefault(),
                    Email = x.Select(z => z.Order.User.Email).FirstOrDefault(),
                    UserName = x.Select(z => z.Order.User.UserName).FirstOrDefault(),
                    TransactionStatus = x.Select(z => z.Order.TransactionStatus).FirstOrDefault(),
                    OrderItems = x.Select(z => new DAOrderItemModel
                    {
                        Title = z.PrintingEdition.Title,
                        Count = z.Count,
                        PrintingEditionType = z.PrintingEdition.PrintingEditionType,
                        Amount = z.Amount,
                        Currency = z.Currency,
                        
                    } ).ToList()
                });

            Expression<Func<DAOrderModel, object>> lambda = null;
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

            if (filterOrder.SortState == Enums.SortState.Asc)
            {
                ordersList = ordersList.OrderBy(lambda);
            }
            if (filterOrder.SortState == Enums.SortState.Desc)
            {
                ordersList = ordersList.OrderByDescending(lambda);
            }
            var result = ordersList.Skip((filterOrder.Page - 1) * filterOrder.PageSize).Take(filterOrder.PageSize).ToList();   
            return result;
        }
    }
}
