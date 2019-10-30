using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Helpers.Mappers
{
    public static class UserMapperHelper
    {
        public static ApplicationUser MapToEntity(this UserUpdateModel source, ApplicationUser instance)
        {
            instance.FirstName = source.FirstName;
            instance.LastName = source.LastName;
            instance.Email = source.Email;
            instance.UserName = $"{source.FirstName}{source.LastName}";

            return instance;
        }
    }
}
