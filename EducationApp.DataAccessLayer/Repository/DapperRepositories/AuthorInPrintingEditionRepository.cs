using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Linq;
using EducationApp.DataAccessLayer.Repository.Base;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class AuthorInPrintingEditionRepository : BaseDapperRepository<AuthorInPrintingEdition>, IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CreateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
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
            using (var connect = GetSqlConnection())
            {
                return await connect.InsertAsync(entities);
            }
        }
        public async Task<int> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId)
        {
            var sql = $@"SELECT aPe.Id, aPe.AuthorId, aPe.PrintingEditionId
                         FROM AuthorInPrintingEditions AS aPe
                         WHERE ape.PrintingEditionId = {printingEditionId};";

            var authorsInPrintingEdition = new List<AuthorInPrintingEdition>();

            using(var connection = GetSqlConnection())
            {
                authorsInPrintingEdition = (await connection.QueryAsync<AuthorInPrintingEdition>(sql)).ToList();
            }

            var sameIds = authorsInPrintingEdition.Select(x => x.AuthorId).ToArray();
            var isEqual = sameIds.SequenceEqual(authorsId.OrderBy(x => x));

            if (isEqual)
            {
                return 0;
            }

            var removedAuthors = authorsInPrintingEdition.ToArray(); // todo why range?

            using(var connection = GetSqlConnection())
            {
                await connection.DeleteAsync(removedAuthors);
            }

            var result = await CreateAuthorsInPrintingEditionAsync(printingEditionId, authorsId);

            return result;
        }

        public async Task<bool> DeleteAuthorsById(long authorsId)
        {
            var sql = $"DELETE FROM AuthorInPrintingEditions WHERE AuthorId = {authorsId}";

            var result = await DeleteBySqlAsync(sql);

            return result;
        }
        public async Task<bool> DeletePrintingEditionsById(long printingEditionId)
        {
            var sql = $"DELETE FROM AuthorInPrintingEditions WHERE PrintingEditionId = {printingEditionId}";

            var result = await DeleteBySqlAsync(sql);

            return result;
        }
    }
}
