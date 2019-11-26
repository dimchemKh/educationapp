using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
        Task<int> CreateOrderItems(OrderItem[] orderItems);
    }
}
