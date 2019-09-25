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

            string[] randomChars = new[] 
            {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    
                "abcdefghijkmnopqrstuvwxyz",    
                "0123456789"                    
            };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (options.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (options.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (options.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);            

            for (int i = chars.Count; i < options.RequiredLength || chars.Distinct().Count() < options.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
