using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task<SignInResult> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IEnumerable<IdentityError>> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByIdAsync(long userId);
        Task<string> GenerateResetPasswordTokenAsync(ApplicationUser user);
        Task<string> GetEmailConfirmTokenAsync(ApplicationUser user);
        Task<IList<string>> GetRoleAsync(ApplicationUser user);
        Task<IEnumerable<IdentityError>> SignUpAsync(ApplicationUser user, string password);
        Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<GenericModel<ApplicationUser>> GetFilteredDataAsync(FilterUserModel model);

        Task BlockUserAsync(ApplicationUser user, bool isBlocked);
    }
}
