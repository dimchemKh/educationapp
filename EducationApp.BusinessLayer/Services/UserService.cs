using System.Threading.Tasks;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EducationApp.BusinessLayer.Services
{
    // adminka
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            if(string.IsNullOrWhiteSpace(currentPassword) && string.IsNullOrWhiteSpace(newPassword))
            {
                return await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);
            }
            return false;
        }

    }
}
