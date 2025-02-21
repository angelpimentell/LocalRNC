using Microsoft.EntityFrameworkCore;
using LocalRNC.Models;

namespace LocalRNC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Company> companies { get; set; }

    }
}
