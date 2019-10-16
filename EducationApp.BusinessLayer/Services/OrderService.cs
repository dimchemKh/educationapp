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

namespace EducationApp.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapperHelper _mapperHelper;
        private readonly IConverterHelper _converterHelper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IPrintingEditionRepository printingEditionRepository, 
            IPaymentRepository paymentRepository, IMapperHelper mapperHelper, IConverterHelper converterHelper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _printingEditionRepository = printingEditionRepository;
            _mapperHelper = mapperHelper;
            _converterHelper = converterHelper;
            _paymentRepository = paymentRepository;
        }
        public async Task<OrderModel> GetUserOrdersAsync(FilterOrderModel filterOrder, string userId, string role)
        {
            var responseModel = new OrderModel();

            var repositoryFilter = _mapperHelper.MapToModelItem<FilterOrderModel, DataFilter.FilterOrderModel>(filterOrder);

            IEnumerable<DataModel.OrderDataModel> ordersList = null;
            if(role == DataConstants.Constants.Roles.Admin)
            {
                ordersList = await _orderRepository.GetAllOrdersAsync(repositoryFilter);
            }
            if(role != DataConstants.Constants.Roles.Admin)
            {
                ordersList = await _orderRepository.GetOrdersAsync(repositoryFilter, long.Parse(userId));
            }

            foreach (var order in ordersList)
            {
                var orderModelItem = _mapperHelper.MapToModelItem<OrderDataModel, OrderModelItem>(order);                

                responseModel.Items.Add(orderModelItem);
            }            
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

                var orderItem = _mapperHelper.MapToModelItem<OrderItemDataModel, OrderItem>(orderPrintingEdition);

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
        public async Task<OrderModel> CreateTransactionAsync(string orderId, string transactionId)
        {
            var responseModel = new OrderModel();
            
            if(string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(transactionId))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidTransaction);
                return responseModel;
            }            
            var payment = new Payment()
            {
                TransactionId = long.Parse(transactionId)
            };
            var result = await _paymentRepository.CreateTransactionAsync(long.Parse(orderId), payment);
            if (!result)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidTransaction);
                return responseModel;
            }
            return responseModel;
        }
    }
}
