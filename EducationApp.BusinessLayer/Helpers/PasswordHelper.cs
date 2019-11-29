using EducationApp.BusinessLogic.Helpers.Interfaces;
using EducationApp.BusinessLogic.Common.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EducationApp.BusinessLogic.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        private readonly IOptions<PasswordOptions> _passwordOptions;
        public PasswordHelper(IOptions<PasswordOptions> passwordOptions)
        {
            _passwordOptions = passwordOptions;
        }
        public string GenerateRandomPassword(IConfiguration configuration)
        {
            var section = configuration.GetSection("PasswordOptions");
            var options = new PasswordOptions()
            {
                RequireLowercase = _passwordOptions.Value.RequireLowercase,
                RequireUppercase = _passwordOptions.Value.RequireUppercase,
                RequireDigit = _passwordOptions.Value.RequireDigit,
                RequiredLength = _passwordOptions.Value.RequiredLength,
                RequiredUniqueChars = _passwordOptions.Value.RequiredUniqueChars,
                RequireNonAlphanumeric = _passwordOptions.Value.RequireNonAlphanumeric
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
