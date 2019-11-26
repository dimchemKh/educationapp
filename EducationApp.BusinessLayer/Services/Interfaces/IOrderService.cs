using EducationApp.BusinessLayer.Models;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Models.Payments;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<decimal> ConvertingPriceAsync(ConverterModel converterModel);
        Task<OrderModel> GetOrdersAsync(FilterOrderModel filterOrder, string userId);
        Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, string userId);
        Task<OrderModel> CreateTransactionAsync(PaymentModel model);
    }
}
