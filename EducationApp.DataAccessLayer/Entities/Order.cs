using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Order : BaseEntity
    {
        public Enums.Enums.TransactionStatus TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public long UserId { get; set; }
        public ApplicationUser User { get; set; }
        public long? PaymentId { get; set; }
        public Payment Payment { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
