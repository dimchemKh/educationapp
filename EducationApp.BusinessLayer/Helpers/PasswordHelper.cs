using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Common.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationApp.BusinessLayer.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public string GenerateRandomPassword()
        {
            var options = new PasswordOptions()
            {
                RequireLowercase = Constants.PasswordOptions.RequireLowercase,
                RequireUppercase = Constants.PasswordOptions.RequireUppercase,
                RequireDigit = Constants.PasswordOptions.RequireDigit,
                RequiredLength = Constants.PasswordOptions.RequiredLength,
                RequiredUniqueChars = Constants.PasswordOptions.RequiredUniqueChars,
                RequireNonAlphanumeric = Constants.PasswordOptions.RequireNonAlphanumeric
            };
       
            var random = new Random(Environment.TickCount);
            var listCharsOfPassword = new List<char>();

            if (options.RequireLowercase)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count),
                    Constants.PasswordGenerationParams.ListCharsForPassword[0][random.Next(0, Constants.PasswordGenerationParams.ListCharsForPassword[0].Length)]);
            }
            if (options.RequireUppercase)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count), 
                    Constants.PasswordGenerationParams.ListCharsForPassword[1][random.Next(0, Constants.PasswordGenerationParams.ListCharsForPassword[1].Length)]);
            }
            if (options.RequireDigit)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count), 
                    Constants.PasswordGenerationParams.ListCharsForPassword[2][random.Next(0, Constants.PasswordGenerationParams.ListCharsForPassword[2].Length)]);
            }
            
            for (int i = listCharsOfPassword.Count; i < options.RequiredLength || listCharsOfPassword.Distinct().Count() < options.RequiredUniqueChars; i++)
            {
                string result = Constants.PasswordGenerationParams.ListCharsForPassword[random.Next(0, Constants.PasswordGenerationParams.ListCharsForPassword.Length)];
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count), result[random.Next(0, result.Length)]);
            }

            return new string(listCharsOfPassword.ToArray());
        }
    }
}
