using Microsoft.EntityFrameworkCore;
using dotnet_tut.Models;

namespace dotnet_tut.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Transactions> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Transactions>()
              .Property(u => u.CreatedAt)
              .HasDefaultValueSql("GETDATE()");
        }
    }
}
