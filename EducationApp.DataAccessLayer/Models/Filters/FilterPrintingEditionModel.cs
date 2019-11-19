using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Models.Filters
{
    public class FilterPrintingEditionModel : BaseFilterModel
    {
        public string SearchString { get; set; }
        public Enums.Currency Currency { get; set; }
        public decimal PriceMinValue { get; set; }
        public decimal PriceMaxValue { get; set; }
        public IEnumerable<Enums.PrintingEditionType> PrintingEditionTypes { get; set; }
    }
}
