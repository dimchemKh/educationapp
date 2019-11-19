using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Base.Interfaces
{
    public interface IBaseEFRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(long id);
        IQueryable<TEntity> ReadAll();
        IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate);
        Task<long> CreateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> SaveAsync();
    }
}
