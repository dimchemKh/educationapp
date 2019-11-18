using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using System.Linq;
using System.Linq.Expressions;
using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;

namespace EducationApp.DataAccessLayer.Repository.Base
{
    public class BaseDapperRepository<TEntity> : IBaseDapperRepository<TEntity> where TEntity : BaseEntity, new()
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
        public Task<IEnumerable<TModel>> PaginationAsync<TModel>(BaseFilterModel baseFilter, Expression<Func<TModel, object>> predicate, IQueryable<TModel> entities) => throw new NotImplementedException();
        public IQueryable<TEntity> ReadAll() => throw new NotImplementedException();
        public IQueryable<TEntity> ReadWhere(Expression<Func<TEntity, bool>> predicate) => throw new NotImplementedException();
        public Task<int> SaveAsync() => throw new NotImplementedException();
        public Task<bool> UpdateAsync(TEntity entity) => throw new NotImplementedException();

        protected SqlConnection SqlConnection()
        {
            return new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }
    }
}
