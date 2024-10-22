using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.DbContexts.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Withdraw> Withdraws { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new WithdrawConfiguration());
            modelBuilder.ApplyConfiguration(new DepositConfiguration());
            modelBuilder.ApplyConfiguration(new TransferConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
