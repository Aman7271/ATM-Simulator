using ATM_Simulator.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM_Simulator.Data
{
    public class ATMDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost\SQLEXPRESS03;
                  Database=ATMDatabase;
                  Trusted_Connection=True;
                  TrustServerCertificate=True;"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.BalanceAfterTransaction)
                .HasPrecision(18, 2);
        }
    }
}