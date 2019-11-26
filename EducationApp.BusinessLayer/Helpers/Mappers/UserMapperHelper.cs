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
            instance.Image = source.Image;

            return instance;
        }
        public static T MapToModel<T>(this ApplicationUser source) where T : UserModelItem, new()
        {
            var instance = new T();
            instance.FirstName = source.FirstName;
            instance.LastName = source.LastName;
            instance.Email = source.Email;
            instance.Id = source.Id;

            var resultBlocked = (source.LockoutEnd != null) ? true : false;
            instance.IsBlocked = resultBlocked;

            return instance;
        }
    }
}
