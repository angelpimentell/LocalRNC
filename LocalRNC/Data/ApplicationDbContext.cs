using Microsoft.EntityFrameworkCore;
using LocalRNC.Models;

namespace LocalRNC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Company> companies { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User {Id = 1, Email = "admin@admin.com", Password = "admin", IsAdmin = true, CreatedAt = new DateTime(2025, 3, 20) }
            );
        }

    }
}
