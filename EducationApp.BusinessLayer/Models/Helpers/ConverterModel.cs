using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Helpers
{
    public class ConverterModel
    {
        public decimal CurrencyRate { get; set; }
        public IEnumerable<PrintingEdition> PrintingEditions { get; set; }
    }
}
