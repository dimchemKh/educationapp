using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using EducationApp.DataAccessLayer.Repository.Base;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class AuthorInPrintingEditionRepository : IAuthorInPrintingEditionRepository
    {
        public readonly IConfiguration _configuration;
        public AuthorInPrintingEditionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> AddAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
        {
            var entities = new List<AuthorInPrintingEdition>();

            foreach(var id in authorsId)
            {
                entities.Add(new AuthorInPrintingEdition { AuthorId = id, PrintingEditionId = printingEditionId });    
            }
            using (var connect = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value))
            {
                await connect.InsertAsync(entities);
            }

            return true;
        }
        public Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId) => throw new NotImplementedException();

        public Task<bool> DeleteByIdAsync(Expression<Func<AuthorInPrintingEdition, bool>> predicate) => throw new NotImplementedException();
    }
}
