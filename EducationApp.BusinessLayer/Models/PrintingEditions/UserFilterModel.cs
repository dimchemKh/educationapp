using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class UserFilterModel : AdminFilterModel
    {
        public string SearchByWord { get; set; }
        public IDictionary<Enums.RangePrice, decimal> RangePrice { get; set; } 
        public Enums.Currency Currency { get; set; } 
        
        public UserFilterModel() : base()
        {
            RangePrice = new Dictionary<Enums.RangePrice, decimal>();
            Currency = Enums.Currency.USD;
        }
    }
}
