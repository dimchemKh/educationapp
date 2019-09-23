using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task Authenticate(string email, string password) => throw new NotImplementedException();

        
        public async Task<ApplicationUser> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            var user = await _userRepository.SignUpAsync(firstName, lastName, email, password);

            return user;
        }
        public async Task<string> GetConfirmToken(ApplicationUser user)
        {
            return await _userRepository.GetEmailConfirmTokenAsync(user);

        }
    }
}
