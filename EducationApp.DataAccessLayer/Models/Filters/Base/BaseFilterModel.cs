using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.DataAccessLayer.Models.Filters.Base
{
    public class BaseFilterModel
    {
        public Enums.SortState SortState { get; set; }
        public Enums.SortType SortType { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
