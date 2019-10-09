using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters
{
    public class FilterPrintingEditionModel : BaseFilterModel
    {
        public string SearchByBody { get; set; }
        public Enums.Currency Currency { get; set; }
        public decimal[] RangePrice { get; set; }
        public ICollection<Enums.PrintingEditionType> PrintingEditionTypes { get; set; }

        public FilterPrintingEditionModel() : base()
        {
            Currency = Enums.Currency.USD;
            RangePrice = new decimal[2] 
            {
                0.0m,
                1000.0m
            };
            PrintingEditionTypes = new List<Enums.PrintingEditionType>();
        }
    }
}
