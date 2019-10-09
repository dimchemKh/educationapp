using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Models.Filters
{
    public class FilterPrintingEditionModel : BaseFilterModel
    {
        public string SearchByBody { get; set; }
        public Enums.Currency Currency { get; set; }
        public decimal[] RangePrice { get; set; }
        public ICollection<Enums.PrintingEditionType> PrintingEditionTypes { get; set; }
    }
}
