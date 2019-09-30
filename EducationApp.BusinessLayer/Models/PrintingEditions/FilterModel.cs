using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class FilterModel
    {
        public string SearchByWord { get; set; }
        public ICollection<Enums.Type> Types { get; set; } 
        public decimal[] RangePrice { get; set; } 
        public Enums.Currency Currency { get; set; } 
        public Enums.StateSort SortPrice { get; set; } 

        public FilterModel()
        {
            Types = new List<Enums.Type>();
            RangePrice = new decimal[] { 0, 9999 };
            Currency = Enums.Currency.USD;
            SortPrice = Enums.StateSort.PriceDesc;
        }
    }
}
