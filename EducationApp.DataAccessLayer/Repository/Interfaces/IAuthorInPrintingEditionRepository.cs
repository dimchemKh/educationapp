using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IAuthorInPrintingEditionRepository
    {
        Task<int> UpdateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<int> CreateAuthorsInPrintingEditionAsync(long printingEditionId, long[] authorsId);
        Task<bool> DeleteAuthorsById(long auhorsId);
        Task<bool> DeletePrintingEditionsById(long printingEditionsId);
    }
}
