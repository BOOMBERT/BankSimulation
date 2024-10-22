using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.DbContexts.Configurations
{
    public sealed class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.Property(t => t.SenderAmount)
                .HasPrecision(18, 2);

            builder.Property(t => t.RecipientAmount)
                .HasPrecision(18, 2);
        }
    }
}
