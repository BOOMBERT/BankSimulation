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
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(ba => ba.Money)
                .HasPrecision(18, 2);

            builder.HasMany(ba => ba.Deposits)
                .WithOne(d => d.BankAccount)
                .HasForeignKey(d => d.BankAccountNumber)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ba => ba.Withdraws)
                .WithOne(w => w.BankAccount)
                .HasForeignKey(w => w.BankAccountNumber)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ba => ba.SentTransfers)
                .WithOne(t => t.SenderBankAccount)
                .HasForeignKey(t => t.SenderBankAccountNumber)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ba => ba.ReceivedTransfers)
                .WithOne(t => t.RecipientBankAccount)
                .HasForeignKey(t => t.RecipientBankAccountNumber)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(ba => ba.Number)
                .IsUnique();
        }
    }
}
