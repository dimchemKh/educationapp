using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models
{
    public class ConverterModel
    {
        public Enums.Currency CurrencyFrom { get; set; }
        public Enums.Currency CurrencyTo { get; set; }
        public decimal Price { get; set; }
    }
}
