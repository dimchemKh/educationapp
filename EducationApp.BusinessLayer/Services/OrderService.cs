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
        private readonly IMapperHelper _mapperHelper;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IUserRepository userRepository, 
                        IPrintingEditionRepository printingEditionRepository, IMapperHelper mapperHelper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            _printingEditionRepository = printingEditionRepository;
            _mapperHelper = mapperHelper;
        }
        public async Task<OrderModel> GetUserOrdersAsync(string userId)
        {
            var responseModel = new OrderModel();
            var ordersList = _orderItemRepository.ReadAll();

            foreach (var order in ordersList)
            {
                responseModel.Items.Add(new OrderModelItem()
                { 
                    // TODO:

                });
            }
            return null;
        }
        public OrderModel GetUsersOrdersForAdmin(FilterOrderModel filterModel)
        {
            var responseModel = new OrderModel();
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
            return responseModel;
        }
        public async Task<OrderModel> CreateOrderAsync(OrderModelItem orderModelItem, long userId)
        {
            var responseModel = new OrderModel();

            if (!orderModelItem.PrintingEditions.Any()
                || orderModelItem.User.Id == 0)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var orderItemsList = new List<OrderItem>();

            foreach (var orderPrintingEdition in orderModelItem.PrintingEditions)
            {
                var printingEdition = await _printingEditionRepository.GetByIdAsync(orderPrintingEdition.PrintingEditionId);
                if (printingEdition == null)
                {
                    responseModel.Errors.Add(Constants.Errors.InvalidData);
                    return responseModel;
                }
                var orderItem = new OrderItem();

                orderItem = _mapperHelper.MapToEntity(orderPrintingEdition, orderItem);
                
                orderItemsList.Add(orderItem);
            }
            
            
            var order = new Order();

            order.Amount = orderModelItem.PrintingEditions.Select(x => x.Amount).Sum();
            order.User.Id = userId;
            order.OrderItems = orderItemsList;
            order.CreationDate = DateTime.Now;
            


            await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveAsync();

            return responseModel;
        }
    }
}
