using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Filters
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
