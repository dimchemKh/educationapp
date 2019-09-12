using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationContext _contextDb;
        public GenericRepository(ApplicationContext contextDb)
        {
            _contextDb = contextDb;
        }
        public Task Create(TEntity entity) => throw new NotImplementedException();
        public Task Delete(int id) => throw new NotImplementedException();
        public IQueryable<TEntity> GetAll()
        {
            return _contextDb.Set<TEntity>().AsNoTracking();
        }
        public Task<TEntity> GetById(int id) => throw new NotImplementedException();
        public Task Update(int id, TEntity entity) => throw new NotImplementedException();
    }
}
