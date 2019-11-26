using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EducationApp.BusinessLayer.Common
{
    public static class CommonExtensions
    {
        public static UserInfoModel MapToInfoModel(this ApplicationUser user, UserInfoModel userInfoModel, string role)
        {
            userInfoModel.UserId = user.Id;
            userInfoModel.UserName = $"{user.FirstName} {user.LastName}";
            userInfoModel.UserRole = role;
            userInfoModel.Image = user.Image;

            return userInfoModel;
        }
        public static Order MapToEntity(this IList<OrderItem> orderItems, Order order, Payment payment)
        {
            var mappedOrderItems = new List<OrderItem>();

            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
                mappedOrderItems.Add(item);
            }
            order.Amount = mappedOrderItems.Select(x => x.Amount).Sum();
            order.OrderItems = mappedOrderItems;
            order.TransactionStatus = Enums.TransactionStatus.UnPaid;
            order.Payment = payment;
            order.PaymentId = payment.Id;
            order.UserId = order.User.Id;

            return order;
        }

    }
}
