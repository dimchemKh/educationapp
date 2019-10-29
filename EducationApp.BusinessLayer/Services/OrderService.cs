using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLayer.Helpers.Mappers;
using EducationApp.BusinessLayer.Common;

namespace EducationApp.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapperHelper _mapperHelper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMapperHelper mapperHelper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapperHelper = mapperHelper;
        }
        public async Task<OrderModel> GetOrdersAsync(FilterOrderModel filterOrder, string userId)
        {
            var responseModel = new OrderModel();

            var repositoryFilter = _mapperHelper.Map<FilterOrderModel, DataFilter.FilterOrderModel>(filterOrder);
            if (!long.TryParse(userId, out long _userId) || _userId == 0)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }
            var orders = (await _orderRepository.GetAllOrdersAsync(repositoryFilter, _userId)).ToList();
            
            foreach (var order in orders)
            {                         
                var orderModelItem = order.MapToModel();
                responseModel.Items.Add(orderModelItem);
            }            
            return responseModel;
        }
        public async Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, string userId)
        {
            var responseModel = new OrderModel();

            if (!orderModelItem.OrderItems.Any())
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            if(!long.TryParse(userId, out long _userId) || _userId == 0)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }            
            var orderItems = new List<OrderItem>();

            var order = new Order();
            var user = await _userRepository.GetUserByIdAsync(_userId);
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }
            order.User = user;
            foreach (var orderPrintingEdition in orderModelItem.OrderItems)
            {
                var orderItem = orderPrintingEdition.MapToEntity();
                orderItem.Order = order;
                orderItems.Add(orderItem);
            }
            var payment = new Payment()
            {
                TransactionId = null
            };

            var mappedOrder = orderItems.MapToEntity(order, payment);

            await _orderRepository.CreateAsync(mappedOrder);

            return responseModel;
        }
        public async Task<OrderModel> CreateTransactionAsync(string orderId, string transactionId)
        {
            var responseModel = new OrderModel();
            
            if(string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(transactionId))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidTransaction);
                return responseModel;
            }
            if(!long.TryParse(orderId, out long _orderId) || !long.TryParse(transactionId, out long _transactionId))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var updateReuslt = await _orderRepository.UpdateTransactionAsync(_orderId, _transactionId);
            if (!updateReuslt)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
            }
            return responseModel;
        }
    }
}
