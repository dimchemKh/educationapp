using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository
    {
        Task<bool> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> CreateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> DeleteAuthorsById(long auhorsId);
        Task<bool> DeletePrintingEditionsById(long printingEditionsId);
    }
}
