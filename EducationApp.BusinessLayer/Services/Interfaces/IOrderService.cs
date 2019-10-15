using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> GetUserOrdersAsync(FilterOrderModel filterOrder, string userId, string role);
        Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, long userId);
        Task<OrderModel> CreateTransactionAsync(string orderId, string transactionId);
    }
}
