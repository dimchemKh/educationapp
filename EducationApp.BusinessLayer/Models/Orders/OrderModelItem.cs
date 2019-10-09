using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Orders
{
    public class OrderModelItem
    {
        public long Id { get; set; }
        public DateTime OrderTime { get; set; }
        public Enums.Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public Enums.TransactionStatus TranscationStatus { get; set; }
        public UserShortModel User { get; set; }
        public ICollection<PrintingEditionShortModel> PrintingEditions { get; set; }
    }
}
