using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class OrderItemRepository : BaseDapperRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IConfiguration  configuration) : base(configuration)
        {
        }
        public async Task<int> CreateOrderItems(OrderItem[] orderItems)
        {
            using(var connection = SqlConnection())
            {
                return await connection.InsertAsync(orderItems);
            }
        } 
    }
}
