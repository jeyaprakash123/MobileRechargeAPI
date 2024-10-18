using BalanceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BalanceApi.DataAccess
{
    public class BalanceDbContext : DbContext
    {
        public BalanceDbContext(DbContextOptions<BalanceDbContext> options) : base(options) { }

        public DbSet<Balance> Balances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Balance>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}
