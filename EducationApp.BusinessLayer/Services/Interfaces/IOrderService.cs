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
        Task<OrderModel> GetOrdersAsync(FilterOrderModel filterOrder, string userId);
        Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, string userId);
        Task<OrderModel> CreateTransactionAsync(string orderId, string transactionId);
    }
}
