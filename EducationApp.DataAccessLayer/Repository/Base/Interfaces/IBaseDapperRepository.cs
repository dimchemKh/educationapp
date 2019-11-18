using EducationApp.DataAccessLayer.Models.Filters.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Base.Interfaces
{
    public interface IBaseDapperRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(long id);
        IQueryable<TEntity> ReadAll();
        IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate);
        Task<long> CreateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<int> SaveAsync();
        Task<IEnumerable<TModel>> PaginationAsync<TModel>(BaseFilterModel baseFilter, Expression<Func<TModel, object>> predicate, IQueryable<TModel> entities);
    }
}
