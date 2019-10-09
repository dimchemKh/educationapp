using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters
{
    public class FilterPrintingEditionDetailsModel
    {
        public long Id { get; set; }
        public Enums.Currency Currency { get; set; }
    }
}
