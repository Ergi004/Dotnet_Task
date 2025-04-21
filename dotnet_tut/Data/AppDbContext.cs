using Microsoft.EntityFrameworkCore;
using dotnet_tut.Models;

namespace dotnet_tut.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //transactions
            mb.Entity<Transactions>()
              .Property(t => t.CreatedAt)
              .HasDefaultValueSql("GETDATE()");
            mb.Entity<Transactions>()
            .Property(t => t.TransactionType)
            .HasConversion<string>();
            mb.Entity<Transactions>()
            .Property(t => t.Status)
            .HasConversion<string>();
            //Customer
             mb.Entity<Customer>()
              .Property(t => t.CreatedAt)
              .HasDefaultValueSql("GETDATE()");

            // customer , transactions one - to - many relation
            mb.Entity<Transactions>()
            .HasOne(t => t.Customer)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
