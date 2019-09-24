using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Initialization
{
    public static class DbBaseInitializing
    {

        public static void Seed(this ModelBuilder modebuilder)
        {
            modebuilder.Entity<Author>().HasData(
                new Author()
                {
                    Id = 1,
                    CreationDate = DateTime.Now,
                    IsRemoved = false,
                    Name = "TestAuthor"
                });
        }
    }
}
