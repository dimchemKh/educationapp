using EducationApp.BusinessLayer.Models.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IPaginationHelper<TEntity>
    {
        IEnumerable<TEntity> Pagination(IQueryable<TEntity> entities, BaseFilterModel filterModel);
    }
}
