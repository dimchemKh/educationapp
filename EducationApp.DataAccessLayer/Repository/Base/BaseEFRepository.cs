using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Common.Constants;
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
            return _dbSet.Where(x => x.IsRemoved == false);
        }
        public IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(x => x.IsRemoved == false).Where(predicate);
        }
        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            entity.IsRemoved = true;
            await _context.SaveChangesAsync();
        }
        public async Task EditAsync(TEntity entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;            
            await _context.SaveChangesAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        // TODO: need user override expression
        public IQueryable<TEntity> FilteringByProperty(Enums.SortType sortType, Enums.SortState sortState, IQueryable<TEntity> entities)
        {
            var list = new Dictionary<Enums.SortType, string>()
            {
                { Enums.SortType.Id, Constants.SortProperties.Id },
                { Enums.SortType.Name, Constants.SortProperties.Name },
                { Enums.SortType.PrintingEditionType, Constants.SortProperties.PrintingEditionType },
                { Enums.SortType.Price, Constants.SortProperties.Price }
            };
            foreach (var item in list)
            {
                if (item.Key == sortType && sortState == Enums.SortState.Asc)
                {
                    // TODO: custom linq OrderBy()
                }
                if(item.Key == sortType && sortState == Enums.SortState.Desc)
                {
                    // TODO: custom linq OrderByDescending()
                }
            }
            return entities;
        }
        public async Task<IEnumerable<TModel>> PaginationAsync<TModel>(BaseFilterModel baseFilter, Expression<Func<TModel, object>> predicate, IQueryable<TModel> entities)
        {
            if (baseFilter.SortState == Enums.SortState.Asc)
            {
                entities = entities.OrderBy(predicate);
            }
            if (baseFilter.SortState == Enums.SortState.Desc)
            {
                entities = entities.OrderByDescending(predicate);
            }
            var result = await entities.Skip((baseFilter.Page - 1) * baseFilter.PageSize).Take(baseFilter.PageSize).ToListAsync();

            return result;
        }
       
    }
}
