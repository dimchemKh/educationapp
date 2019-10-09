using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Interfaces;

namespace EducationApp.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPrintingEditionRepository _printingEditionRepository;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IUserRepository userRepository, 
                        IPrintingEditionRepository printingEditionRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            _printingEditionRepository = printingEditionRepository;
        }
        public async Task<OrderModel> GetUserOrdersAsync(OrderModel orderModel, string userId)
        {
            var ordersList = _orderItemRepository.ReadAll();

            foreach (var order in ordersList)
            {
                orderModel.Items.Add(new OrderModelItem()
                {
                    // TODO:

                });
            }
            return null;
        }
        public OrderModel GetUsersOrdersForAdmin(OrderModel orderModel, FilterOrderModel filterModel)
        {
            var ordersList = _orderRepository.ReadAll();
            //ordersList = ordersList.Where(x => filterModel.TransactionStatus == x.Status);

            //var filteringList = _sorterHelper.Sorting(filterModel.SortType, ordersList);

            //var orders = _paginationHelper.Pagination(filteringList, filterModel);

            //foreach (var item in orders)
            //{
            //    orderModel.Items.Add(new OrderModelItem()
            //    {
            //        OrderId = item.Id,
            //        OrderTime = item.Date,
            //        User = new UserShortModel() { UserName = item.User.UserName, Email = item.User.Email },
            //        Status = item.Status
            //    });
            //}
            return orderModel;
        }
        public async Task<OrderModel> AddOrderAsync(OrderModel orderModel, OrderModelItem orderModelItem)
        {
            if (orderModelItem == null)
            {
                orderModel.Errors.Add(Constants.Errors.InvalidDataFromClient);
                return orderModel;
            } 
            if (orderModelItem.Currency == Enums.Currency.None
                || orderModelItem.Amount == 0
                || !orderModelItem.PrintingEditions.Any()
                || orderModelItem.User.UserId == 0)
            {
                orderModel.Errors.Add(Constants.Errors.InvalidData);
                return orderModel;
            }
            ICollection<OrderItem> orderItemsList = null;

            foreach (var orderPrintingEdition in orderModelItem.PrintingEditions)
            {
                var printingEdition = await _printingEditionRepository.GetByIdAsync(orderPrintingEdition.Id);
                orderItemsList.Add(new OrderItem()
                {
                    Currency = orderModelItem.Currency,
                    Count = orderPrintingEdition.Count,
                    Amount = orderPrintingEdition.Amount,
                    PrintingEdition =  printingEdition,
                });
            }

            var order = new Order()
            {
                Amount = orderModelItem.Amount,
                User = await _userRepository.GetUserByIdAsync(orderModelItem.User.UserId),
                OrderItems = orderItemsList
            };

            await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveAsync();

            return orderModel;
        }
    }
}
