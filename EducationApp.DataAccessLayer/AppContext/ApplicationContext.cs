using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, Role, long>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Author>().HasData(
                new Author() { Id = 1, CreationDate = DateTime.Now, IsRemoved = false, Name = "Tom Jackson" },
                new Author() { Id = 2, CreationDate = DateTime.Now, IsRemoved = false, Name = "Jack Jill" },
                new Author() { Id = 3, CreationDate = DateTime.Now, IsRemoved = false, Name = "Dan Bolson" },
                new Author() { Id = 4, CreationDate = DateTime.Now, IsRemoved = false, Name = "Mark Avreliy" },
                new Author() { Id = 5, CreationDate = DateTime.Now, IsRemoved = false, Name = "Bob Tomhson" },
                new Author() { Id = 6, CreationDate = DateTime.Now, IsRemoved = false, Name = "Arthur Bang" }
                );
            builder.Entity<PrintingEdition>().HasData(
                new PrintingEdition()
                {
                    Id = 1,
                    CreationDate = DateTime.Now,
                    IsRemoved = false,
                    Name = "Babysister",
                    Description = "Text",
                    Price = 20,
                    Type = Entities.Enums.Enums.Type.Book
                },
                new PrintingEdition()
                {
                    Id = 2,
                    CreationDate = DateTime.Now,
                    IsRemoved = false,
                    Name = "Butcher",
                    Description = "Some Text",
                    Price = 40,
                    Type = Entities.Enums.Enums.Type.Book
                },
                new PrintingEdition()
                {
                    Id = 3,
                    CreationDate = DateTime.Now,
                    IsRemoved = false,
                    Name = "Planet after us",
                    Description = "Some Descript",
                    Price = 10,
                    Type = Entities.Enums.Enums.Type.Newspaper
                },
                new PrintingEdition()
                {
                    Id = 4,
                    CreationDate = DateTime.Now,
                    IsRemoved = false,
                    Name = "Peppka Shmepka",
                    Description = "Some descr",
                    Price = 30,
                    Type = Entities.Enums.Enums.Type.Journal
                });
            base.OnModelCreating(builder);
        }
    }
}
