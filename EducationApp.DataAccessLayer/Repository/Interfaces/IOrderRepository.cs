using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.Orders;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IOrderRepository : IBaseEFRepository<Order>
    {
        Task<IEnumerable<DAOrderModel>> GetOrdersAsync(FilterOrderModel filterOrder, long userId);
    }
}
