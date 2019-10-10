using EducationApp.BusinessLayer.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> GetUserOrdersAsync(string userId);
        Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, long userId);
    }
}
