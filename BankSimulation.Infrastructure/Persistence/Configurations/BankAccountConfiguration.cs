using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.Persistence.Configurations
{
    internal sealed class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(ba => ba.Number)
                .HasMaxLength(6);

            builder.Property(ba => ba.Balance)
                .HasPrecision(18, 2);

            builder.Property(ba => ba.CreationDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasMany(ba => ba.Deposits)
                .WithOne(d => d.BankAccount)
                .HasForeignKey(d => d.BankAccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ba => ba.Withdraws)
                .WithOne(w => w.BankAccount)
                .HasForeignKey(w => w.BankAccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ba => ba.SentTransfers)
                .WithOne(t => t.SenderBankAccount)
                .HasForeignKey(t => t.SenderBankAccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ba => ba.ReceivedTransfers)
                .WithOne(t => t.RecipientBankAccount)
                .HasForeignKey(t => t.RecipientBankAccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ba => ba.Number)
                .IsUnique();
        }
    }
}
