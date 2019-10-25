﻿using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.Orders;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces
{
    public interface IOrderRepository : IBaseEFRepository<Order>
    {
        Task<IEnumerable<OrderDataModel>> GetAllOrdersAsync(FilterOrderModel filterOrder, long userId);
        Task<bool> UpdateTransactionAsync(long orderId, long transactionId);
    }
}