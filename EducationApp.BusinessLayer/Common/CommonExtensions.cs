using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
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

            return userInfoModel;
        }
        public static Order MapToEntity(this IList<OrderItem> orderItems, Order order, Payment payment)
        {
            order.Amount = orderItems.Select(x => x.Amount).Sum();
            order.OrderItems = orderItems;
            order.TransactionStatus = DataAccessLayer.Entities.Enums.Enums.TransactionStatus.UnPaid;
            order.Payment = payment;

            return order;
        }
    }
}
