﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.BusinessLayer.Common.Constants;
using DataConstants = EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using DataModel = EducationApp.DataAccessLayer.Models.Orders;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.DataAccessLayer.Models.Orders;
using EducationApp.DataAccessLayer.Models.OrderItems;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLayer.Helpers.Mappers;

namespace EducationApp.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapperHelper _mapperHelper;
        private readonly ICurrencyConverterHelper _converterHelper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IPrintingEditionRepository printingEditionRepository, 
            IPaymentRepository paymentRepository, IMapperHelper mapperHelper, ICurrencyConverterHelper converterHelper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _printingEditionRepository = printingEditionRepository;
            _mapperHelper = mapperHelper;
            _converterHelper = converterHelper;
            _paymentRepository = paymentRepository;
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
            var result = await _orderRepository.GetAllOrdersAsync(repositoryFilter, _userId);
            var orderList = result.ToList();
            
            foreach (var order in orderList)
            {                         
                var orderModelItem = order.MapTo();
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
            var orderItemsList = new List<OrderItem>();

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
                var orderItem = orderPrintingEdition.MapTo();
                orderItem.Order = order;
                orderItemsList.Add(orderItem);
            }
            var payment = new Payment()
            {
                TransactionId = null
            };

            order.Amount = orderItemsList.Select(x => x.Amount).Sum();            
            order.OrderItems = orderItemsList;
            order.TransactionStatus = DataAccessLayer.Entities.Enums.Enums.TransactionStatus.Unpaid;            
            order.Payment = payment;

            await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveAsync();

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
            if(!await _orderRepository.UpdateTransactionAsync(_orderId, _transactionId))
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return responseModel;
            }
            return responseModel;
        }
    }
}
