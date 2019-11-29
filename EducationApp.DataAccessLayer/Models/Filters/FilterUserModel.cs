using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Filters.Base;

namespace EducationApp.DataAccessLayer.Models.Filters
{
    public class FilterUserModel : BaseFilterModel
    {
        public Enums.BlockState IsBlocked { get; set; }
    }
}
