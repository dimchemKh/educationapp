using Microsoft.Extensions.Configuration;

namespace EducationApp.BusinessLogic.Helpers.Interfaces
{
    public interface IPasswordHelper
    {
        string GenerateRandomPassword(IConfiguration configuration);
    }
}
