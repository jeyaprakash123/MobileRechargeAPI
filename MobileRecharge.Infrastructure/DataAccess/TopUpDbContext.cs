using Microsoft.EntityFrameworkCore;
using MobileRecharge.Domain.Models;
using TopUpAPI.Models;

namespace TopUpAPI.DataAccess
{
    public class TopUpDbContext : DbContext
    {
        public TopUpDbContext(DbContextOptions<TopUpDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<BeneficiaryTopUpDetails> BeneficiaryTopUpDetails { get; set; }
        public DbSet<TopUpOption> TopUpOptions { get; set; }
        public DbSet<Login>  UserLogin{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u=>u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Beneficiary>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<BeneficiaryTopUpDetails>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Login>().Property(b => b.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<User>().HasData(
                new User { Id = 201, Username = "Prakash", IsVerified = true, AvailableBalance = 1450, TotalTopUpLimit = 3000 },
                new User { Id = 202, Username = "Pavithra", IsVerified = false, AvailableBalance = 2000, TotalTopUpLimit = 3000 }
                );

            modelBuilder.Entity<Beneficiary>().HasData(
                new Beneficiary {Id =11,UserId=201,Nickname="William",MonthlyTopUpLimit = 500 },
                new Beneficiary { Id = 12, UserId = 202, Nickname = "Robert", MonthlyTopUpLimit = 1000 }
              );

            modelBuilder.Entity<BeneficiaryTopUpDetails>().HasData(
                new BeneficiaryTopUpDetails { Id = 121, UserId = 201, BeneficiaryId = 11, Amount = 100, Charge = 1, MonthWise = 10, YearWise = 2024 },
                new BeneficiaryTopUpDetails { Id = 122, UserId = 202, BeneficiaryId = 12, Amount = 75, Charge = 1, MonthWise = 10, YearWise = 2024 }
                );

            modelBuilder.Entity<Login>().HasData(
                new Login { Id = 1, Username="Prakash" ,Password= BCrypt.Net.BCrypt.HashPassword("Strength") },
                new Login { Id = 2, Username = "Harini", Password = BCrypt.Net.BCrypt.HashPassword("Daddy") }
               );

            modelBuilder.Entity<TopUpOption>().HasData(
                new TopUpOption { Id = 1, Amount = 5 },
                new TopUpOption { Id = 2, Amount = 10 },
                new TopUpOption { Id = 3, Amount = 20 },
                new TopUpOption { Id = 4, Amount = 30 },
                new TopUpOption { Id = 5, Amount = 50 },
                new TopUpOption { Id = 6, Amount = 75 },
                new TopUpOption { Id = 7, Amount = 100 }
            );
        }
    }
}
