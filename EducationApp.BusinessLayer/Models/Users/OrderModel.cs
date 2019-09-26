using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public Enums.Enums.Status Status { get; set; }
    }
}
