using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.DataAccessLayer.Initialization
{
    public class DataBaseInitialization : CreateDatabaseIfNotExists<ApplicationContext>
    {
        //public override void InitializeDatabase(ApplicationContext context)
        //{
            
        //}
        protected override void Seed(ApplicationContext context)
        {
            PrintingEdition pe1 = new PrintingEdition()
            {
                CreationDate = DateTime.Now,
                Description = "Some description",
                Currency = Enums.Currency.USD,
                Name = "some name",
                Price = 50,
                Status = Enums.Status.Paid,
                Type = Enums.BookType.Book
            };

            context.PrintingEditions.Add(pe1);
        }
    }
}
