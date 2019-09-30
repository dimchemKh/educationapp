using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationApp.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, Role, long>
    {
        public DbSet<Author> Authors { get; set; }
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

            builder.Entity<AuthorInBooks>()
            .HasKey(key => new { key.AuthorId, key.PrintingEditionId });

            builder.Entity<AuthorInBooks>()
                .HasOne(navigationName => navigationName.Author)
                .WithMany(table => table.AuthorInBooks)
                .HasForeignKey(key => key.AuthorId);

            builder.Entity<AuthorInBooks>()
                //.HasOne(navigationName => navigationName.PrintingEdition)
                //.WithMany(table => table.AuthorInBooks)
                //.HasForeignKey(key => key.PrintingEditionId);
                .HasOne<AuthorInBooks>()
                .WithMany()
                .HasForeignKey(key => key.PrintingEditionId);

            base.OnModelCreating(builder);
        }
    }
}
