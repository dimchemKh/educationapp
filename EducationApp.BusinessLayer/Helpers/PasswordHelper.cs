using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationApp.BusinessLayer.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public string GenerateRandomPassword(PasswordOptions options = null)
        {
            if (options == null)
            {
                options = new PasswordOptions()
                {
                    RequireLowercase = Constants.PasswordsOptions.RequireLowercase,
                    RequireUppercase = Constants.PasswordsOptions.RequireUppercase,
                    RequireDigit = Constants.PasswordsOptions.RequireDigit,
                    RequiredLength = Constants.PasswordsOptions.RequiredLength,
                    RequiredUniqueChars = Constants.PasswordsOptions.RequiredUniqueChars,
                    RequireNonAlphanumeric = Constants.PasswordsOptions.RequireNonAlphanumeric
                };
            }            
            var random = new Random(Environment.TickCount);
            var listCharsOfPassword = new List<char>();

            if (options.RequireLowercase)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count),
                    Constants.CharsForPassword.ListCharsForPassword[0][random.Next(0, Constants.CharsForPassword.ListCharsForPassword[0].Length)]);
            }
            if (options.RequireUppercase)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count), 
                    Constants.CharsForPassword.ListCharsForPassword[1][random.Next(0, Constants.CharsForPassword.ListCharsForPassword[1].Length)]);
            }
            if (options.RequireDigit)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count), 
                    Constants.CharsForPassword.ListCharsForPassword[2][random.Next(0, Constants.CharsForPassword.ListCharsForPassword[2].Length)]);
            }
            
            for (int i = listCharsOfPassword.Count; i < options.RequiredLength || listCharsOfPassword.Distinct().Count() < options.RequiredUniqueChars; i++)
            {
                string result = Constants.CharsForPassword.ListCharsForPassword[random.Next(0, Constants.CharsForPassword.ListCharsForPassword.Length)];
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count), result[random.Next(0, result.Length)]);
            }

            return new string(listCharsOfPassword.ToArray());
        }
    }
}
