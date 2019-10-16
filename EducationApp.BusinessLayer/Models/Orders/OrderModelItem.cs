using EducationApp.BusinessLayer.Models.OrderItems;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.OrderItems;
using System;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Orders
{
    public class OrderModelItem
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.TransactionStatus TransactionStatus { get; set; }
        public long? PaymentId { get; set; }
        public ICollection<OrderItemDataModel> OrderItems { get; set; }
    }
}
