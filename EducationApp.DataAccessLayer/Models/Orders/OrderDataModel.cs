using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.OrderItems;
using System;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Models.Orders
{
    public class OrderDataModel
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Enums.Currency Currency { get; set; }
        public string PaymentId { get; set; }
        public ICollection<OrderItemDataModel> OrderItems { get; set; }
    }
}
