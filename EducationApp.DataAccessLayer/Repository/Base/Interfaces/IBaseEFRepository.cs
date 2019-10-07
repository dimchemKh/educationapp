using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Base.Interfaces
{
    public interface IBaseEFRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> ReadAll();
        IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate);
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task EditAsync(TEntity entity);
        Task SaveAsync();
        IEnumerable<TEntity> FilteringPage(int page, int pageSize, IQueryable<TEntity> entities);
        IQueryable<TEntity> FilteringByProperty(Enums.SortType sortType, Enums.SortState sortState, IQueryable<TEntity> entities);
    }
}
