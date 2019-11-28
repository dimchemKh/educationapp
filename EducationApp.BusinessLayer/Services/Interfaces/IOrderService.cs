using EducationApp.BusinessLogic.Models;
using EducationApp.BusinessLogic.Models.Filters;
using EducationApp.BusinessLogic.Models.Orders;
using EducationApp.BusinessLogic.Models.Payments;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<decimal> ConvertPriceAsync(ConverterModel converterModel);
        Task<OrderModel> GetOrdersAsync(FilterOrderModel filterOrder, string userId);
        Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, string userId);
        Task<OrderModel> CreateTransactionAsync(PaymentModel model);
    }
}
