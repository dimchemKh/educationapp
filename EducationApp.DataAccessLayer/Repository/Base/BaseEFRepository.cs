using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Base
{
    public class BaseEFRepository<TEntity> : IBaseEFRepository<TEntity> where TEntity : BaseEntity
    {        
        protected ApplicationContext _context;
        protected DbSet<TEntity> _dbSet;
        public BaseEFRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<TEntity> GetByIdAsync(long id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public IQueryable<TEntity> ReadAll()
        {
            return _dbSet.AsNoTracking().Where(x => x.IsRemoved == false);
        }
        public IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(x => x.IsRemoved == false).Where(predicate);
        }
        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            entity.IsRemoved = true;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;            
            await _context.SaveChangesAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
       
        public async Task<IEnumerable<TModel>> PaginationAsync<TModel>(BaseFilterModel filter, Expression<Func<TModel, object>> predicate, IQueryable<TModel> entities)
        {
            if (filter.SortState.Equals(Enums.SortState.Asc))
            {
                entities = entities.OrderBy(predicate);
            }
            if (filter.SortState.Equals(Enums.SortState.Desc))
            {
                entities = entities.OrderByDescending(predicate);
            }

            var result = await entities
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToAsyncEnumerable()
                .ToList();
                
            return result;
        }

    }
}
