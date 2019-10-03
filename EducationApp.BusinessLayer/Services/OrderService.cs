using System.Threading.Tasks;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Repository.Interfaces;

namespace EducationApp.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task<OrderModel> GetUserOrdersAsync(OrderModel orderModel, string userId)
        {
            var ordersList = await _orderItemRepository.GetAllAsync();

            foreach (var order in ordersList)
            {
                orderModel.Items.Add(new OrderModelItem()
                {
                    //Id = order.Id,
                    //Title = order.
                    //Count = order.Count,
                    //Currency = order.Currency,

                });
            }
            return null;
        }
        public async Task<OrderModel> AddUserOrderAsync(OrderModel orderModel)
        {
            return null;
        }
    }
}
