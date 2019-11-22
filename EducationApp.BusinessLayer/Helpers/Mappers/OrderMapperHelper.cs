using EducationApp.BusinessLayer.Models.OrderItems;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Orders;
using System.Collections.Generic;
using System.Linq;

namespace EducationApp.BusinessLayer.Helpers.Mappers
{
    public static class OrderMapperHelper
    {

        public static OrderItem MapToEntity(this OrderItemModelItem orderItemModel)
        {
            var orderItem = new OrderItem
            {
                PrintingEditionId = orderItemModel.PrintingEditionId,
                Count = orderItemModel.Count,
                Amount = orderItemModel.Count * orderItemModel.Price,
                Currency = orderItemModel.Currency
            };

            return orderItem;
        }
        public static OrderModelItem MapToModel(this OrderDataModel orderDataModel)
        {
            var orderItem = new OrderModelItem
            {
                Id = orderDataModel.Id,
                Date = orderDataModel.CreationDate,
                Email = orderDataModel.Email,
                UserName = $"{orderDataModel.FirstName} {orderDataModel.LastName}", 
                Amount = orderDataModel.Amount
            };

            var result = orderDataModel.PaymentId == null ? Enums.TransactionStatus.UnPaid : Enums.TransactionStatus.Paid;
            orderItem.TransactionStatus = result;

            orderItem.Currency = orderDataModel.OrderItems.Select(x => x.Currency).FirstOrDefault();
            orderItem.OrderItems = new List<OrderItemModelItem>();
            foreach (var item in orderDataModel.OrderItems)
            {
                orderItem.OrderItems.Add(new OrderItemModelItem
                {
                    PrintingEditionType = item.PrintingEditionType,
                    Title = item.Title,
                    Count = item.Count
                });
            }

            return orderItem;
        }
    }
}
