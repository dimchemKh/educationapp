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
        private EmailHelper _emailHelper;
        public AccountService(IUserRepository userRepository, EmailHelper emailHelper)
        {
            _userRepository = userRepository;
            _emailHelper = emailHelper;
        }

        public Task Authenticate(string email, string password) => throw new NotImplementedException();

        
        public async Task<string> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            var user = await _userRepository.SignUpAsync(firstName, lastName, email, password);

            return await _userRepository.GetEmailConfirmTokenAsync(user);


            //await emailService.SendAsync(user, "Confirm your account",
            //    $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
            //return code;
        }
    }
}
