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
                    RequiredLength = Constants.PasswordsOptions.RequiredLength,
                    RequiredUniqueChars = Constants.PasswordsOptions.RequiredUniqueChars,
                    RequireDigit = Constants.PasswordsOptions.RequireDigit,
                    RequireLowercase = Constants.PasswordsOptions.RequireLowercase,
                    RequireNonAlphanumeric = Constants.PasswordsOptions.RequireNonAlphanumeric,
                    RequireUppercase = Constants.PasswordsOptions.RequireUppercase
                };
            }

            string[] charsForPassword = new[]
            {
                Constants.CharsForPassword.UppercaseChars,
                Constants.CharsForPassword.LowercaseChars,
                Constants.CharsForPassword.DigitChars
            };

            var random = new Random(Environment.TickCount);
            var listCharsOfPassword = new List<char>();

            if (options.RequireUppercase)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count),
                    charsForPassword[0][random.Next(0, charsForPassword[0].Length)]);
            }

            if (options.RequireLowercase)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count),
                    charsForPassword[1][random.Next(0, charsForPassword[1].Length)]);
            }

            if (options.RequireDigit)
            {
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count),
                    charsForPassword[2][random.Next(0, charsForPassword[2].Length)]);
            }

            for (int i = listCharsOfPassword.Count; i < options.RequiredLength || listCharsOfPassword.Distinct().Count() < options.RequiredUniqueChars; i++)
            {
                string rcs = charsForPassword[random.Next(0, charsForPassword.Length)];
                listCharsOfPassword.Insert(random.Next(0, listCharsOfPassword.Count),
                    rcs[random.Next(0, rcs.Length)]);
            }

            return new string(listCharsOfPassword.ToArray());
        }
    }
}
