using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationApp.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, Role, long>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PrintingEdition> PrintingEditions { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {          
            builder.Entity<AuthorInPrintingEdition>()
            .HasKey(key => new { key.AuthorId, key.PrintingEditionId });

            builder.Entity<AuthorInPrintingEdition>()
                .HasOne(navigationName => navigationName.PrintingEdition)
                .WithMany(table => table.AuthorInPrintingEditions)
                .HasForeignKey(key => key.PrintingEditionId);

            base.OnModelCreating(builder);
        }
    }
}
