using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models.OrderItems
{
    public class DAOrderItemModel
    {
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public Enums.Currency Currency { get; set; }
        public long PrintingEditionId { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
        public string Title { get; set; }
    }
}
