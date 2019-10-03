using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Orders
{
    public class OrderModelItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Enums.Type Type { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public Enums.Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public Enums.Status Status { get; set; }
    }
}
