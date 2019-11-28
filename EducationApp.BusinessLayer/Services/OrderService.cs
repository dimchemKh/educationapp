using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLogic.Models.Filters;
using EducationApp.BusinessLogic.Models.Orders;
using EducationApp.BusinessLogic.Services.Interfaces;
using EducationApp.BusinessLogic.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using EducationApp.BusinessLogic.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLogic.Helpers.Mappers;
using EducationApp.BusinessLogic.Helpers.Interfaces;
using EducationApp.BusinessLogic.Models;
using EducationApp.BusinessLogic.Models.Payments;

namespace EducationApp.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapperHelper _mapperHelper;
        private readonly ICurrencyConverterHelper _currencyConverterHelper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMapperHelper mapperHelper, ICurrencyConverterHelper currencyConverterHelper,
                            IPaymentRepository paymentRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapperHelper = mapperHelper;
            _currencyConverterHelper = currencyConverterHelper;
            _paymentRepository = paymentRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task<decimal> ConvertPriceAsync(ConverterModel converterModel)
        {
            var resultConverting = _currencyConverterHelper.Convert(converterModel.CurrencyFrom, converterModel.CurrencyTo, converterModel.Price);
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
            var payment = new Payment()
            {
                TransactionId = null
            };

            var paymentId = await _paymentRepository.CreateAsync(payment);

            var order = new Order()
            {
                Amount = orderModelItem.OrderItems.Sum(x => x.Price * x.Count),
                PaymentId = paymentId,
                UserId = user.Id
            };

            var orderId = await _orderRepository.CreateAsync(order);

            foreach (var orderPrintingEdition in orderModelItem.OrderItems)
            {
                var orderItem = orderPrintingEdition.MapToEntity();
                orderItem.OrderId = orderId;
                orderItems.Add(orderItem);
            }

            var orderItemsResult = await _orderItemRepository.CreateOrderItems(orderItems.ToArray());

            responseModel.Items.Add(new OrderModelItem
            {
                Id = orderId
            });

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
