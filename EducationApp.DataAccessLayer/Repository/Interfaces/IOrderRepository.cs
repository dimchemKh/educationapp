using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.Orders;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<GenericModel<OrderDataModel>> GetAllOrdersAsync(FilterOrderModel filterOrder, long userId);
        Task<bool> UpdateTransactionAsync(long orderId, string transactionId);
    }
}
