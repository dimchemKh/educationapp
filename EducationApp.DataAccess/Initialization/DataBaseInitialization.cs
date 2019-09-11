using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.DataAccessLayer.Initialization
{
    public class DataBaseInitialization
    {
        public void Initialize(ApplicationContext context)
        {
            context.Database.EnsureCreated();

            if (context.Authors.Any())
            {
                return;
            }

            var authors = new Author[]
            {
                new Author{Name = "Carson"},
                new Author{Name = "A"},
                new Author{Name = "B"}
            };
            foreach (var author in authors)
            {
                context.Add(author);
            }
            context.SaveChanges();
        }
    }
}
