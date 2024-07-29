using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.DbContexts
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Withdraw> Withdraws { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(eb =>
            {
                eb.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(64);

                eb.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(64);

                eb.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

                eb.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(256);

                eb.HasOne(u => u.SecurityQuestion)
                .WithOne(sq => sq.User)
                .HasForeignKey<SecurityQuestion>(sq => sq.UserId);

                eb.HasMany(u => u.BankAccounts)
                .WithOne(ba => ba.User)
                .HasForeignKey(ba => ba.UserId)
                .OnDelete(DeleteBehavior.NoAction);

                eb.HasIndex(u => u.Email)
                .IsUnique();
            });

            modelBuilder.Entity<SecurityQuestion>(eb =>
            {
                eb.Property(sq => sq.Question)
                .IsRequired()
                .HasMaxLength(128);

                eb.Property(sq => sq.Answer)
                .IsRequired()
                .HasMaxLength(256);
            });

            modelBuilder.Entity<BankAccount>(eb =>
            {
                eb.Property(ba => ba.Number)
                .IsRequired()
                .HasMaxLength(26);

                eb.Property(ba => ba.Money)
                .HasPrecision(18, 2);

                eb.HasMany(ba => ba.Deposits)
                .WithOne(d => d.BankAccount)
                .HasForeignKey(d => d.BankAccountId)
                .OnDelete(DeleteBehavior.NoAction);

                eb.HasMany(ba => ba.Withdraws)
                .WithOne(w => w.BankAccount)
                .HasForeignKey(w => w.BankAccountId)
                .OnDelete(DeleteBehavior.NoAction);

                eb.HasMany(ba => ba.SentTransfers)
                .WithOne(t => t.SenderBankAccount)
                .HasForeignKey(t => t.SenderBankAccountId)
                .OnDelete(DeleteBehavior.NoAction);

                eb.HasMany(ba => ba.ReceivedTransfers)
                .WithOne(t => t.RecipientBankAccount)
                .HasForeignKey(t => t.RecipientBankAccountId)
                .OnDelete(DeleteBehavior.NoAction);

                eb.HasIndex(ba => ba.Number)
                .IsUnique();
            });

            modelBuilder.Entity<Withdraw>(eb =>
            {
                eb.Property(w => w.Amount)
                .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Deposit>(eb =>
            {
                eb.Property(d => d.Amount)
                .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Transfer>(eb =>
            {
                eb.Property(t => t.Amount)
                .HasPrecision(18, 2);
            });
        }
    }
}
