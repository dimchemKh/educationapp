using EducationApp.BusinessLayer.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> GetUserOrdersAsync(OrderModel orderModel, string userId);
        Task<OrderModel> AddOrderAsync(OrderModel orderModel, OrderModelItem orderModelItem);
    }
}
