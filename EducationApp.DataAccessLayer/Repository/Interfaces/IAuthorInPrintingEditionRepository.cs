using EducationApp.DataAccessLayer.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository
    {
        Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> AddAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> DeleteAuthorsById(long auhorsId);
        Task<bool> DeletePrintingEditionsById(long printingEditionsId);
    }
}
