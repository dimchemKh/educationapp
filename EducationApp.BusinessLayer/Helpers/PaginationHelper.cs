using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EducationApp.BusinessLayer.Helpers
{
    public class PaginationHelper<TEntity> : IPaginationHelper<TEntity>
    {
        public IEnumerable<TEntity> Pagination(IQueryable<TEntity> entities, BaseFilterModel filterModel)
        {
            return entities.Skip((filterModel.Page - 1) * (int)filterModel.PageSize).Take((int)filterModel.PageSize).ToList();
        }
    }
}
