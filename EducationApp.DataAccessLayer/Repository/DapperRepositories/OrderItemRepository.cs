using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
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
            using(var connection = GetSqlConnection())
            {
                return await connection.InsertAsync(orderItems);
            }
        } 
    }
}
