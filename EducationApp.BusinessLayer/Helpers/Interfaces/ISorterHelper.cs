using System.Linq;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface ISorterHelper<T>
    {
        IQueryable<T> Sorting(Enums.SortType sortType, IQueryable<T> entitesList);
    }
}
