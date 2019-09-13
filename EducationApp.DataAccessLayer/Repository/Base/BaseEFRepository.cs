using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Base
{
    public class BaseEFRepository<TEntity> where TEntity : BaseEntity
    {
        
        private ApplicationContext _context;
        private DbSet<TEntity> _dbSet;

        public BaseEFRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }
        public async Task AddAsync(TEntity entity)
        {

            //_context.Set<TEntity>().Attach(entity);

            await _context.Set<TEntity>().AddAsync(entity);

        }
        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            entity.IsRemoved = true;
            
            await SaveChangesAsync();
        }
        public Task Edit(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return SaveChangesAsync(); 
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
