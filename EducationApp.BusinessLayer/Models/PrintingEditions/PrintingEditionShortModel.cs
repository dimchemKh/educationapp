using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionShortModel
    {
        public long PrintingEditionId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public Enums.PrintingEditionType Type { get; set; }
    }
}
