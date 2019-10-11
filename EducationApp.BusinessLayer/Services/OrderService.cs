using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.OrderItems;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;

namespace EducationApp.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IMapperHelper _mapperHelper;
        private readonly IConverterHelper _converterHelper;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IUserRepository userRepository, 
                        IPrintingEditionRepository printingEditionRepository, IMapperHelper mapperHelper, IConverterHelper converterHelper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            _printingEditionRepository = printingEditionRepository;
            _mapperHelper = mapperHelper;
            _converterHelper = converterHelper;
        }
        public async Task<OrderModel> GetUserOrdersAsync(FilterOrderModel filterOrder, string userId)
        {
            var responseModel = new OrderModel();

            var orderModelItem = new OrderModelItem();
            var orderItem = new OrderItemModel();

            var repositoryFilter = new DataFilter.FilterOrderModel();

            repositoryFilter = _mapperHelper.MapToModelItem(filterOrder, repositoryFilter);

            var ordersList = await _orderRepository.GetOrdersAsync(repositoryFilter, long.Parse(userId));

            foreach (var item in ordersList)
            {
                orderModelItem = _mapperHelper.MapToModelItem(item, orderModelItem);
                
                responseModel.Items.Add(orderModelItem);
            }
            
            return responseModel;
        }
        public OrderModel GetUsersOrdersForAdmin(FilterOrderModel filterModel)
        {
            var responseModel = new OrderModel();            

            return responseModel;
        }
        public async Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, long userId)
        {
            var responseModel = new OrderModel();

            if (!orderModelItem.OrderItems.Any())
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            var orderItemsList = new List<OrderItem>();

            var order = new Order();
            var user = await _userRepository.GetUserByIdAsync(userId);
            order.User = user;
            
            foreach (var orderPrintingEdition in orderModelItem.OrderItems)
            {
                var printingEdition = await _printingEditionRepository.GetByIdAsync(orderPrintingEdition.PrintingEditionId);

                if (printingEdition == null)
                {
                    responseModel.Errors.Add(Constants.Errors.InvalidData);
                    return responseModel;
                }

                var orderItem = new OrderItem();

                orderItem = _mapperHelper.MapToEntity(orderPrintingEdition, orderItem);

                var price = _converterHelper.Converting(printingEdition.Currency, orderPrintingEdition.Currency, printingEdition.Price);
                orderItem.Amount = orderItem.Count * price;
                orderItem.Order = order;

                orderItemsList.Add(orderItem);
            }                     
            
            order.Amount = orderItemsList.Select(x => x.Amount).Sum();
            
            order.OrderItems = orderItemsList;
            order.TransactionStatus = DataAccessLayer.Entities.Enums.Enums.TransactionStatus.Unpaid;
            order.Payment = null;

            await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveAsync();
            return responseModel;
        }
    }
}
