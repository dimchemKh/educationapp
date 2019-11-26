using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.OrderItems;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Orders
{
    public class OrderModelItem : BaseModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.TransactionStatus TransactionStatus { get; set; }
        public ICollection<OrderItemModelItem> OrderItems { get; set; }
    }
}
