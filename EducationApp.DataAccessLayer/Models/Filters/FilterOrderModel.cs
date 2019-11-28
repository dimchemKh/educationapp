using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Filters.Base;

namespace EducationApp.DataAccessLayer.Models.Filters
{
    public class FilterOrderModel : BaseFilterModel
    {
        public Enums.TransactionStatus TransactionStatus { get; set; }
    }
}
