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

        public static OrderItem MapTo(this OrderItemModel orderItemModel)
        {
            var orderItem = new OrderItem();

            orderItem.PrintingEditionId = orderItemModel.PrintingEditionId;
            orderItem.Count = orderItemModel.Count;
            orderItem.Amount = orderItemModel.Count * orderItemModel.Price;
            orderItem.Currency = orderItemModel.Currency;

            return orderItem;
        }
        public static OrderModelItem MapTo(this OrderDataModel orderDataModel)
        {
            var orderItem = new OrderModelItem();

            orderItem.Id = orderDataModel.Id;
            orderItem.Date = orderDataModel.Date;
            orderItem.Email = orderDataModel.Email;
            orderItem.UserName = orderDataModel.UserName;
            orderItem.Amount = orderDataModel.Amount;
            if(orderDataModel.PaymentId == null)
            {
                orderItem.TransactionStatus = Enums.TransactionStatus.Unpaid;
            }
            if (orderDataModel.PaymentId != null)
            {
                orderItem.TransactionStatus = Enums.TransactionStatus.Paid;
            }
            orderItem.Currency = orderDataModel.OrderItems.Select(x => x.Currency).FirstOrDefault();
            orderItem.OrderItems = new List<OrderItemModel>();
            foreach (var item in orderDataModel.OrderItems)
            {
                orderItem.OrderItems.Add(new OrderItemModel
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
