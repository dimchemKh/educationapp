using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLayer.Helpers.Mappers;
using EducationApp.BusinessLayer.Common;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models;
using EducationApp.BusinessLayer.Models.Payments;

namespace EducationApp.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapperHelper _mapperHelper;
        private readonly ICurrencyConverterHelper _currencyConverterHelper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMapperHelper mapperHelper, ICurrencyConverterHelper currencyConverterHelper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapperHelper = mapperHelper;
            _currencyConverterHelper = currencyConverterHelper;
        }
        public async Task<decimal> ConvertingPriceAsync(ConverterModel converterModel)
        {
            var resultConverting = _currencyConverterHelper.Converting(converterModel.CurrencyFrom, converterModel.CurrencyTo, converterModel.Price);
            return resultConverting;
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

            var orders = await _orderRepository.GetAllOrdersAsync(repositoryFilter, _userId);

            responseModel.ItemsCount = orders.CollectionCount;

            foreach (var order in orders.Collection)
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
            
            var user = await _userRepository.GetUserByIdAsync(_userId);

            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }

            var order = new Order()
            {
                User = user
            };

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

            var orderId = await _orderRepository.CreateAsync(mappedOrder);

            responseModel.Items.Add(new OrderModelItem { Id = orderId });

            return responseModel;
        }
        public async Task<OrderModel> CreateTransactionAsync(PaymentModel payment)
        {
            var responseModel = new OrderModel();

            if (payment.OrderId == 0 || string.IsNullOrWhiteSpace(payment.TransactionId))
            {
                responseModel.Errors.Add(Constants.Errors.TransactionInvalid);
                return responseModel;
            }

            var updateReuslt = await _orderRepository.UpdateTransactionAsync(payment.OrderId, payment.TransactionId);

            if (!updateReuslt)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
            }

            return responseModel;
        }
    }
}
