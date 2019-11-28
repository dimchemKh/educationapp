using EducationApp.BusinessLogic.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLogic.Models.Filters
{
    public class FilterPrintingEditionModel : BaseFilterModel
    {
        public Enums.Currency Currency { get; set; }
        public decimal PriceMinValue { get; set; }
        public decimal PriceMaxValue { get; set; }
        public ICollection<Enums.PrintingEditionType> PrintingEditionTypes { get; set; }

        public FilterPrintingEditionModel() : base()
        {
            Currency = Enums.Currency.USD;
            PrintingEditionTypes = new List<Enums.PrintingEditionType>();
        }
    }
}
