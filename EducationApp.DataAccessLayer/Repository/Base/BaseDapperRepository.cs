using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;

namespace EducationApp.DataAccessLayer.Repository.Base
{
    public class BaseDapperRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public readonly IConfiguration _configuration;
        public BaseDapperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<long> CreateAsync(TEntity entity)
        {
            using(var connection = SqlConnection())
            {
                return await connection.InsertAsync(entity);
            }
        }
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            using(var connection = SqlConnection())
            {
                entity.IsRemoved = true;
                return await connection.UpdateAsync(entity);
            }
        }
        public async Task<TEntity> GetByIdAsync(long id)
        {
            using(var connection = SqlConnection())
            {
                connection.Open();
                return await connection.GetAsync<TEntity>(id);
            }
        }
        public IQueryable<TEntity> ReadAll() => throw new NotImplementedException();
        public IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate) => throw new NotImplementedException();
        public Task<int> SaveAsync()
        {
            using (var connection = SqlConnection())
            {                   
            }
            return null;
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            using (var connection = SqlConnection())
            {
                return await connection.UpdateAsync(entity);
            }
        }

        protected SqlConnection SqlConnection()
        {
            return new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        Task<int> IBaseRepository<TEntity>.DeleteAsync(TEntity entity) => throw new NotImplementedException();
        Task<int> IBaseRepository<TEntity>.UpdateAsync(TEntity entity) => throw new NotImplementedException();
    }
}
