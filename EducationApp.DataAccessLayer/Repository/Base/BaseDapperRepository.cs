using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using Dapper;

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
            using(var connection = GetSqlConnection())
            {
                return await connection.InsertAsync(entity);
            }
        }
        public async Task<int> DeleteAsync(TEntity entity)
        {
            var result = false;
            using(var connection = GetSqlConnection())
            {
                entity.IsRemoved = true;
                result = await connection.UpdateAsync(entity);
            }
            var response = result ? 1 : 0;
            return response;
        }
        public async Task<TEntity> GetByIdAsync(long id)
        {
            using(var connection = GetSqlConnection())
            {
                return await connection.GetAsync<TEntity>(id);
            }
        }
        public IQueryable<TEntity> ReadAll() => throw new NotImplementedException();
        public IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate) => throw new NotImplementedException();
        public Task<int> SaveAsync() => throw new NotImplementedException();
        public async Task<int> UpdateAsync(TEntity entity)
        {
            var result = false;
            using (var connection = GetSqlConnection())
            {
                result = await connection.UpdateAsync(entity);
            }
            var response = result ? 1 : 0;
            return response;
        }        
        protected async Task<bool> DeleteBySqlAsync(string sql)
        {
            using (var connection = GetSqlConnection())
            {
                await connection.QueryAsync(sql);
            }
            return true;
        }
        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }
    }
}
