using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionShortModel
    {
        public int PrintingEditionId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public Enums.Type Type { get; set; }
    }
}
