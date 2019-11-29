using EducationApp.BusinessLogic.Models.OrderItems;
using EducationApp.BusinessLogic.Models.Orders;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Orders;
using System.Collections.Generic;
using System.Linq;

namespace EducationApp.BusinessLogic.Helpers.Mappers
{
    public static class OrderMapperHelper
    {

        public static OrderItem MapToEntity(this OrderItemModelItem orderItemModel)
        {
            var orderItem = new OrderItem();
            orderItem.PrintingEditionId = orderItemModel.PrintingEditionId;
            orderItem.Count = orderItemModel.Count;
            orderItem.Amount = orderItemModel.Count * orderItemModel.Price;
            orderItem.Currency = orderItemModel.Currency;

            return orderItem;
        }
        public static OrderModelItem MapToModel(this OrderDataModel orderDataModel)
        {
            var orderItem = new OrderModelItem();
            orderItem.Id = orderDataModel.Id;
            orderItem.Date = orderDataModel.CreationDate;
            orderItem.Email = orderDataModel.Email;
            orderItem.UserName = $"{orderDataModel.FirstName} {orderDataModel.LastName}";
            orderItem.Amount = orderDataModel.Amount;

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
