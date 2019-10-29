using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.OrderItems
{
    public class OrderItemModelItem
    {
        public long PrintingEditionId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
    }
}
