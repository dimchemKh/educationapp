using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace EducationApp.DataAccessLayer.AppContext
{
    public class ApplicationContext 
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        //public ApplicationContext()
        //{
        //    Database.SetInitializer(new Initialization.DataBaseInitialization());
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PrintingEdition>(entity =>
        //    {
        //        entity.Property(e => { e.Name = "sd", e.Status = Enums.Status.Paid})
        //    });
        //}
    }
}
