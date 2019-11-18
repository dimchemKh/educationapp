﻿using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserUpdateModel> UpdateUserProfileAsync(UserUpdateModel userModel, bool isAdmin);
        Task<UserUpdateModel> GetOneUserAsync(string userId);
        Task<UserModel> GetAllUsersAsync(FilterUserModel filterUserModel);
        Task<UserModel> DeleteUserAsync(long userId);
        Task<UserModel> BlockUserAsync(long userId, bool isBlocked);
    }
}
