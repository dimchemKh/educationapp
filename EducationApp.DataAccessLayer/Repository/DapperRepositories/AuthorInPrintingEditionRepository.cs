using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using EducationApp.DataAccessLayer.Repository.Base;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

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
                entities.Add(new AuthorInPrintingEdition
                {
                    AuthorId = id,
                    PrintingEditionId = printingEditionId
                });    
            }
            using (var connect = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value))
            {
                await connect.InsertAsync(entities);
            }

            return true;
        }
        public async Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
        {
            var sql = $@"SELECT aPe.Id, aPe.AuthorId, aPe.PrintingEditionId
                         FROM AuthorInPrintingEditions AS aPe
                         WHERE ape.PrintingEditionId = {printingEditionId};";

            var authorsInPrintingEdition = new List<AuthorInPrintingEdition>();

            using(var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value))
            {
                authorsInPrintingEdition = (await connection.QueryAsync<AuthorInPrintingEdition>(sql)).ToList();
            }

            var sameIds = authorsInPrintingEdition.Select(x => x.AuthorId).ToArray();
            var isEqual = sameIds.SequenceEqual(authorsId.OrderBy(x => x));

            if (isEqual)
            {
                return false;
            }

            var removeRange = authorsInPrintingEdition.ToArray();

            using(var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value))
            {
                await connection.DeleteAsync(removeRange);
            }

            var result = await AddAuthorsInPrintingEditionAsync(printingEditionId, authorsId);

            return result;
        }

        public async Task<bool> DeleteAuthorsById(long authorsId)
        {
            var sql = $"DELETE FROM AuthorInPrintingEditions WHERE AuthorId = {authorsId}";

            using(var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value))
            {
                await connection.QueryAsync(sql);
            }

            return true;
        }
        public async Task<bool> DeletePrintingEditionsById(long printingEditionId)
        {
            var sql = $"DELETE FROM AuthorInPrintingEditions WHERE PrintingEditionId = {printingEditionId}";

            using (var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value))
            {
                await connection.QueryAsync(sql);
            }

            return true;
        }
    }
}
