using EducationApp.BusinessLogic.Helpers.Interfaces;
using EducationApp.BusinessLogic.Common.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EducationApp.BusinessLogic.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        private bool ParseBoolData(IConfigurationSection section, string sectionName)
        {
            if(bool.TryParse(section.GetSection(sectionName).Value, out bool _result)){
                return _result;
            }
            return false;
        }
        private int ParseIntData(IConfigurationSection section, string sectionName)
        {
            if (int.TryParse(section.GetSection(sectionName).Value, out int _result))
            {
                return _result;
            }
            return 0;
        }
        public string GenerateRandomPassword(IConfiguration configuration)
        {
            var section = configuration.GetSection("PasswordOptions");
            var options = new PasswordOptions()
            {
                RequireLowercase = ParseBoolData(section, "RequireLowercase"),
                RequireUppercase = ParseBoolData(section, "RequireUppercase"),
                RequireDigit = ParseBoolData(section, "RequireDigit"),
                RequiredLength = ParseIntData(section, "RequiredLength"),
                RequiredUniqueChars = ParseIntData(section, "RequiredUniqueChars"),
                RequireNonAlphanumeric = ParseBoolData(section, "RequireNonAlphanumeric"),
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
