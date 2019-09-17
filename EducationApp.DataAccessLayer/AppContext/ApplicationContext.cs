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
                new Author() { Id = 1, CreationDate = DateTime.Now, IsRemoved = false, Name = "Tom Jackson" }
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
                });
                
            base.OnModelCreating(builder);
        }
    }
}
