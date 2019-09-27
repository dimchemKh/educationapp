using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class FilterModel
    {
        public string SearchByWord { get; set; }
        public IList<Enums.Type> Types { get; set; }
        public decimal[] RangePrice { get; set; } = new decimal[] { 0, 9999 };
        public Enums.Currency Currency { get; set; }
        public Enums.StateSort SortPrice { get; set; }
    }
}
