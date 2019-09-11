using System.Data.Entity;
using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.DataAccessLayer.AppContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public ApplicationContext()
        {
            Database.SetInitializer(new Initialization.DataBaseInitialization());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<>
        }
    }
}
