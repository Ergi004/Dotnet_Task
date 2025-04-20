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
              .Property(t => t.CreatedAt)
              .HasDefaultValueSql("GETDATE()");
            mb.Entity<Transactions>()
            .Property(t => t.TransactionType)
            .HasConversion<string>();
             mb.Entity<Transactions>()
            .Property(t => t.Status)
            .HasConversion<string>();
        }
    }
}
