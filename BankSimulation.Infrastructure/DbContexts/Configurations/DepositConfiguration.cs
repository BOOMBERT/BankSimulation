using BankSimulation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSimulation.Infrastructure.DbContexts.Configurations
{
    public sealed class DepositConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.Property(d => d.Amount)
                .HasPrecision(18, 2);
        }
    }
}
